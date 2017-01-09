using OneDo.Application.Core;
using OneDo.Application.Services.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application
{
    public class Api
    {
        public ICommandDispatcher CommandDispatcher { get; }

        private readonly IDataService dataService;

        public Api()
        {
            dataService = new DataService();

            CommandDispatcher = new CommandDispatcher(dataService);
        }
    }
}
