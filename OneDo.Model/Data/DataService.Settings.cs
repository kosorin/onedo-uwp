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
    public partial class DataService : IDisposable
    {
        private const string SettingsFileName = "OneDo.Settings.json";


        public Settings Settings { get; private set; } = new Settings();

        public async Task LoadSettingsAsync()
        {
            var defaultSettings = new Settings();
            try
            {
                Settings = await Serialization.DeserializeFromFileAsync<Settings>(SettingsFileName, ApplicationData.Current.LocalFolder) ?? defaultSettings;
                Logger.Current.Info($"Settings loaded from file '{SettingsFileName}'");
            }
            catch (Exception e)
            {
                Settings = defaultSettings;
                Logger.Current.Error($"Loading settings from file '{SettingsFileName}' failed", e);
            }
        }

        public async Task SaveSettingsAsync()
        {
            try
            {
                await Serialization.SerializeToFileAsync(Settings, SettingsFileName, ApplicationData.Current.LocalFolder);
                Logger.Current.Info($"Settings saved to file '{SettingsFileName}'");
            }
            catch (Exception e)
            {
                Logger.Current.Error($"Saving settings to file '{SettingsFileName}' failed", e);
            }
        }


        private void DisposeSettings()
        {

        }
    }
}
