using OneDo.Model.Data.Objects;

namespace OneDo.Services.Context
{
    public class Context : IContext
    {
        public Todo Todo { get; set; }
    }
}
