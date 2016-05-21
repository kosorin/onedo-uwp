using OneDo.Model.Data.Objects;

namespace OneDo.Services.Context
{
    public interface IContext
    {
        Todo Todo { get; set; }
    }
}