using OneDo.Model.Data.Objects;
using System;

namespace OneDo.Services.Context
{
    public class Context : IContext
    {
        public Guid TodoId { get; set; }
    }
}
