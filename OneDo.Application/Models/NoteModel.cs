using OneDo.Application.Common;
using OneDo.Common;
using OneDo.Domain.Model.Entities;
using OneDo.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReminderVO = OneDo.Domain.Model.ValueObjects.Reminder;

namespace OneDo.Application.Models
{
    public class NoteModel : IModel
    {
        public Guid Id { get; set; }

        public Guid FolderId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public ReminderModel Reminder { get; set; }

        public bool IsFlagged { get; set; }


        internal Note ToEntity()
        {
            return new Note(Id, FolderId, Title, Text, Reminder?.ToEntity(), IsFlagged);
        }

        internal static NoteModel FromData(NoteData noteData)
        {
            var reminder = ReminderVO.Load(noteData.Reminder);
            return new NoteModel
            {
                Id = noteData.Id,
                FolderId = noteData.FolderId,
                Title = noteData.Title,
                Text = noteData.Text,
                Reminder = ReminderModel.FromData(noteData.Reminder),
                IsFlagged = noteData.IsFlagged,
            };
        }
    }
}
