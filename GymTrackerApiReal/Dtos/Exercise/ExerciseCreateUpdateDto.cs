using GymTrackerApiReal.Dtos.Muscle;

namespace GymTrackerApiReal.Dtos.Exercise
{
    public class ExerciseCreateUpdateDto
    {
        public MuscleReadDto Muscle { get; set; }
        public string Name { get; set; }
    }
}
