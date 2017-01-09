using OneDo.Common;
using OneDo.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Common
{
    public interface ICommandHandler<in TCommand> : IHandler<TCommand> where TCommand : ICommand
    {
    }
}
