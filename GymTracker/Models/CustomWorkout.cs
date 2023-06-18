namespace GymTracker.Models
{
    public class CustomWorkout : BaseEntity
    {
        public List<SpecificExercise> CustomWorkoutSpecificExercises { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
        public Guid? Guid { get; set; }
        public string Location { get; set; }
        public byte[]? PhotoAsBytes { get; set; }
    }
}
