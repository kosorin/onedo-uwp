using Newtonsoft.Json;
using System;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Common.Data
{
    public static class Serialization
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
#if DEBUG
            Formatting = Formatting.Indented
#else
                Formatting = Formatting.None
#endif
        };


        public static async Task SerializeToFileAsync<T>(T value, string fileName, StorageFolder folder)
        {
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            var serializedValue = Serialize(value);
            await FileIO.WriteTextAsync(file, serializedValue);
        }

        public static async Task<T> DeserializeFromFileAsync<T>(string fileName, StorageFolder folder)
        {
            var file = await folder.GetFileAsync(fileName);
            var serializedValue = await FileIO.ReadTextAsync(file);
            var value = Deserialize<T>(serializedValue);
            return value;
        }


        public static string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item, settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
