namespace GymTracker.Models
{
    public class Muscle : BaseEntity
    {
        public MainMuscleGroup MainMasculeGroup { get; set; }
        public string Name { get; set; }
    }
}
