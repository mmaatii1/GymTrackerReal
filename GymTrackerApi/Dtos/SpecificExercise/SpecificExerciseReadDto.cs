using GymTrackerApi.Dtos.Exercise;

namespace GymTrackerApi.Dtos.SpecificExercise
{
    public class SpecificExerciseReadDto
    {
        public int Id
        {
            get; set;
        }
        public ExerciseReadDto Exercise { get; set; }
        public double Repetitions { get; set; }
        public byte Sets { get; set; }
        public double Weight { get; set; }
    }
}
