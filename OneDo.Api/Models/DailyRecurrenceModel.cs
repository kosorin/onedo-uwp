using OneDo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Domain.Model.ValueObjects;

namespace OneDo.Application.Models
{
    public class DailyRecurrenceModel : RecurrenceModel
    {
        internal override Recurrence ToEntity()
        {
            return new DailyRecurrence(Every, Until);
        }
    }
}