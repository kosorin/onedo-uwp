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
    public class Repository<TEntity> : IRepository where TEntity : class, IEntity
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

        public AsyncTableQuery<TEntity> GetTable()
        {
            return connection.Table<TEntity>();
        }
    }
}
