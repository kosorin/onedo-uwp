using OneDo.Model.Data.Entities;
using SQLite.Net;
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
using SQLite.Net.Async;

namespace OneDo.Model.Data
{
    public partial class DataService : IDisposable
    {
        private const string FileName = "Data.db";


        public Repository<Folder> Folders { get; private set; }

        public Repository<Note> Notes { get; private set; }


        private SQLiteConnectionWithLock baseConnection;

        private SQLiteAsyncConnection connection;

        private Dictionary<Type, IRepository> repositories = new Dictionary<Type, IRepository>();

        public Task InitializeDataAsync()
        {
            if (baseConnection == null)
            {
                var connectionString = new SQLiteConnectionString(GetPath(), true);
                baseConnection = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), connectionString);
                connection = new SQLiteAsyncConnection(() => baseConnection);

                EnsureTableExists<Folder>();
                EnsureTableExists<Note>();

                Folders = GetRepository<Folder>();
                Notes = GetRepository<Note>();
            }
            return Task.CompletedTask;
        }

        public Repository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            var type = typeof(TEntity);
            if (repositories.ContainsKey(type))
            {
                return (Repository<TEntity>)repositories[type];
            }
            else
            {
                var repository = new Repository<TEntity>(connection);
                repositories[type] = repository;
                return repository;
            }
        }


        private void EnsureTableExists<TEntity>() where TEntity : IEntity
        {
            var tableName = GetTableName<TEntity>();
            var columnInfos = baseConnection.GetTableInfo(tableName);
            if (!columnInfos.Any())
            {
                baseConnection.CreateTable<TEntity>();
            }
        }

        private string GetTableName<TEntity>() where TEntity : IEntity
        {
            string tableName = nameof(TEntity);
            var tableAttribute = typeof(TEntity).GetTypeInfo().GetCustomAttribute<TableAttribute>(false);
            if (tableAttribute != null)
            {
                tableName = tableAttribute.Name;
            }
            return tableName;
        }

        private string GetPath()
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, FileName);
        }


        private void DisposeData()
        {
            baseConnection?.Close();
            baseConnection?.Dispose();
            baseConnection = null;
        }
    }
}
