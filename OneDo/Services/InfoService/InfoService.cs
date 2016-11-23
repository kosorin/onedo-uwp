using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneDo.Services.InfoService
{
    public class InfoService : IInfoService
    {
        public IMessenger Messenger => GalaSoft.MvvmLight.Messaging.Messenger.Default;

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