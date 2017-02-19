using OneDo.Common;
using OneDo.Domain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Models
{
    public class MonthlyRecurrenceModel : RecurrenceModel
    {
        internal override Recurrence ToEntity()
        {
            return new MonthlyRecurrence(Every, Until);
        }
    }
}