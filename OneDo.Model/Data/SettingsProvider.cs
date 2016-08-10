using OneDo.Common.Data;
using OneDo.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OneDo.Model.Data
{
    public class SettingsProvider : ISettingsProvider
    {
        private const string FileName = "OneDo.Settings.json";


        public Settings Current { get; private set; } = new Settings();

        public async Task LoadAsync()
        {
            var defaultSettings = new Settings();
            try
            {
                Current = await Serialization.DeserializeFromFileAsync<Settings>(FileName, ApplicationData.Current.LocalFolder) ?? defaultSettings;
                Logger.Current.Info($"Settings loaded from file '{FileName}'");
            }
            catch (Exception e)
            {
                Current = defaultSettings;
                Logger.Current.Error($"Loading settings from file '{FileName}' failed", e);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await Serialization.SerializeToFileAsync(Current, FileName, ApplicationData.Current.LocalFolder);
                Logger.Current.Info($"Settings saved to file '{FileName}'");
            }
            catch (Exception e)
            {
                Logger.Current.Error($"Saving settings to file '{FileName}' failed", e);
            }
        }
    }
}
