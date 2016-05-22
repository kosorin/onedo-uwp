using OneDo.Model.Data.Objects;
using System;
using System.Collections.Generic;

namespace OneDo.Model.Business.Validation
{
    public class TodoValidator : Validator<Todo>
    {
        protected override List<Rule<Todo>> Rules { get; } = new List<Rule<Todo>>
        {
            new Rule<Todo>(t => t.Date != null),
            new Rule<Todo>(t => t.Reminder != null && t.Date != null),
        };

        protected override Dictionary<string, IPropertyRule> PropertyRules { get; } = new Dictionary<string, IPropertyRule>
        {
            [nameof(Todo.Id)] = new PropertyRule<Guid>(guid => guid != Guid.Empty),
            [nameof(Todo.Title)] = new PropertyRule<string>(title => !string.IsNullOrWhiteSpace(title)),
        };
    }
}
