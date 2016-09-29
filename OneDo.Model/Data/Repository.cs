using SQLite.Net;
using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await connection.DeleteAsync<TEntity>(entity);
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

        public async Task<List<TEntity>> GetAll(Predicate<TEntity> predicate)
        {
            return await connection
                .Table<TEntity>()
                .Where(x => predicate(x))
                .ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await connection
                .Table<TEntity>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<TEntity> Get(Predicate<TEntity> predicate)
        {
            return await connection
                .Table<TEntity>()
                .Where(x => predicate(x))
                .FirstOrDefaultAsync();
        }
    }
}
