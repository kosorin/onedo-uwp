using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;

namespace OneDo.Services.InfoService
{
    public class InfoMessage
    {
        public string Text { get; set; }

        public TimeSpan Duration { get; set; }

        public string Color { get; set; }

        public InfoAction Action { get; set; }

        public class InfoAction
        {
            public Func<Task> Action { get; set; }

            public string Glyph { get; set; }

            public string Text { get; set; }
        }
    }
}
