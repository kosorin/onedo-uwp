using Windows.ApplicationModel.Resources;

namespace OneDo.Services.StringProvider
{
    public class StringProvider : IStringProvider
    {
        private readonly ResourceLoader loader;

        public StringProvider()
        {
            loader = new ResourceLoader();
        }

        public string GetString(string key)
        {
            return loader.GetString(key);
        }
    }
}
