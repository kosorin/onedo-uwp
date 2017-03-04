using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Domain.Model.ValueObjects;
using OneDo.Common.Extensions;

namespace OneDo.Application.Models
{
    public class OnceRecurrenceModel : RecurrenceModel
    {
        internal override Recurrence ToEntity()
        {
            return null;
        }


        protected override bool EqualsCore(RecurrenceModel other)
        {
            return other is DailyRecurrenceModel;
        }

        protected override int GetHashCodeCore()
        {
            return this.GetHashCodeFromFields(default(int));
        }
    }
}