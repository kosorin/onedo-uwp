using Autofac;
using OneDo.Application.Common;
using OneDo.Application.Folders;
using OneDo.Application.Services.DataService;
using OneDo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Core
{
    public class CommandDispatcher : ICommandDispatcher
    {
        public IContainer Container { get; }

        public CommandDispatcher(IDataService dataService)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(dataService).As<IDataService>().ExternallyOwned().SingleInstance();
            builder.RegisterType<DateTimeService>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<FolderCommandHandler>().AsImplementedInterfaces().SingleInstance();

            Container = builder.Build();
        }

        public async Task Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handler = Container.Resolve<ICommandHandler<TCommand>>();
            if (handler == null)
            {
                throw new NotSupportedException($"Command '{typeof(TCommand).Name}' not supported");
            }

            await handler.Handle(command);
        }
    }
}
