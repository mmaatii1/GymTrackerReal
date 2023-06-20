using GymTrackerApiReal.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Xml;

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
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpecificExercise>(entity =>
            {
                entity.Property(e => e.Repetitions)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray());
            });
            modelBuilder.Entity<CustomWorkout>()
        .ToTable(tb => tb.HasTrigger("SomeTrigger"));
            modelBuilder.Entity<Exercise>()
      .ToTable(tb => tb.HasTrigger("SomeTrigger"));
            modelBuilder.Entity<Muscle>()
      .ToTable(tb => tb.HasTrigger("SomeTrigger"));
            modelBuilder.Entity<SpecificExercise>()
      .ToTable(tb => tb.HasTrigger("SomeTrigger"));
            modelBuilder.Entity<WorkoutPlan>()
      .ToTable(tb => tb.HasTrigger("SomeTrigger"));
            modelBuilder.Entity<CustomWorkout>()
    .HasOne(c => c.WorkoutPlan)
    .WithMany(w => w.DoneWorkouts)
    .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CustomWorkout>()
        .HasMany(cw => cw.CustomWorkoutSpecificExercises)
        .WithOne(se => se.CustomWorkout)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SpecificExercise>()
      .HasOne(se => se.CustomWorkout)
      .WithMany(cw => cw.CustomWorkoutSpecificExercises)
      .HasForeignKey(se => se.CustomWorkoutId)
      .IsRequired(false);
        }

      
    }
}
