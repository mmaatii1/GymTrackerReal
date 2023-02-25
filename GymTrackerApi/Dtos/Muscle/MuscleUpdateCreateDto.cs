using GymTrackerApi.Models;

namespace GymTrackerApi.Dtos.Muscle
{
    public class MuscleUpdateCreateDto
    {
        public MainMuscleGroup MainMasculeGroup { get; set; }
        public string Name { get; set; }
    }
}
