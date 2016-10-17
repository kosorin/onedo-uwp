using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Common;

namespace OneDo.Model.Business.Validation
{
    public class DateTimeBusiness : BusinessBase
    {
        public DateTimeBusiness(DataService dataService) : base(dataService)
        {

        }

        public DateTime Yesterday() => DateTime.Today.AddDays(-1);

        public DateTime Today() => DateTime.Today;

        public DateTime Tomorrow() => DateTime.Today.AddDays(1);

        public DateTime ThisWeek()
        {
            var date = DateTime.Today;
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date;
            }
            return date.NextDayOfWeek(DayOfWeek.Sunday);
        }

        public DateTime NextWeek()
        {
            return ThisWeek().AddWeeks(1);
        }
    }
}
