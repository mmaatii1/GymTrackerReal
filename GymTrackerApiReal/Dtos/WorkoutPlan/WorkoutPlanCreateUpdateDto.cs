﻿namespace GymTrackerApiReal.Dtos.WorkoutPlan
{
    public class WorkoutPlanCreateUpdateDto
    {
        public int Id { get; set; }
        public IEnumerable<int> ExercisesIds { get; set; }
        public IEnumerable<int>? DoneWorkoutsIds { get; set; }
        public string Name { get; set; }
    }
}
