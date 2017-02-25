using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Domain.Model.ValueObjects;

namespace OneDo.Application.Models
{
    public abstract class StandardRecurrenceModel : RecurrenceModel
    {
        public int Every { get; set; }

        public DateTime? Until { get; set; }
    }
}
