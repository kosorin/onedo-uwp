using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading.Tasks;

namespace OneDo.Services.InfoService
{
    public interface IInfoService
    {
        IMessenger Messenger { get; }

        void Hide();

        void Show(string text);

        void Show(string text, TimeSpan duration, string color);

        void Show(string text, string actionGlyph, string actionText, Func<Task> action);

        void Show(string text, TimeSpan duration, string color, string actionGlyph, string actionText, Func<Task> action);
    }
}
