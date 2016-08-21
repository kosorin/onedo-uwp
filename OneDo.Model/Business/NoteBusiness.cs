using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;

namespace OneDo.Model.Business
{
    public class NoteBusiness : EntityBusiness<Note>
    {
        public NoteBusiness(ISettingsProvider settingsProvider) : base(settingsProvider)
        {

        }

        public override Note Clone(Note entity)
        {
            return entity;
        }

        public void ToggleComplete(Note note)
        {
            if (note.Completed == null)
            {
                note.Completed = DateTime.Now;
            }
            else
            {
                note.Completed = null;
            }
        }
    }
}
