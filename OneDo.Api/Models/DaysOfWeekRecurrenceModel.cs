using OneDo.Common;
using OneDo.Domain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Models
{
    public class DaysOfWeekRecurrenceModel : RecurrenceModel
    {
        public DaysOfWeek DaysOfWeek { get; set; }

        internal override Recurrence ToEntity()
        {
            return new WeeklyRecurrence(DaysOfWeek, Every, Until);
        }
    }
}