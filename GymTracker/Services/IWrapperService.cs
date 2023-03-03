using GymTracker.Models;

namespace GymTracker.Services
{
    public interface IWrapperService<TEentity> where
        TEentity: class
    {
        Task<List<TEentity>> GetAllAsync();
        Task<TEentity> SaveAsync(TEentity item, bool isNewItem);
        Task DeleteAsync(TEentity item);

        Task<TEentity> GetByIdAsync(int id);
    }
}


