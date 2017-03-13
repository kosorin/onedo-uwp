using Microsoft.Toolkit.Uwp.Notifications;
using OneDo.Domain.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace OneDo.Application.Notifications
{
    public class ContentBuilder
    {
        public XmlDocument Build(Note note)
        {
            var visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = note.Title,
                        },
                        new AdaptiveText()
                        {
                            Text = note.Text,
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
            selectionBox.Items.Add(new ToastSelectionBoxItem("15", "15 minutes"));
            selectionBox.Items.Add(new ToastSelectionBoxItem("60", "1 hour"));
            selectionBox.Items.Add(new ToastSelectionBoxItem("240", "4 hours"));
            selectionBox.Items.Add(new ToastSelectionBoxItem("1440", "1 day"));
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
                        SelectionBoxId = selectionBox.Id,
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
            return xml;
        }
    }
}
