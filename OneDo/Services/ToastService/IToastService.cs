using System;
using System.Collections.Generic;
using Windows.UI.Notifications;

namespace OneDo.Services.ToastService
{
    public interface IToastService
    {
        IEnumerable<ScheduledToastNotification> GetScheduledToasts();

        void AddToSchedule(ScheduledToastNotification toast);

        void RemoveFromSchedule(ScheduledToastNotification toast);

        void RemoveFromSchedule(string group);

        void ClearSchedule();

        ScheduledToastNotification CreateToast(string title, string detail, DateTime dateTime);
    }
}