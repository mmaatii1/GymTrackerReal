using GymTracker.Models;

namespace GymTracker.Services
{
    public interface IWorkoutService
    {
        Task<List<CustomWorkout>> GetWorkoutAsync();
        Task SaveWorkoutAsync(CustomWorkout item, bool isNewItem);
        Task DeleteWorkoutAsync(CustomWorkout item);

        Task<CustomWorkout> GetWorkoutByIdAsync(int id);
    }
}
