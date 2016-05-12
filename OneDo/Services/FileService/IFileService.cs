using OneDo.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Services.FileService
{
    public interface IFileService
    {
        Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local);

        Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local);

        Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local);

        Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local, CreationCollisionOption option = CreationCollisionOption.ReplaceExisting);
    }
}
