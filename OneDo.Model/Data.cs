using OneDo.Model.Entity;
using System;
using System.Collections.Generic;

namespace OneDo.Model
{
    public class Data
    {
        public Settings Settings { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Todo> Todos { get; set; }
    }
}
