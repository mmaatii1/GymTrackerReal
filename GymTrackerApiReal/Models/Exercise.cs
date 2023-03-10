namespace GymTrackerApiReal.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public Muscle Muscle { get; set; }
        public string Name { get; set; }
        public ICollection<WorkoutPlan> WorkoutPlans { get; set; }
    }
}
