using OneDo.Application.Services.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Core
{
    public class QueryFactory
    {
        public IDataService DataService { get; }

        public QueryFactory(IDataService dataService)
        {
            DataService = dataService;
        }


    }
}
