using OneDo.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.Entities
{
    public class Note : Entity
    {
        public Guid FolderId { get; private set; }

        public string Title { get; private set; }

        public string Text { get; private set; }

        public DateTime? Date { get; private set; }

        public TimeSpan? Reminder { get; private set; }

        public bool IsFlagged { get; private set; }

        public Note(Guid id, Guid folderId, string title, string text, DateTime? date, TimeSpan? reminder, bool isFlagged) : base(id)
        {
            FolderId = folderId;
            Title = title;
            Text = text;
            Date = date;
            Reminder = reminder;
            IsFlagged = isFlagged;
        }

        public void ChangeTitle(string title)
        {
            Title = title;
        }

        public void ChangeText(string text)
        {
            Text = text;
        }

        public void ChangeDate(DateTime? date)
        {
            Date = date;
        }

        public void ChangeReminder(TimeSpan time)
        {
            Reminder = time;
        }

        public void ToggleFlag()
        {
            IsFlagged = !IsFlagged;
        }
    }
}
