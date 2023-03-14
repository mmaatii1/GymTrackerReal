using GymTrackerApiReal.Dtos.Exercise;

namespace GymTrackerApiReal.Dtos.WorkoutPlan
{
    public class WorkoutPlanReadDto
    {
        public int Id { get; set; }
        public ICollection<ExerciseReadDto> Exercises { get; set; }
        public List<int> DoneWorkoutsIds { get; set; }
        public string Name { get; set; }
    }
}
