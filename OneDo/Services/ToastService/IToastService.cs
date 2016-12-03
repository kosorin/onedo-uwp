using System;
using System.Collections.Generic;
using Windows.UI.Notifications;

namespace OneDo.Services.ToastService
{
    public interface IToastService
    {
        void Show(ToastNotification toast);

        IEnumerable<ScheduledToastNotification> GetAllScheduledToasts();

        void AddToSchedule(ScheduledToastNotification toast);

        void RemoveFromSchedule(ScheduledToastNotification toast);

        void RemoveAllFromSchedule();

        ScheduledToastNotification CreateToast(string title, string detail, DateTime dateTime);
    }
}