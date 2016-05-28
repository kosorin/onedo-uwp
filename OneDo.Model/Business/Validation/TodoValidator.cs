using OneDo.Model.Data.Objects;
using System;
using System.Collections.Generic;

namespace OneDo.Model.Business.Validation
{
    public class TodoValidator : Validator<Todo>
    {
        protected override List<Rule<Todo>> Rules { get; } = new List<Rule<Todo>>
        {
            new Rule<Todo>(o => o.Date != null),
            new Rule<Todo>(o => o.Reminder != null && o.Date != null),
            new Rule<Todo>(o => o.Id != Guid.Empty),
            new Rule<Todo>(o => !string.IsNullOrWhiteSpace(o.Title)),
        };
    }
}
