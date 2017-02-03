using OneDo.Infrastructure.Data.Entities;
using OneDo.Infrastructure.Data.Repositories;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Attributes;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Infrastructure.Data
{
    public class DataService : IDataService
    {
        public string FileName => "Data.db";

        private SQLiteConnectionWithLock baseConnection;

        private SQLiteAsyncConnection connection;

        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public DataService()
        {
            var sqlitePlatform = new SQLitePlatformWinRT();
            var connectionString = Path.Combine(ApplicationData.Current.LocalFolder.Path, FileName);
            var sqliteConnectionString = new SQLiteConnectionString(connectionString, false);
            baseConnection = new SQLiteConnectionWithLock(sqlitePlatform, sqliteConnectionString);
            connection = new SQLiteAsyncConnection(() => baseConnection);

            EnsureTableExists<FolderData>();
            EnsureTableExists<NoteData>();
        }

        private void EnsureTableExists<TEntityData>() where TEntityData : class, IEntityData
        {
            var tableName = GetTableName<TEntityData>();
            var columnInfos = baseConnection.GetTableInfo(tableName);
            if (!columnInfos.Any())
            {
                baseConnection.CreateTable<TEntityData>();
            }
            else
            {
                baseConnection.MigrateTable<TEntityData>();
            }
        }

        private string GetTableName<TEntityData>() where TEntityData : IEntityData
        {
            var tableName = nameof(TEntityData);
            var tableAttribute = typeof(TEntityData).GetTypeInfo().GetCustomAttribute<TableAttribute>(false);
            if (tableAttribute != null)
            {
                tableName = tableAttribute.Name;
            }
            return tableName;
        }


        public IRepository<TEntityData> GetRepository<TEntityData>() where TEntityData : class, IEntityData
        {
            return GetCachedRepository<TEntityData>();
        }

        public IQueryRepository<TEntityData> GetQueryRepository<TEntityData>() where TEntityData : class, IEntityData
        {
            return GetCachedRepository<TEntityData>();
        }

        public ICommandRepository<TEntityData> GetCommandRepository<TEntityData>() where TEntityData : class, IEntityData
        {
            return GetCachedRepository<TEntityData>();
        }

        private Repository<TEntityData> GetCachedRepository<TEntityData>() where TEntityData : class, IEntityData
        {
            var type = typeof(TEntityData);
            if (repositories.ContainsKey(type))
            {
                return (Repository<TEntityData>)repositories[type];
            }
            else
            {
                var repository = CreateRepository<TEntityData>();
                repositories[type] = repository;
                return repository;
            }
        }

        private Repository<TEntityData> CreateRepository<TEntityData>() where TEntityData : class, IEntityData
        {
            return new Repository<TEntityData>(connection);
        }


        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    baseConnection?.Close();
                    baseConnection?.Dispose();
                }
                disposed = true;
            }
        }
    }
}
