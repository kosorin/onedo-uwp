using OneDo.Model.Entity;
using System.Collections.Generic;

namespace OneDo.Model.DataAccess
{
    public class Data
    {
        public Settings Settings { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Todo> Todos { get; set; }
    }
}