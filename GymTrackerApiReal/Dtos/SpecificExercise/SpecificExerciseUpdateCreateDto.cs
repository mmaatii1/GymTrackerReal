using GymTrackerApiReal.Dtos.Exercise;

namespace GymTrackerApiReal.Dtos.SpecificExercise
{
    public class SpecificExerciseUpdateCreateDto
    {
        public ExerciseReadDto Exercise { get; set; }
        public double Repetitions { get; set; }
        public byte Sets { get; set; }
        public double Weight { get; set; }
    }
}
