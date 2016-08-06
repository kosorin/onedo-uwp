﻿using OneDo.Model.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Model.Data.Repositories
{
    public class TodoRepository : Repository<Todo>
    {
        public TodoRepository(List<Todo> items = null) : base(items)
        {

        }
    }
}