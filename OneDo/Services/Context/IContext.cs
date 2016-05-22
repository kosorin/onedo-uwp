using OneDo.Model.Data.Objects;
using System;

namespace OneDo.Services.Context
{
    public interface IContext
    {
        Guid TodoId { get; set; }
    }
}