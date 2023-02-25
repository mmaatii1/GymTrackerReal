using GymTrackerApiReal.Models;

namespace GymTrackerApiReal.Dtos.Muscle
{
    public class MuscleReadDto
    {
        public int Id { get; set; }
        public MainMuscleGroup MainMasculeGroup { get; set; }
        public string Name { get; set; }
    }
}
