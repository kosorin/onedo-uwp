using OneDo.Model.Data.Objects;
using System.Collections.Generic;

namespace OneDo.Model.Data
{
    public class Data
    {
        public Settings Settings { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Todo> Todos { get; set; }
    }
}