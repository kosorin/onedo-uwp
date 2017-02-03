using OneDo.Infrastructure.Data.Entities;
using OneDo.Domain.Common;
using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OneDo.Infrastructure.Data.Repositories
{
    public class Repository<TEntityData> : IRepository<TEntityData> where TEntityData : class, IEntityData
    {
        private readonly SQLiteAsyncConnection connection;

        internal Repository(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        private AsyncTableQuery<TEntityData> GetTable()
        {
            return connection.Table<TEntityData>();
        }


        public async Task<IList<TEntityData>> GetAll()
        {
            return await GetTable()
                .ToListAsync();
        }

        public async Task<IList<TEntityData>> GetAll(Expression<Func<TEntityData, bool>> predicate)
        {
            return await GetTable()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<TEntityData> Get(Guid id)
        {
            return await GetTable()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<TEntityData> Get(Expression<Func<TEntityData, bool>> predicate)
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

        public async Task<bool> Any(Expression<Func<TEntityData, bool>> predicate)
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

        public async Task<int> Count(Expression<Func<TEntityData, bool>> predicate)
        {
            return await GetTable()
                .Where(predicate)
                .CountAsync();
        }


        public async Task Add(TEntityData entityData)
        {
            await connection.InsertAsync(entityData);
        }

        public async Task Update(TEntityData entityData)
        {
            await connection.UpdateAsync(entityData);
        }

        public async Task Delete(Guid id)
        {
            await connection.DeleteAsync<TEntityData>(id);
        }

        public async Task DeleteAll()
        {
            var entityDatas = await GetTable().ToListAsync();
            foreach (var entityData in entityDatas)
            {
                await Delete(entityData.Id);
            }
        }

        public async Task DeleteAll(Expression<Func<TEntityData, bool>> predicate)
        {
            var entityDatas = await GetTable().Where(predicate).ToListAsync();
            foreach (var entityData in entityDatas)
            {
                await Delete(entityData.Id);
            }
        }
    }
}