using OneDo.Model.Data.Objects;
using System;
using System.Collections.Generic;

namespace OneDo.Model.Business.Validation
{
    public class TodoValidator : Validator<Todo>
    {
        protected override IDictionary<string, Rule> Rules { get; } = new Dictionary<string, Rule>
        {
            [nameof(Todo.Id)] = new Rule(t => t.Id != Guid.Empty),
            [nameof(Todo.Title)] = new Rule(t => !string.IsNullOrWhiteSpace(t.Title)),
        };
    }
}
