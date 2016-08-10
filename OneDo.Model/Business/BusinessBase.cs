using OneDo.Model.Data;

namespace OneDo.Model.Business
{
    public abstract class BusinessBase
    {
        public ISettingsProvider SettingsProvider { get; set; }

        public BusinessBase(ISettingsProvider settingsProvider)
        {
            SettingsProvider = settingsProvider;
        }
    }
}
