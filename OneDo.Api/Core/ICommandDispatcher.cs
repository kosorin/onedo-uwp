using System.Threading.Tasks;
using OneDo.Application.Common;

namespace OneDo.Application.Core
{
    public interface ICommandDispatcher
    {
        Task Execute<TCommand>(TCommand command) where TCommand : ICommand;
    }
}