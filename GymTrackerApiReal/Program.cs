using AutoMapper;
using GymTrackerApiReal.Data;
using GymTrackerApiReal.Dtos.CustomWorkout;
using GymTrackerApiReal.Dtos.Exercise;
using GymTrackerApiReal.Dtos.Muscle;
using GymTrackerApiReal.Dtos.SpecificExercise;
using GymTrackerApiReal.Interfaces;
using GymTrackerApiReal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Net.WebSockets;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions
               .ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

#region services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
#endregion

#region setupDb
builder.Services.AddDbContext<TrackingDbContext>
    (o => o.UseSqlServer(connectionString: "Server=localhost\\SQLEXPRESS01;Database=master;TrustServerCertificate=True;Integrated Security=true; Initial Catalog = TrackerReal"));
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/api/customWorkout", async (HttpContext httpContext, IGenericRepository<CustomWorkout> repo, IGenericRepository<Exercise> exerciseRepo, IMapper mapper) =>
{
    var workouts = repo.GetAsQueryable()
                       .Include(c => c.CustomWorkoutSpecificExercises)
                       .ThenInclude(e => e.Exercise)
                       .ThenInclude(ex => ex.Muscle)
                       .ToList();
    return Results.Ok(workouts);
})
.WithName("GetWorkouts");

app.MapPost("/api/customWorkout", async (CustomWorkoutCreateUpdateDto workout, IGenericRepository<SpecificExercise> specExRepo, IGenericRepository<CustomWorkout> repo, IMapper mapper) =>
{
    if (workout is null)
    {
        return Results.BadRequest();
    }
    var specificExercises = await specExRepo.GetAsQueryable().Where(c => workout.SpecificExercisesIds.Contains(c.Id)).Include(c => c.Exercise).Include(c => c.Exercise.Muscle).ToListAsync();
    var toAdd = mapper.Map<CustomWorkout>(workout);

    // Attach SpecificExercises entities
    specificExercises.ForEach(se => specExRepo.Attach(se));

    toAdd.CustomWorkoutSpecificExercises = specificExercises;
    var added = await repo.AddAsync(toAdd);
    return Results.Created($"/todoitems/{added.Id}", added);
})
.WithName("PostWorkout");



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
    return Results.Created($"/{nameof(SpecificExercise)}/{added.Id}", added);
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
    return Results.Created($"/{nameof(Exercise)}/{added.Id}", added);
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
    return Results.Created($"/{nameof(Muscle)}/{added.Id}", added);
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
