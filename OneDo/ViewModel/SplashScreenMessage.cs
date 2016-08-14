using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class SplashScreenMessage
    {
        public SplashScreenMessage(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
