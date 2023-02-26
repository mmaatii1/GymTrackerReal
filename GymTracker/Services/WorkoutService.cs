
using GymTracker.Models;

namespace GymTracker.Services
{
    public class WorkoutService : IWorkoutService
    {
        IRestService<CustomWorkout> _restService;

        public WorkoutService(IRestService<CustomWorkout> service)
        {
            _restService = service;
        }

        public Task<List<CustomWorkout>> GetWorkoutAsync()
        {
            return _restService.GetAllAsync();
        }

        public Task SaveWorkoutAsync(CustomWorkout item, bool isNewItem = false)
        {
            return _restService.SaveAsync(item, isNewItem);
        }

        public Task DeleteWorkoutAsync(CustomWorkout item)
        {
            return _restService.DeleteAsync(item.Id.ToString());
        }

        public Task<CustomWorkout> GetWorkoutByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
