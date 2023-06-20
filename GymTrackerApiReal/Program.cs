using AutoMapper;
using GymTrackerApiReal.Data;
using GymTrackerApiReal.Dtos.CustomWorkout;
using GymTrackerApiReal.Dtos.Exercise;
using GymTrackerApiReal.Dtos.Muscle;
using GymTrackerApiReal.Dtos.Picture;
using GymTrackerApiReal.Dtos.SpecificExercise;
using GymTrackerApiReal.Dtos.WorkoutPlan;
using GymTrackerApiReal.Interfaces;
using GymTrackerApiReal.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

using System.Reflection;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


#region services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "gymtracker.redis.cache.windows.net:6380,password=D3ELJeEXsc2dbWk3OsIUo7B0HpeDQLPuQAzCaKY3oL8=,ssl=True,abortConnect=False";
    options.InstanceName = "master";
});
#endregion

#region setupDb
builder.Services.AddDbContext<TrackingDbContext>
    // ""
    (o => o.UseSqlServer(connectionString: "Server=tcp:gymtrackerapirealdbserver.database.windows.net,1433;Initial Catalog=GymTrackerApiReal_db;Persist Security Info=False;User ID=Mati;Password=Szymonek12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
#endregion
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";

app.MapGet("api/WorkoutPhoto/{guid}", async (string guid) =>
{
    var blobClient= GetBlobClient.GetClientBasedOnGuid(guid);
    BlobDownloadInfo download = await blobClient.DownloadAsync();

    try
    {
        var photo = new WorkoutPhoto();
        using (MemoryStream ms = new MemoryStream())
        {
            await download.Content.CopyToAsync(ms);
            photo.PhotoAsBytes = ms.ToArray();
            photo.Guid = new Guid(guid);
        }
        return Results.Ok(photo);
    }
    catch (Exception)
    {
        return Results.NoContent();
    }

}).WithName("GetWorkoutPhoto");
app.MapPost("api/WorkoutPhoto", async(WorkoutPhoto photo )=>{

    var blobClient = GetBlobClient.GetClientBasedOnGuid(photo.Guid.ToString());

    FileStream fileStream;
    try
    {
    using (fileStream = new FileStream(photo.Guid.ToString(), FileMode.Create, FileAccess.ReadWrite))
    {
        fileStream.Write(photo.PhotoAsBytes, 0, photo.PhotoAsBytes.Length);
        fileStream.Position= 0;
        await blobClient.UploadAsync(fileStream, true);
        fileStream.Dispose();
        File.Delete(photo.Guid.ToString());
    }
    }
    catch(Exception ex)
    {
        return await Task.FromResult(false);
    }
    return await Task.FromResult(true);

}).WithName("WorkoutPhoto");

app.MapDelete("/api/CustomWorkout/{id}", async (int id,IGenericRepository < CustomWorkout> repo, IMapper mapper ) =>
{
    
    var workouts = await repo.DeleteAsync(id);

    return Results.Ok();
})
.WithName("DeleteWorkout");

app.MapGet($"/api/{nameof(CustomWorkout)}", async (IGenericRepository<CustomWorkout> repo, IMapper mapper, IDistributedCache redis) =>
{
    byte[] fromRedis = await redis.GetAsync("customworkouts");
    if ((fromRedis?.Count() ?? 0) > 0)
    {
        var workoutstring = Encoding.UTF8.GetString(fromRedis);
        List<CustomWorkout> listFromRedis = System.Text.Json.JsonSerializer.Deserialize<List<CustomWorkout>>(workoutstring);
        return Results.Ok(mapper.Map<List<CustomWorkoutReadDto>>(listFromRedis));
    }
    var workouts = await repo.GetAsQueryable()
                       .Include(c => c.CustomWorkoutSpecificExercises)
                       .ThenInclude(e => e.Exercise)
                       .ThenInclude(ex => ex.Muscle)
                       .Include(cw=>cw.WorkoutPlan)
                       .ToListAsync();
    var settings = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };
    var serialized = System.Text.Json.JsonSerializer.Serialize(workouts,options: settings);
    byte[] serializedBytes = Encoding.UTF8.GetBytes(serialized);
    DistributedCacheEntryOptions expiration = new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15),
        SlidingExpiration = TimeSpan.FromSeconds(15),
    };
    await redis.SetAsync("customworkouts", serializedBytes,expiration);

    return Results.Ok(mapper.Map<List<CustomWorkoutReadDto>>(workouts));
})
.WithName("GetWorkouts");

app.MapPost($"/api/{nameof(CustomWorkout)}", async (CustomWorkoutCreateUpdateDto workout, IGenericRepository<SpecificExercise> specExRepo, IGenericRepository<CustomWorkout> repo, IMapper mapper) =>
{
    if (workout is null)
    {
        return Results.BadRequest();
    }
    var specificExercises = await specExRepo.GetAsQueryable().Where(c => workout.SpecificExercisesIds.Contains(c.Id)).Include(c => c.Exercise).Include(c => c.Exercise.Muscle).ToListAsync();
    var toAdd = mapper.Map<CustomWorkout>(workout);

    toAdd.CustomWorkoutSpecificExercises = specificExercises;
    var added = await repo.AddAsync(toAdd);
    workout.Id = added.Id;

    CustomWorkoutAzureBus.SendToBus("customworkout", 1, workout);
    return Results.Created($"/{nameof(CustomWorkout)}/{added.Id}", workout);
})
.WithName("PostWorkout");

app.MapGet($"/api/{nameof(WorkoutPlan)}", async (IGenericRepository<WorkoutPlan> repo, IMapper mapper, IDistributedCache redis) =>
{
    byte[] fromRedis = await redis.GetAsync("workoutplan");
    if ((fromRedis?.Count() ?? 0) > 0)
    {
        var workoutstring = Encoding.UTF8.GetString(fromRedis);
        List<WorkoutPlan> listFromRedis = System.Text.Json.JsonSerializer.Deserialize<List<WorkoutPlan>>(workoutstring);
        return Results.Ok(mapper.Map<List<WorkoutPlanReadDto>>(listFromRedis));
    }
    var workoutsPlans = await repo.GetAsQueryable()
                       .Include(c=>c.DoneWorkouts)
                       .Include(c => c.Exercises)
                       .ThenInclude(a => a.Muscle)
                       .ToListAsync();
    var settings = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };
    var serialized = System.Text.Json.JsonSerializer.Serialize(workoutsPlans,settings);
    byte[] serializedBytes = Encoding.UTF8.GetBytes(serialized);
    DistributedCacheEntryOptions expiration = new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15),
        SlidingExpiration = TimeSpan.FromSeconds(15),
    };
    await redis.SetAsync("workoutplan", serializedBytes, expiration);

    var mapped = mapper.Map<List<WorkoutPlanReadDto>>(workoutsPlans);
    return Results.Ok(mapped);
})
.WithName("GetWorkoutsPlan");

app.MapPost($"/api/{nameof(WorkoutPlan)}", async (WorkoutPlanCreateDto plan, 
    IGenericRepository<WorkoutPlan> planRepo, IGenericRepository<Exercise> exerciseRepo, IMapper mapper) =>
{
    if (plan is null)
    {
        return Results.BadRequest();
    }
    var exercises = await exerciseRepo.GetAsQueryable().Where(c=>plan.ExercisesIds.Contains(c.Id)).ToListAsync();
    var toAdd = mapper.Map<WorkoutPlan>(plan);


    toAdd.Exercises = exercises;
    var added = await planRepo.AddAsync(toAdd);
    return Results.Created($"/{nameof(WorkoutPlan)}/{added.Id}", plan);
})
.WithName("PostWorkoutPlan");

app.MapPut($"/api/{nameof(WorkoutPlan)}", async (WorkoutPlanUpdateDto plan,
    IGenericRepository<WorkoutPlan> planRepo, IGenericRepository<Exercise> exerciseRepo, IGenericRepository<CustomWorkout> customWorkoutRepo, IMapper mapper) =>
{
    if (plan is null)
    {
        return Results.BadRequest();
    }

    var planToUpdate = await planRepo.GetAsQueryable().Include(c=>c.Exercises).Include(c=>c.DoneWorkouts).FirstOrDefaultAsync(c => c.Id == plan.Id);

    if (!planToUpdate.Exercises.Select(c => c.Id).OrderBy(i=>i)
    .SequenceEqual(plan.ExercisesIds.OrderBy(i=>i)))
    {
        planToUpdate.Exercises = await exerciseRepo.GetAsQueryable().Where(c => plan.ExercisesIds.Contains(c.Id)).ToListAsync();
    }
    if(plan.DoneWorkoutsIds is not null)
    {
        planToUpdate.DoneWorkouts = await customWorkoutRepo.GetAsQueryable().Where(c => plan.DoneWorkoutsIds.Contains(c.Id)).ToListAsync();
    }
    if (!string.IsNullOrWhiteSpace(plan.Name))
    {
        planToUpdate.Name = plan.Name;
    }

    var added = await planRepo.UpdateAsync(planToUpdate);
    return Results.Accepted($"/{nameof(WorkoutPlan)}/{added.Id}", plan);
})
.WithName("UpdateWorkoutPlan");

app.MapGet($"/api/{nameof(SpecificExercise)}", async (IGenericRepository<SpecificExercise> repo, IMapper mapper) =>
{
    var exercies = repo.GetAsQueryable().Include(c => c.Exercise).Include(c => c.Exercise.Muscle);
    return Results.Ok(mapper.Map<List<SpecificExerciseReadDto>>(exercies));
});

app.MapPost($"/api/{nameof(SpecificExercise)}", async (IGenericRepository<SpecificExercise> repo, IGenericRepository<Exercise> exerciseRepo, IMapper mapper, SpecificExerciseUpdateCreateDto specificExercise) =>
{
    if (specificExercise is null)
    {
        return Results.BadRequest();
    }
    var exercise = await exerciseRepo.GetByIdAsync(specificExercise.ExerciseId);
    if(exercise == null)
    {
        return Results.BadRequest();
    }
    var toAdd = mapper.Map<SpecificExercise>(specificExercise);
    toAdd.Exercise = exercise;

    var added = await repo.AddAsync(toAdd);
    specificExercise.Id = added.Id;
    return Results.Created($"/{nameof(SpecificExercise)}/{added.Id}", specificExercise);
});


app.MapGet($"/api/{nameof(Exercise)}", async (IGenericRepository<Exercise> repo, IMapper mapper) =>
{
    var exercies = repo.GetAsQueryable().Include(c => c.Muscle);
    return Results.Ok(mapper.Map<List<ExerciseReadDto>>(exercies));
});

app.MapPost($"/api/{nameof(Exercise)}", async (IGenericRepository<Exercise> repo, IGenericRepository<Muscle> muscleRepo ,IMapper mapper, ExerciseCreateUpdateDto exercise) =>
{
    if (exercise is null)
    {
        return Results.BadRequest();
    }
    var muscle = await muscleRepo.GetByIdAsync(exercise.MuscleId);
    if(muscle is null)
    {
        return Results.BadRequest();
    }
    var toAdd = new Exercise();
    toAdd.Muscle = muscle;
    toAdd.Name = exercise.Name;
    var added = await repo.AddAsync(toAdd);
    return Results.Created($"/{nameof(Exercise)}/{added.Id}", exercise);
});

app.MapGet($"/api/{nameof(Muscle)}", async (IGenericRepository<Muscle> repo, IMapper mapper) =>
{
    var muscle = await repo.GetAllAsync();
    return Results.Ok(mapper.Map<List<MuscleReadDto>>(muscle));
});

app.MapPost($"/api/{nameof(Muscle)}", async (IGenericRepository<Muscle> repo, IMapper mapper, MuscleUpdateCreateDto muscle) =>
{
    if (muscle is null)
    {
        return Results.BadRequest();
    }
    var added = await repo.AddAsync(mapper.Map<Muscle>(muscle));
    return Results.Created($"/{nameof(Muscle)}/{added.Id}", muscle);
});

app.Run();

