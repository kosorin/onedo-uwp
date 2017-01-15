using OneDo.Data.Entities;
using OneDo.Domain.Common;
using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneDo.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly SQLiteAsyncConnection connection;

        internal Repository(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        private AsyncTableQuery<TEntity> GetTable()
        {
            return connection.Table<TEntity>();
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
            var entity = await GetTable()
                .FirstOrDefaultAsync();
            return entity != null;
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await GetTable()
                .Where(predicate)
                .FirstOrDefaultAsync();
            return entity != null;
        }

        public async Task<int> Count()
        {
            return await GetTable()
                .CountAsync();
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetTable()
                .Where(predicate)
                .CountAsync();
        }


        public async Task Add(TEntity entity)
        {
            await connection.InsertAsync(entity);
        }

        public async Task Update(TEntity entity)
        {
            await connection.UpdateAsync(entity);
        }

        public async Task Delete(Guid id)
        {
            await connection.DeleteAsync<TEntity>(id);
        }

        public async Task DeleteAll()
        {
            var entities = await GetTable().ToListAsync();
            foreach (var entity in entities)
            {
                await Delete(entity.Id);
            }
        }

        public async Task DeleteAll(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await GetTable().Where(predicate).ToListAsync();
            foreach (var entity in entities)
            {
                await Delete(entity.Id);
            }
        }
    }
}