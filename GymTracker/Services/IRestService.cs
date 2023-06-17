using GymTracker.Models;

namespace GymTracker.Services
{
    public interface IRestService<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> SaveAsync(TEntity item, bool isNewItem, bool isPhoto);

        Task DeleteAsync(int id);

        Task<TEntity> GetByIdAsync(string id);
    }
}