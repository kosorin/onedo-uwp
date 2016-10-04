using SQLite.Net;
using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Model.Data
{
    public class Repository<TEntity> where TEntity : class, IEntity
    {
        protected readonly SQLiteAsyncConnection connection;

        public Repository(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task Add(TEntity entity)
        {
            await connection.InsertAsync(entity);
        }

        public async Task Update(TEntity entity)
        {
            await connection.UpdateAsync(entity);
        }

        public async Task Delete(TEntity entity)
        {
            await connection.DeleteAsync<TEntity>(entity.Id);
        }

        public async Task DeleteAll()
        {
            await connection.DeleteAllAsync<TEntity>();
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await connection
                .Table<TEntity>()
                .ToListAsync();
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return await connection
                .Table<TEntity>()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<TEntity> Get(int id)
        {
            return await connection
                .Table<TEntity>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await connection
                .Table<TEntity>()
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Any()
        {
            return (await connection
                .Table<TEntity>()
                .CountAsync()) != 0;
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return (await connection
                .Table<TEntity>()
                .Where(predicate)
                .CountAsync()) != 0;
        }
    }
}
