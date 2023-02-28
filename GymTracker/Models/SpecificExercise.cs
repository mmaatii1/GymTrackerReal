namespace GymTracker.Models
{
    public class SpecificExercise : BaseEntity
    {
        public Exercise Exercise { get; set; }
        public double Repetitions { get; set; }
        public byte Sets { get; set; }
        public double Weight { get; set; }
    }
}
