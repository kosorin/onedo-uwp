using OneDo.Data.Entities;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Attributes;
using SQLite.Net.Platform.WinRT;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Data.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly SQLiteAsyncConnection connection;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public RepositoryFactory(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public IQueryRepository<TEntity> GetQueryRepository<TEntity>() where TEntity : class, IEntity
        {
            return GetRepository<TEntity>();
        }

        public ICommandRepository<TEntity> GetCommandRepository<TEntity>() where TEntity : class, IEntity
        {
            return GetRepository<TEntity>();
        }

        private Repository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            var type = typeof(TEntity);
            if (repositories.ContainsKey(type))
            {
                return (Repository<TEntity>)repositories[type];
            }
            else
            {
                var repository = CreateRepository<TEntity>();
                repositories[type] = repository;
                return repository;
            }
        }

        private Repository<TEntity> CreateRepository<TEntity>() where TEntity : class, IEntity
        {
            return new Repository<TEntity>(connection);
        }
    }
}
