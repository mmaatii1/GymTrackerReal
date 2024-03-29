﻿using GymTrackerApiReal.Dtos.SpecificExercise;

namespace GymTrackerApiReal.Dtos.CustomWorkout
{
    public class CustomWorkoutCreateUpdateDto
    {
        public int Id { get; set; }
        public IEnumerable<int> SpecificExercisesIds { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Guid? Guid { get; set; }
    }
}
