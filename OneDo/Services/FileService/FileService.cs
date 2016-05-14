using OneDo.Common.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Services.FileService
{
    public class FileService : IFileService
    {
        public async Task<bool> FileExistsAsync(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local)
            => await FileHelper.FileExistsAsync(key, locality);

        public async Task<bool> DeleteFileAsync(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local)
            => await FileHelper.DeleteFileAsync(key, locality);

        public async Task<T> ReadFileAsync<T>(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local)
            => await FileHelper.ReadFileAsync<T>(key, locality);

        public async Task<bool> WriteFileAsync<T>(string key, T value, ApplicationDataLocality locality = ApplicationDataLocality.Local, CreationCollisionOption option = CreationCollisionOption.ReplaceExisting)
            => await FileHelper.WriteFileAsync(key, value, locality, option);
    }
}