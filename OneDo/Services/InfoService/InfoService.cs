using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading.Tasks;

namespace OneDo.Services.InfoService
{
    public class InfoService : IInfoService
    {
        public IMessenger Messenger { get; } = new Messenger();

        public void Hide()
        {
            Messenger.Send<InfoMessage>(null);
        }

        public void Show(string text)
        {
            Show(text, InfoMessageDurations.Default, null);
        }

        public void Show(string text, TimeSpan duration, string color)
        {
            Messenger.Send(new InfoMessage
            {
                Text = text,
                Duration = duration,
                Color = color,
            });
        }

        public void Show(string text, string actionGlyph, string actionText, Func<Task> action)
        {
            Show(text, InfoMessageDurations.Default, null, actionGlyph, actionText, action);
        }

        public void Show(string text, TimeSpan duration, string color, string actionGlyph, string actionText, Func<Task> action)
        {
            Messenger.Send(new InfoMessage
            {
                Text = text,
                Duration = duration,
                Color = color,
                Action = new InfoMessage.InfoAction
                {
                    Action = action,
                    Glyph = actionGlyph,
                    Text = actionText,
                },
            });
        }
    }
}