using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Common.IO
{
    public class FileHelper
    {
        public enum StorageStrategies { Local, Roaming, Temporary }


        public static async Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local)
        {
            return (await GetIfFileExistsAsync(key, location)) != null;
        }

        public static async Task<bool> FileExistsAsync(string key, StorageFolder folder)
        {
            return (await GetIfFileExistsAsync(key, folder)) != null;
        }

        public static async Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local)
        {
            var file = await GetIfFileExistsAsync(key, location);
            if (file != null)
            {
                await file.DeleteAsync();
            }
            return !(await FileExistsAsync(key, location));
        }

        public static async Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local)
        {
            try
            {
                var file = await GetIfFileExistsAsync(key, location);
                if (file == null)
                {
                    return default(T);
                }
                var value = await FileIO.ReadTextAsync(file);
                var result = Deserialize<T>(value);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local, CreationCollisionOption option = CreationCollisionOption.ReplaceExisting)
        {
            var file = await CreateFileAsync(key, location, option);
            var serializedValue = Serialize(value);
            await FileIO.WriteTextAsync(file, serializedValue);
            return await FileExistsAsync(key, location);
        }


        private static async Task<StorageFile> CreateFileAsync(string key, StorageStrategies location = StorageStrategies.Local, CreationCollisionOption option = CreationCollisionOption.OpenIfExists)
        {
            switch (location)
            {
            case StorageStrategies.Local:
                return await ApplicationData.Current.LocalFolder.CreateFileAsync(key, option);
            case StorageStrategies.Roaming:
                return await ApplicationData.Current.RoamingFolder.CreateFileAsync(key, option);
            case StorageStrategies.Temporary:
                return await ApplicationData.Current.TemporaryFolder.CreateFileAsync(key, option);
            default:
                throw new NotSupportedException(location.ToString());
            }
        }

        private static async Task<StorageFile> GetIfFileExistsAsync(string key, StorageFolder folder)
        {
            StorageFile retval;
            try
            {
                retval = await folder.GetFileAsync(key);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine($"{nameof(GetIfFileExistsAsync)}:{nameof(FileNotFoundException)}");
                return null;
            }
            return retval;
        }

        private static async Task<StorageFile> GetIfFileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local)
        {
            try
            {
                switch (location)
                {
                case StorageStrategies.Local:
                    return await ApplicationData.Current.LocalFolder.GetFileAsync(key);
                case StorageStrategies.Roaming:
                    return await ApplicationData.Current.RoamingFolder.GetFileAsync(key);
                case StorageStrategies.Temporary:
                    return await ApplicationData.Current.TemporaryFolder.GetFileAsync(key);
                default:
                    throw new NotSupportedException(location.ToString());
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine($"{nameof(GetIfFileExistsAsync)}:{nameof(FileNotFoundException)}");
                return null;
            }
        }


        private static string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            });
        }

        private static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
