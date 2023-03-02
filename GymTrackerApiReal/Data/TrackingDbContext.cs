﻿using GymTrackerApiReal.Models;
using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpecificExercise>(entity =>
            {
                entity.Property(e => e.Repetitions)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray());
            });
        }
    }
}
