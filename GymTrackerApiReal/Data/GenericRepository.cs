using GymTrackerApiReal.Exceptions;
using GymTrackerApiReal.Interfaces;
using GymTrackerApiReal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymTrackerApiReal.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly TrackingDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(TrackingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> DeleteAsync(int id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);

            if (entityToDelete is null)
            {
                var getEntityType = _dbSet.EntityType;
                var entityName = getEntityType.Name.ToString().Split('.').Last();
                throw new EntityNotFoundException(entityName);
            }
           
            _dbSet.Remove(entityToDelete);
            await _context.SaveChangesAsync();
            return entityToDelete;
        }

        public IEnumerable<TEntity> GetWithEntity<TProperty>(Expression<Func<TEntity, TProperty>> includeEntityOne)
        {
            return _dbSet.Include(includeEntityOne);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is null)
            {
                var getEntityType = _dbSet.EntityType;
                var entityName = getEntityType.Name.ToString().Split('.').Last();
                throw new EntityNotFoundException(entityName);
            }

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var updatedEntity = _dbSet.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("The database operation was expected to affect 1 row(s), but actually affected 0 row(s);")
                    || exception.Message.Contains("Attempted to update or delete an entity that does not exist in the store"))
                {
                    var entityName = updatedEntity.GetType().ToString().Split('.').Last();
                    throw new EntityNotFoundException(entityName);
                }
                else
                {
                    throw new Exception(exception.Message);
                }
            }
        }

        public IQueryable<TEntity> GetAsQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
    }
