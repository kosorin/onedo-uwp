using OneDo.Infrastructure.Entities;
using OneDo.Infrastructure.Repositories;
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

namespace OneDo.Infrastructure.Services.DataService
{
    public class DataService : IDataService
    {
        public string FileName => "Data.db";

        public IRepositoryFactory RepositoryFactory { get; private set; }

        private SQLiteConnectionWithLock baseConnection;

        private SQLiteAsyncConnection connection;

        public DataService()
        {
            var sqlitePlatform = new SQLitePlatformWinRT();
            var connectionString = Path.Combine(ApplicationData.Current.LocalFolder.Path, FileName);
            var sqliteConnectionString = new SQLiteConnectionString(connectionString, false);
            baseConnection = new SQLiteConnectionWithLock(sqlitePlatform, sqliteConnectionString);
            connection = new SQLiteAsyncConnection(() => baseConnection);

            EnsureTableExists<FolderData>();
            EnsureTableExists<NoteData>();

            RepositoryFactory = new RepositoryFactory(connection);
        }


        private void EnsureTableExists<TEntity>() where TEntity : class, IEntity
        {
            var tableName = GetTableName<TEntity>();
            var columnInfos = baseConnection.GetTableInfo(tableName);
            if (!columnInfos.Any())
            {
                baseConnection.CreateTable<TEntity>();
            }
            else
            {
                baseConnection.MigrateTable<TEntity>();
            }
        }

        private string GetTableName<TEntity>() where TEntity : IEntity
        {
            var tableName = nameof(TEntity);
            var tableAttribute = typeof(TEntity).GetTypeInfo().GetCustomAttribute<TableAttribute>(false);
            if (tableAttribute != null)
            {
                tableName = tableAttribute.Name;
            }
            return tableName;
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
