using OneDo.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Services.FileService
{
    public class FileService : IFileService
    {
        public async Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local)
            => await FileHelper.FileExistsAsync(key, location);

        public async Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local)
            => await FileHelper.DeleteFileAsync(key, location);

        public async Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local)
            => await FileHelper.ReadFileAsync<T>(key, location);

        public async Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local, CreationCollisionOption option = CreationCollisionOption.ReplaceExisting)
            => await FileHelper.WriteFileAsync(key, value, location, option);
    }
}