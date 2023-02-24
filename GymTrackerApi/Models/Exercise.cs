namespace GymTrackerApi.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public Muscle MuscleGroup { get; set; } 
        public string Name { get; set; }    
    }
}
