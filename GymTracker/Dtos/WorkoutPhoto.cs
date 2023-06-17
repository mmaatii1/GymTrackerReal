
namespace GymTracker.Dtos
{
    [Serializable]
    [ApiEndpoint("WorkoutPhoto")]
    public class WorkoutPhoto : BaseEntity
    {
        public byte[] PhotoAsBytes { get; set; }
        public Guid Guid { get; set; }
    }
}
