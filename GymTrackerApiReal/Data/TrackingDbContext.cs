using GymTrackerApiReal.Models;
using Microsoft.EntityFrameworkCore;

namespace GymTrackerApiReal.Data
{
    public class TrackingDbContext : DbContext
    {
        public TrackingDbContext(DbContextOptions<TrackingDbContext> options) : base(options)
        {

        }

        public DbSet<CustomWorkout> CustomWorkouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<SpecificExercise> SpecificExercises { get; set; }
    }
}
