namespace GymTracker.Models
{
    public class Exercise : BaseEntity
    {
        public Muscle Muscle { get; set; }
        public string Name { get; set; }
    }
}
