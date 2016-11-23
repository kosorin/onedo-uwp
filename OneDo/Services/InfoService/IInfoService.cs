using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneDo.Services.InfoService
{
    public interface IInfoService
    {
        void Hide();

        void Show(string text);

        void Show(string text, TimeSpan duration, string color);

        void Show(string text, string actionGlyph, string actionText, Func<Task> action);

        void Show(string text, TimeSpan duration, string color, string actionGlyph, string actionText, Func<Task> action);
    }
}
