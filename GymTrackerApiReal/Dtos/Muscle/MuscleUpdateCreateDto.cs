using GymTrackerApiReal.Models;

namespace GymTrackerApiReal.Dtos.Muscle
{
    public class MuscleUpdateCreateDto
    {
        public MainMuscleGroup MainMasculeGroup { get; set; }
        public string Name { get; set; }
    }
}
