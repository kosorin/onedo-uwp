using OneDo.Application.Common;
using OneDo.Common;
using OneDo.Domain.Model.Entities;
using OneDo.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Models
{
    public class NoteModel : IModel
    {
        public Guid Id { get; set; }

        public Guid FolderId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime? Date { get; set; }

        public TimeSpan? Reminder { get; set; }

        public RecurrenceModel Recurrence { get; set; }

        public bool IsFlagged { get; set; }


        internal Note ToEntity()
        {
            return new Note(Id, FolderId, Title, Text, Date, Reminder, Recurrence?.ToEntity(), IsFlagged);
        }

        internal static NoteModel FromData(NoteData noteData)
        {
            return new NoteModel
            {
                Id = noteData.Id,
                FolderId = noteData.FolderId,
                Title = noteData.Title,
                Text = noteData.Text,
                Date = noteData.Date,
                Reminder = noteData.Reminder,
                Recurrence = RecurrenceModel.FromData(noteData.Recurrence),
                IsFlagged = noteData.IsFlagged,
            };
        }
    }
}
