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
        private const string FileName = "OneDo.Data.Test.db";


        public Repository<Folder> Folders { get; set; }

        public Repository<Note> Notes { get; set; }


        private SQLiteConnectionWithLock baseConnection;

        private SQLiteAsyncConnection connection;


        public async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
                if (baseConnection == null)
                {
                    var connectionString = new SQLiteConnectionString(GetPath(), true);
                    baseConnection = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), connectionString);
                    connection = new SQLiteAsyncConnection(() => baseConnection);

                    EnsureTableExists<Folder>();
                    EnsureTableExists<Note>();
                }

                Folders = new Repository<Folder>(connection);
                Notes = new Repository<Note>(connection);
            });
        }

        public Repository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            return new Repository<TEntity>(connection);
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
