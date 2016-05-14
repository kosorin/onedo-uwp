using Newtonsoft.Json;
using OneDo.Common.Logging;
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
        public static async Task<bool> FileExistsAsync(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local)
        {
            return (await GetIfFileExistsAsync(key, locality)) != null;
        }

        public static async Task<bool> FileExistsAsync(string key, StorageFolder folder)
        {
            return (await GetIfFileExistsAsync(key, folder)) != null;
        }

        public static async Task<bool> DeleteFileAsync(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local)
        {
            var file = await GetIfFileExistsAsync(key, locality);
            if (file != null)
            {
                await file.DeleteAsync();
            }
            return !(await FileExistsAsync(key, locality));
        }

        public static async Task<T> ReadFileAsync<T>(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local)
        {
            try
            {
                var file = await GetIfFileExistsAsync(key, locality);
                if (file == null)
                {
                    return default(T);
                }
                var serializedValue = await FileIO.ReadTextAsync(file);
                var value = Deserialize<T>(serializedValue);
                return value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<bool> WriteFileAsync<T>(string key, T value, ApplicationDataLocality locality = ApplicationDataLocality.Local, CreationCollisionOption option = CreationCollisionOption.ReplaceExisting)
        {
            var file = await CreateFileAsync(key, locality, option);
            var serializedValue = Serialize(value);
            await FileIO.WriteTextAsync(file, serializedValue);
            return await FileExistsAsync(key, locality);
        }


        private static async Task<StorageFile> CreateFileAsync(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local, CreationCollisionOption option = CreationCollisionOption.OpenIfExists)
        {
            switch (locality)
            {
            case ApplicationDataLocality.Local:
                return await ApplicationData.Current.LocalFolder.CreateFileAsync(key, option);
            case ApplicationDataLocality.Roaming:
                return await ApplicationData.Current.RoamingFolder.CreateFileAsync(key, option);
            case ApplicationDataLocality.Temporary:
                return await ApplicationData.Current.TemporaryFolder.CreateFileAsync(key, option);
            default:
                throw new NotSupportedException($"Lokalita {locality} není podporována.");
            }
        }

        private static async Task<StorageFile> GetIfFileExistsAsync(string key, StorageFolder folder)
        {
            StorageFile retval;
            try
            {
                retval = await folder.GetFileAsync(key);
            }
            catch (FileNotFoundException e)
            {
                Logger.Current.Warn(e);
                return null;
            }
            return retval;
        }

        private static async Task<StorageFile> GetIfFileExistsAsync(string key, ApplicationDataLocality locality = ApplicationDataLocality.Local)
        {
            try
            {
                switch (locality)
                {
                case ApplicationDataLocality.Local:
                    return await ApplicationData.Current.LocalFolder.GetFileAsync(key);
                case ApplicationDataLocality.Roaming:
                    return await ApplicationData.Current.RoamingFolder.GetFileAsync(key);
                case ApplicationDataLocality.Temporary:
                    return await ApplicationData.Current.TemporaryFolder.GetFileAsync(key);
                default:
                    throw new NotSupportedException($"Lokalita {locality} není podporována.");
                }
            }
            catch (FileNotFoundException e)
            {
                Logger.Current.Warn(e);
                return null;
            }
        }


        private static string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
#if DEBUG
                Formatting = Formatting.Indented
#else
                Formatting = Formatting.None
#endif
            });
        }

        private static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
