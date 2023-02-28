namespace GymTracker.Models
{
    public class CustomWorkout : BaseEntity
    {
        public List<SpecificExercise> CustomWorkoutSpecificExercises { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
    }
}
