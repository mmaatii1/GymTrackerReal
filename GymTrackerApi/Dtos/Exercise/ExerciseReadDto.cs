using GymTrackerApi.Dtos.Muscle;

namespace GymTrackerApi.Dtos.Exercise
{
    public class ExerciseReadDto
    {
        public int Id { get; set; }
        public MuscleReadDto Muscle { get; set; }
        public string Name { get; set; }
    }
}
