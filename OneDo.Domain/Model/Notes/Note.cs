using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.Notes
{
    public class Note : Entity
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime? Date { get; private set; }

        public TimeSpan? Reminder { get; private set; }

        public bool IsFlagged { get; private set; }

        public Guid FolderId { get; private set; }


        public void ClearDate()
        {
            Date = null;
        }

        public void SetDate(DateTime date)
        {
            Date = date;
        }

        public void ClearReminder()
        {
            Reminder = null;
        }

        public void SetReminder(TimeSpan time)
        {
            Reminder = time;
        }

        public void ToggleFlag()
        {
            IsFlagged = !IsFlagged;
        }
    }
}
