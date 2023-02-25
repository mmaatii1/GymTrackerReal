using GymTrackerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GymTrackerApi.Data
{
    public class TrackingDbContext : DbContext
    {
        public TrackingDbContext(DbContextOptions<TrackingDbContext> options) : base(options)
        {

        }

        public DbSet<CustomWorkout> CustomWorkouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<SpecificExercise> SpecificExercises { get; set;}
    }
}
