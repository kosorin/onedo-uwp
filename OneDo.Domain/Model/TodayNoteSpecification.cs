using OneDo.Domain.Common;
using OneDo.Domain.Model.Entities;
using OneDo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model
{
    public class TodayNoteSpecification : Specification<Note>
    {
        private readonly DateTimeService dateTimeService;

        public TodayNoteSpecification(DateTimeService dateTimeService)
        {
            this.dateTimeService = dateTimeService;
        }

        public override Expression<Func<Note, bool>> ToExpression()
        {
            return note => note.Date != null && note.Date <= dateTimeService.Today();
        }
    }
}
