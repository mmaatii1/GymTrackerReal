﻿using GymTrackerApi.Dtos.SpecificExercise;

namespace GymTrackerApi.Dtos.CustomWorkout
{
    public class CustomWorkoutReadDto
    {
        public int Id { get; set; }
        public List<SpecificExerciseReadDto> Exercises { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
    }
}
