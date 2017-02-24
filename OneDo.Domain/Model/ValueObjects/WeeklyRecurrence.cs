using Newtonsoft.Json;
using OneDo.Common;
using OneDo.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.ValueObjects
{
    public class WeeklyRecurrence : Recurrence
    {
        public DaysOfWeek DaysOfWeek { get; }

        [JsonConstructor]
        public WeeklyRecurrence(DaysOfWeek daysOfWeek, int every, DateTime? until) : base(every, until)
        {
            DaysOfWeek = daysOfWeek;
        }

        public override IEnumerable<DateTime> GetOccurrences(DateTime from)
        {
            var occurrence = from;

            if (DaysOfWeek == DaysOfWeek.None)
            {
                while (occurrence <= ActualUntil)
                {
                    yield return occurrence;
                    occurrence = occurrence.AddDays(Every * 7);
                }
                yield break;
            }
            else
            {
                while (true)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (occurrence > ActualUntil)
                        {
                            yield break;
                        }
                        if (occurrence.DayOfWeek.IsIn(DaysOfWeek))
                        {
                            yield return occurrence;
                        }
                        occurrence = occurrence.AddDays(1);
                    }
                    occurrence = occurrence.AddDays(7 * (Every - 1));
                }
            }
        }

        protected override bool EqualsCore(Recurrence other)
        {
            var weeklyRecurrence = other as WeeklyRecurrence;
            if (weeklyRecurrence != null)
            {
                return Every == weeklyRecurrence.Every
                    && Until == weeklyRecurrence.Until
                    && DaysOfWeek == weeklyRecurrence.DaysOfWeek;
            }
            else
            {
                return false;
            }
        }

        protected override int GetHashCodeCore()
        {
            return this.GetHashCodeFromFields(Every, Until, DaysOfWeek);
        }
    }
}