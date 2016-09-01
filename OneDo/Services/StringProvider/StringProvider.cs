using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace OneDo.Services.StringProvider
{
    public class StringProvider : ViewModelBase, IStringProvider
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
