using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;
using System.Linq.Expressions;

namespace OneDo.Model.Business
{
    public abstract class EntityBusiness<TEntity> : DataBusinessBase where TEntity : class, IEntity, new()
    {
        private readonly Repository<TEntity> repository;

        public event EventHandler<EntityEventArgs<TEntity>> Saved;

        public event EventHandler<EntityEventArgs<TEntity>> Deleted;

        public EntityBusiness(DataService dataService) : base(dataService)
        {
            repository = dataService.GetRepository<TEntity>();
        }

        public bool IsNew(TEntity entity)
        {
            return entity.Id == default(int);
        }

        public virtual TEntity CreateDefault()
        {
            return new TEntity();
        }

        public async Task Save(TEntity entity)
        {
            if (IsNew(entity))
            {
                await repository.Add(entity);
            }
            else
            {
                await repository.Update(entity);
            }
        }

        public async Task SaveAsNew(TEntity entity)
        {
            entity.Id = default(int);
            await repository.Add(entity);
            OnSaved(entity);
        }

        public async Task Delete(TEntity entity)
        {
            await repository.Delete(entity);
            OnDeleted(entity);
        }

        public async Task DeleteAll()
        {
            await repository.DeleteAll();
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await repository
                .GetTable()
                .ToListAsync();
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return await repository
                .GetTable()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<TEntity> Get(int id)
        {
            return await repository
                .GetTable()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await repository
                .GetTable()
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Any()
        {
            return (await repository
                .GetTable()
                .CountAsync()) != 0;
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return (await repository
                .GetTable()
                .Where(predicate)
                .CountAsync()) != 0;
        }

        private void OnSaved(TEntity entity)
        {
            Saved?.Invoke(this, new EntityEventArgs<TEntity>(entity));
        }

        private void OnDeleted(TEntity entity)
        {
            Deleted?.Invoke(this, new EntityEventArgs<TEntity>(entity));
        }
    }
}
