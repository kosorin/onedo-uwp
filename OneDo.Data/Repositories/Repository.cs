using OneDo.Data.Entities;
using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneDo.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        //public event EventHandler<EntityEventArgs<TEntity>> Added;

        //public event EventHandler<EntityEventArgs<TEntity>> Updated;

        //public event EventHandler<EntityEventArgs<TEntity>> Deleted;

        //public event EventHandler<EntityListEventArgs<TEntity>> BulkDeleted;


        protected readonly SQLiteAsyncConnection connection;

        private bool isBulkDelete;

        internal Repository(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }


        public bool IsTransient(TEntity entity)
        {
            return entity.Id == default(Guid);
        }


        public async Task AddOrUpdate(TEntity entity)
        {
            if (IsTransient(entity))
            {
                await Add(entity);
            }
            else
            {
                await Update(entity);
            }
        }

        public virtual async Task Add(TEntity entity)
        {
            await connection.InsertAsync(entity);
            OnAdded(entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            await connection.UpdateAsync(entity);
            OnUpdated(entity);
        }

        public virtual async Task Delete(TEntity entity)
        {
            await connection.DeleteAsync<TEntity>(entity.Id);
            OnDeleted(entity);
        }

        public virtual async Task DeleteAll()
        {
            await RunBulkDelete(async () =>
            {
                var entities = await GetAll();
                foreach (var entity in entities)
                {
                    await Delete(entity);
                }
                OnBulkDeleted(entities);
            });
        }

        public virtual async Task DeleteAll(Expression<Func<TEntity, bool>> predicate)
        {
            await RunBulkDelete(async () =>
            {
                var entities = await GetAll(predicate);
                foreach (var entity in entities)
                {
                    await Delete(entity);
                }
                OnBulkDeleted(entities);
            });
        }

        protected async Task RunBulkDelete(Func<Task> action)
        {
            try
            {
                isBulkDelete = true;
                await action?.Invoke();
            }
            finally
            {
                isBulkDelete = false;
            }
        }


        public async Task<IList<TEntity>> GetAll()
        {
            return await GetTable()
                .ToListAsync();
        }

        public async Task<IList<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetTable()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<TEntity> Get(Guid id)
        {
            return await GetTable()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetTable()
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Any()
        {
            return (await GetTable()
                .CountAsync()) != 0;
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return (await GetTable()
                .Where(predicate)
                .CountAsync()) != 0;
        }


        protected void OnAdded(TEntity entity)
        {
            //Added?.Invoke(this, new EntityEventArgs<TEntity>(entity));
        }

        protected void OnUpdated(TEntity entity)
        {
        }

        protected void OnDeleted(TEntity entity)
        {
            if (!isBulkDelete)
            {
                //Deleted?.Invoke(this, new EntityEventArgs<TEntity>(entity));
            }
        }

        protected void OnBulkDeleted(IList<TEntity> entities)
        {
            //BulkDeleted?.Invoke(this, new EntityListEventArgs<TEntity>(entities));
        }


        protected AsyncTableQuery<TEntity> GetTable()
        {
            return connection.Table<TEntity>();
        }
    }
}
