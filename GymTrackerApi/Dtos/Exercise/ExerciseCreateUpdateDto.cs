using GymTrackerApi.Dtos.Muscle;

namespace GymTrackerApi.Dtos.Exercise
{
    public class ExerciseCreateUpdateDto
    {
        public MuscleReadDto Muscle { get; set; }
        public string Name { get; set; }
    }
}
