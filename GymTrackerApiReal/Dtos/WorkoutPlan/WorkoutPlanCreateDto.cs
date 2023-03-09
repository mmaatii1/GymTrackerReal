namespace GymTrackerApiReal.Dtos.WorkoutPlan
{
    public class WorkoutPlanCreateDto
    {
        public IEnumerable<int> ExercisesIds { get; set; }
        public string Name { get; set; }
    }
}
