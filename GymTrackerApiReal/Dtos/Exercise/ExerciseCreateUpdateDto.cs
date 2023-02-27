using GymTrackerApiReal.Dtos.Muscle;

namespace GymTrackerApiReal.Dtos.Exercise
{
    public class ExerciseCreateUpdateDto
    {
        public int MuscleId { get; set; }
        public string Name { get; set; }
    }
}
