using System;
using Windows.UI.Notifications;

namespace OneDo.Services.ToastService
{
    public interface IToastService
    {
        void Show(ToastNotification toast);

        void AddToSchedule(ScheduledToastNotification toast);

        void RemoveFromSchedule(ScheduledToastNotification toast);

        void RemoveAllFromSchedule();

        ScheduledToastNotification CreateToast(string title, string detail, DateTime dateTime);
    }
}