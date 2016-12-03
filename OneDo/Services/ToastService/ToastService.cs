﻿using Microsoft.Toolkit.Uwp.Notifications;
using OneDo.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace OneDo.Services.ToastService
{
    public class ToastService : IToastService
    {
        public ToastNotifier Notifier { get; }

        public ToastService(ToastNotifier toastNotifier)
        {
            Notifier = toastNotifier;
        }

        public void Show(ToastNotification toast)
        {
            Notifier.Show(toast);
        }

        public IEnumerable<ScheduledToastNotification> GetAllScheduledToasts()
        {
            return Notifier.GetScheduledToastNotifications();
        }

        public void AddToSchedule(ScheduledToastNotification toast)
        {
            Logger.Current.Info($"Add to schedule '{toast.Group}.{toast.Tag}' on {toast.DeliveryTime}");
            Notifier.AddToSchedule(toast);
        }

        public void RemoveFromSchedule(ScheduledToastNotification toast)
        {
            Logger.Current.Info($"Remove from schedule '{toast.Group}.{toast.Tag}'");
            Notifier.RemoveFromSchedule(toast);
        }

        public void RemoveAllFromSchedule()
        {
            Logger.Current.Info($"Remove all from schedule");
            foreach (var toast in Notifier.GetScheduledToastNotifications())
            {
                Notifier.RemoveFromSchedule(toast);
            }
        }

        public ScheduledToastNotification CreateToast(string title, string detail, DateTime dateTime)
        {
            var visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = title,
                        },
                        new AdaptiveText()
                        {
                            Text = detail,
                        },
                    },
                }
            };

            var selectionBox = new ToastSelectionBox("snoozeTime")
            {
                DefaultSelectionBoxItemId = "15",
                Title = "Snooze until:",
            };
            selectionBox.Items.Add(new ToastSelectionBoxItem("5", "5 minutes"));
            selectionBox.Items.Add(new ToastSelectionBoxItem("10", "10 minutes"));
            selectionBox.Items.Add(new ToastSelectionBoxItem("60", "1 hour"));
            selectionBox.Items.Add(new ToastSelectionBoxItem("240", "4 hours"));
            selectionBox.Items.Add(new ToastSelectionBoxItem("144", "1 day"));
            var actions = new ToastActionsCustom()
            {
                Inputs =
                {
                    selectionBox,
                },
                Buttons =
                {
                    new ToastButtonSnooze
                    {
                        SelectionBoxId = "snoozeTime",
                    },
                    new ToastButtonDismiss(),
                }
            };


            var toastContent = new ToastContent()
            {
                Visual = visual,
                Actions = actions,
                Scenario = ToastScenario.Reminder,
            };

            var xml = toastContent.GetXml();
            var toast = new ScheduledToastNotification(xml, dateTime);
            return toast;
        }
    }
}
