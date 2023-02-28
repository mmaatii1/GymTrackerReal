using GymTracker.Models;

namespace GymTracker.Services
{
    public interface IWrapperService<TEentity> where
        TEentity: class
    {
        Task<List<TEentity>> GetWorkoutAsync();
        Task SaveWorkoutAsync(TEentity item, bool isNewItem);
        Task DeleteWorkoutAsync(TEentity item);

        Task<TEentity> GetWorkoutByIdAsync(int id);
    }
}


