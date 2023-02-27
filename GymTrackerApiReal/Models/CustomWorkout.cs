namespace GymTrackerApiReal.Models
{
    public class CustomWorkout
    {
        public int Id { get; set; }
        public ICollection<SpecificExercise> CustomWorkoutSpecificExercises { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
    }
}
