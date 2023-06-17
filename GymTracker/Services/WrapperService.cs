using GymTracker.Models;

namespace GymTracker.Services
{
    public class WrapperService
        <TEntity> : IWrapperService<TEntity> where TEntity : BaseEntity
    {
        IRestService<TEntity> _restService;

        public WrapperService(IRestService<TEntity> service)
        {
            _restService = service;
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _restService.GetAllAsync();
        }

        public Task<TEntity> SaveAsync(TEntity item, bool isNewItem = false, bool isPhoto = false)
        {
            return _restService.SaveAsync(item, isNewItem, isPhoto);
        }

        public Task DeleteAsync(TEntity item)
        {
            return _restService.DeleteAsync(item.Id);
        }

        public Task<TEntity> GetByIdAsync(string id)
        {
            return _restService.GetByIdAsync(id);
        }
    }
}

