using OneDo.Domain.Common;
using OneDo.Domain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.Entities
{
    public class Note : Entity, IAggreagteRoot
    {
        public Guid FolderId { get; private set; }

        public string Title { get; private set; }

        public string Text { get; private set; }

        public Reminder Reminder { get; private set; }

        public bool IsFlagged { get; private set; }

        public Note(Guid id, Guid folderId, string title, string text, Reminder reminder, bool isFlagged) : base(id)
        {
            MoveToFolder(folderId);
            ChangeTitle(title);
            ChangeText(text);
            ChangeReminder(reminder);
            SetFlag(isFlagged);
        }

        public void MoveToFolder(Guid folderId)
        {
            if (folderId == null)
            {
                throw new ArgumentNullException(nameof(folderId), $"{nameof(folderId)} should not be null");
            }
            FolderId = folderId;
        }

        public void ChangeTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidOperationException($"{nameof(title)} should not be null, empty or white space");
            }
            Title = title.Trim();
        }

        public void ChangeText(string text)
        {
            Text = text?.Trim();
        }

        public void ChangeReminder(Reminder reminder)
        {
            Reminder = reminder;
        }

        public void SetFlag(bool isFlagged)
        {
            IsFlagged = isFlagged;
        }
    }
}
