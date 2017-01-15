using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Queries.Notes
{
    public class NoteModel
    {
        public Guid Id { get; set; }

        public Guid FolderId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime? Date { get; set; }

        public TimeSpan? Reminder { get; set; }

        public bool IsFlagged { get; set; }
    }
}
