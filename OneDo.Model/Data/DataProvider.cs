﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Model.Data
{
    public class DataProvider : IDataProvider
    {
        public OneDoContext Context { get; private set; }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
            Context = null;
        }

        public void Initialize()
        {
            if (Context == null)
            {
                Context = new OneDoContext();
            }
        }
    }
}