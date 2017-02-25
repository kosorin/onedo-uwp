using OneDo.Application.Common;
using OneDo.Common;
using OneDo.Common.Extensions;
using OneDo.Domain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Models
{
    public class ReminderModel : Equatable<ReminderModel>, IValueModel
    {
        public DateTime DateTime { get; set; }

        public RecurrenceModel Recurrence { get; set; }

        internal Reminder ToEntity()
        {
            return new Reminder(DateTime, Recurrence?.ToEntity());
        }

        internal static ReminderModel FromData(string reminderData)
        {
            var reminder = Reminder.Load(reminderData);
            if (reminder == null)
            {
                return null;
            }
            return new ReminderModel
            {
                DateTime = reminder.DateTime,
                Recurrence = RecurrenceModel.FromData(reminder?.Recurrence),
            };
        }

        protected override bool EqualsCore(ReminderModel other)
        {
            return DateTime == other.DateTime && Recurrence == other.Recurrence;
        }

        protected override int GetHashCodeCore()
        {
            return this.GetHashCodeFromFields(DateTime, Recurrence);
        }
    }
}
