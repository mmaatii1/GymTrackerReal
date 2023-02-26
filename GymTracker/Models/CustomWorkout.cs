namespace GymTracker.Models
{
    public class CustomWorkout
    {
        public int Id { get; set; }
        public List<SpecificExercise> Exercises { get; set; }
        public DateTime DateOfWorkout { get; set; }
        public string Name { get; set; }
    }
}
