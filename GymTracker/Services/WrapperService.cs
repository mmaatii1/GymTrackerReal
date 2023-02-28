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

        public Task SaveAsync(TEntity item, bool isNewItem = false)
        {
            return _restService.SaveAsync(item, isNewItem);
        }

        public Task DeleteAsync(TEntity item)
        {
            return _restService.DeleteAsync(item.Id.ToString());
        }

        public Task<TEntity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

