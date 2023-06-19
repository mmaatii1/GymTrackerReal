﻿// <auto-generated />
using System;
using GymTrackerApiReal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GymTrackerApiReal.Migrations
{
    [DbContext(typeof(TrackingDbContext))]
    partial class TrackingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExerciseWorkoutPlan", b =>
                {
                    b.Property<int>("ExercisesId")
                        .HasColumnType("int");

                    b.Property<int>("WorkoutPlansId")
                        .HasColumnType("int");

                    b.HasKey("ExercisesId", "WorkoutPlansId");

                    b.HasIndex("WorkoutPlansId");

                    b.ToTable("ExerciseWorkoutPlan");
                });

            modelBuilder.Entity("GymTrackerApiReal.Models.CustomWorkout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfWorkout")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WorkoutPlanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutPlanId");

                    b.ToTable("CustomWorkouts");
                });

            modelBuilder.Entity("GymTrackerApiReal.Models.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MuscleId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MuscleId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("GymTrackerApiReal.Models.Muscle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MainMasculeGroup")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Muscles");
                });

            modelBuilder.Entity("GymTrackerApiReal.Models.SpecificExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CustomWorkoutId")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<string>("Repetitions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Sets")
                        .HasColumnType("tinyint");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CustomWorkoutId");

                    b.HasIndex("ExerciseId");

                    b.ToTable("SpecificExercises");
                });

            modelBuilder.Entity("GymTrackerApiReal.Models.WorkoutPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WorkoutPlans");
                });

            modelBuilder.Entity("ExerciseWorkoutPlan", b =>
                {
                    b.HasOne("GymTrackerApiReal.Models.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExercisesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GymTrackerApiReal.Models.WorkoutPlan", null)
                        .WithMany()
                        .HasForeignKey("WorkoutPlansId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GymTrackerApiReal.Models.CustomWorkout", b =>
                {
                    b.HasOne("GymTrackerApiReal.Models.WorkoutPlan", "WorkoutPlan")
                        .WithMany("DoneWorkouts")
                        .HasForeignKey("WorkoutPlanId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("WorkoutPlan");
                });

            modelBuilder.Entity("GymTrackerApiReal.Models.Exercise", b =>
                {
                    b.HasOne("GymTrackerApiReal.Models.Muscle", "Muscle")
                        .WithMany()
                        .HasForeignKey("MuscleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Muscle");
                });

            modelBuilder.Entity("GymTrackerApiReal.Models.SpecificExercise", b =>
                {
                    b.HasOne("GymTrackerApiReal.Models.CustomWorkout", "CustomWorkout")
                        .WithMany("CustomWorkoutSpecificExercises")
                        .HasForeignKey("CustomWorkoutId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GymTrackerApiReal.Models.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomWorkout");

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("GymTrackerApiReal.Models.CustomWorkout", b =>
                {
                    b.Navigation("CustomWorkoutSpecificExercises");
                });

            modelBuilder.Entity("GymTrackerApiReal.Models.WorkoutPlan", b =>
                {
                    b.Navigation("DoneWorkouts");
                });
#pragma warning restore 612, 618
        }
    }
}
