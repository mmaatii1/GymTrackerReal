using GymTrackerApi.Models;

namespace GymTrackerApi.Dtos.Muscle
{
    public class MuscleReadDto
    {
        public int Id { get; set; }
        public MainMuscleGroup MainMasculeGroup { get; set; }
        public string Name { get; set; }
    }
}
