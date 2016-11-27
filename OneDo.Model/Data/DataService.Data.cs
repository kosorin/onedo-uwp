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
using OneDo.Model.Business;
using OneDo.Model.Data.Repositories;

namespace OneDo.Model.Data
{
    public partial class DataService : IDisposable
    {
        private const string FileName = "Data.db";


        public IRepository<Folder> Folders { get; private set; }

        public IRepository<Note> Notes { get; private set; }


        private SQLiteConnectionWithLock baseConnection;

        private SQLiteAsyncConnection connection;

        private Dictionary<Type, IRepository> repositories = new Dictionary<Type, IRepository>();

        public async Task InitializeDataAsync()
        {
            if (baseConnection == null)
            {
                var connectionString = new SQLiteConnectionString(GetPath(), true);
                baseConnection = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), connectionString);
                connection = new SQLiteAsyncConnection(() => baseConnection);

                await EnsureTableExists<Folder>();
                await EnsureTableExists<Note>();

                Folders = GetRepository<Folder>();
                Notes = GetRepository<Note>();
            }
        }


        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            var type = typeof(TEntity);
            if (repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>)repositories[type];
            }
            else
            {
                var repository = CreateRepository<TEntity>();
                repositories[type] = repository;
                return repository;
            }
        }

        private IRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class, IEntity
        {
            if (typeof(TEntity) == typeof(Folder))
            {
                return (IRepository<TEntity>)new FolderRepository(connection, GetRepository<Note>());
            }
            return new Repository<TEntity>(connection);
        }


        private async Task EnsureTableExists<TEntity>() where TEntity : class, IEntity
        {
            var tableName = GetTableName<TEntity>();
            var columnInfos = baseConnection.GetTableInfo(tableName);
            if (!columnInfos.Any())
            {
                await connection.CreateTableAsync<TEntity>();
            }
            else
            {
                baseConnection.MigrateTable<TEntity>();
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
