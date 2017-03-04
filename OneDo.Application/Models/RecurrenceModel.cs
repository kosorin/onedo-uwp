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
    public abstract class RecurrenceModel : Equatable<RecurrenceModel>, IValueModel
    {
        internal abstract Recurrence ToEntity();

        internal static RecurrenceModel FromData(Recurrence recurrenceValueObject)
        {
            if (recurrenceValueObject == null)
            {
                return new OnceRecurrenceModel();
            }
            else if (recurrenceValueObject is DailyRecurrence)
            {
                return new DailyRecurrenceModel
                {
                    Every = recurrenceValueObject.Every,
                    Until = recurrenceValueObject.Until,
                };
            }
            else if (recurrenceValueObject is WeeklyRecurrence)
            {
                var weeklyRecurrenceValueObject = (WeeklyRecurrence)recurrenceValueObject;
                if (weeklyRecurrenceValueObject.DaysOfWeek == DaysOfWeek.None)
                {
                    return new WeeklyRecurrenceModel
                    {
                        Every = weeklyRecurrenceValueObject.Every,
                        Until = weeklyRecurrenceValueObject.Until,
                    };
                }
                else
                {
                    return new DaysOfWeekRecurrenceModel
                    {
                        Every = weeklyRecurrenceValueObject.Every,
                        Until = weeklyRecurrenceValueObject.Until,
                        DaysOfWeek = weeklyRecurrenceValueObject.DaysOfWeek,
                    };
                }
            }
            else if (recurrenceValueObject is MonthlyRecurrence)
            {
                return new MonthlyRecurrenceModel
                {
                    Every = recurrenceValueObject.Every,
                    Until = recurrenceValueObject.Until,
                };
            }
            else
            {
                throw new NotSupportedException($"Given recurrence '{recurrenceValueObject.GetType().Name}' is not supported");
            }
        }
    }
}
