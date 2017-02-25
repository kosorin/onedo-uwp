using Microsoft.Toolkit.Uwp.Notifications;
using OneDo.Common.Logging;
using OneDo.Domain.Model.Entities;
using System;
using System.Collections.Generic;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace OneDo.Application.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly ToastNotifier notifier;

        private readonly ContentBuilder contentBuilder;

        public NotificationService()
        {
            notifier = ToastNotificationManager.CreateToastNotifier();
            contentBuilder = new ContentBuilder();
        }


        public void Reschedule(Note note)
        {
            CancelScheduled(note.Id);
            Schedule(note);
        }

        public void Schedule(Note note)
        {
            if (note.Reminder == null)
            {
                return;
            }

            foreach (var occurrence in note.Reminder.GetOccurrences())
            {
                var group = GetGroup(note.Id);
                if (occurrence < DateTime.Now.AddSeconds(5))
                {
                    Logger.Current.Warn($"Cannot schedule '{group}' on {occurrence} - it's in the past");
                    continue;
                }

                var content = contentBuilder.Build(note);
                var toast = new ScheduledToastNotification(content, occurrence);
                toast.Group = group;

                Logger.Current.Info($"Add to schedule '{toast.Group}.{toast.Tag}' on {toast.DeliveryTime}");
                notifier.AddToSchedule(toast);
            }
        }

        public void CancelScheduled(Guid id)
        {
            var group = GetGroup(id);

            Logger.Current.Info($"Remove from schedule '{group}'");
            foreach (var toast in notifier.GetScheduledToastNotifications())
            {
                if (toast.Group == group)
                {
                    notifier.RemoveFromSchedule(toast);
                }
            }
        }

        public void ClearSchedule()
        {
            Logger.Current.Info($"Remove all from schedule");
            foreach (var toast in notifier.GetScheduledToastNotifications())
            {
                notifier.RemoveFromSchedule(toast);
            }
        }


        private string GetGroup(Guid id)
        {
            var group = id.ToString("N").Substring(0, 15);
            return group;
        }
    }
}
