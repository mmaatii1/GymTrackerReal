
namespace GymTracker.Dtos
{
    public class SpecificExerciseUpdateCreateDto : BaseEntity
    {
        public int ExerciseId { get; set; }
        public double[] Repetitions { get; set; }
        public byte Sets { get; set; }
        public double Weight { get; set; }
    }
}
