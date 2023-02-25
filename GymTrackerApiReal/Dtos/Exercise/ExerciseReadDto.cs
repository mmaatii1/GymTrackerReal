using GymTrackerApiReal.Dtos.Muscle;

namespace GymTrackerApiReal.Dtos.Exercise
{
    public class ExerciseReadDto
    {
        public int Id { get; set; }
        public MuscleReadDto Muscle { get; set; }
        public string Name { get; set; }
    }
}
