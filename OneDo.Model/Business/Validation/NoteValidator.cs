using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;

namespace OneDo.Model.Business.Validation
{
    public class NoteValidator : Validator<Note>
    {
        protected override List<Rule<Note>> Rules { get; } = new List<Rule<Note>>
        {
            new Rule<Note>(o => o.Date != null),
            new Rule<Note>(o => o.Reminder != null && o.Date != null),
            new Rule<Note>(o => o.Id != default(int)),
            new Rule<Note>(o => !string.IsNullOrWhiteSpace(o.Title)),
        };
    }
}
