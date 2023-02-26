using AutoMapper;
using GymTrackerApiReal.Data;
using GymTrackerApiReal.Interfaces;
using GymTrackerApiReal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
#endregion

#region setupDb
builder.Services.AddDbContext<TrackingDbContext>
    (o => o.UseSqlServer(connectionString: "Server=localhost\\SQLEXPRESS01;Database=master;TrustServerCertificate=True;Integrated Security=true; Initial Catalog = Tracker"));
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

app.MapGet("/api/customWorkout", async (HttpContext httpContext, IGenericRepository<CustomWorkout> repo, IMapper mapper) =>
{
    var muscle = new Muscle() { Id = 1, Name = "Klata", MainMasculeGroup = MainMuscleGroup.Chest };
    var muscle2 = new Muscle() { Id = 2, Name = "Plery", MainMasculeGroup = MainMuscleGroup.Back };
    var exerices = new Exercise() { Id = 1, Name = "Wyciskanie", Muscle = muscle };
    var exerices2 = new Exercise() { Id = 2, Name = "Martwy", Muscle = muscle2 };
    var specificEx = new SpecificExercise() { Id = 1, Exercise = exerices, Repetitions = 5, Sets = 5, Weight = 60 };
    var specificEx2 = new SpecificExercise() { Id = 2, Exercise = exerices2, Repetitions = 5, Sets = 5, Weight = 100 };
    var listOf = new List<SpecificExercise>() { specificEx,specificEx2 };
    var newCustomWorkout = new CustomWorkout() { DateOfWorkout = DateTime.Now, Id = 1, Name = "Mega workout", Exercises = listOf };
    var newCustomWorkout2 = new CustomWorkout() { DateOfWorkout = DateTime.MaxValue, Id = 2, Name = "Sztos workout", Exercises = listOf };
    var listOfCustomWorkout = new List<CustomWorkout>() { newCustomWorkout, newCustomWorkout2 };
    return Results.Ok(listOfCustomWorkout);


})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
