namespace GymTrackerApiReal.Models
{
    public class WorkoutPlan
    {
        public int Id { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
        public ICollection<CustomWorkout>? DoneWorkouts { get; set; }
        public string Name { get; set; }
    }
}
