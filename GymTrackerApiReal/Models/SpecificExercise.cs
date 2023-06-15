
namespace GymTrackerApiReal.Models
{
    public class SpecificExercise
    {
        public int Id
        {
            get; set;
        }
        public Exercise Exercise { get; set; }
        public double[] Repetitions { get; set; }
        public byte Sets { get; set; }
        public double Weight { get; set; }
        public CustomWorkout CustomWorkout { get; set; }
        public int? CustomWorkoutId { get; set; }
    }
}
