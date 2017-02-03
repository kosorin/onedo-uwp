using Autofac;
using OneDo.Application.Commands.Folders;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Common;
using OneDo.Infrastructure.Data;
using OneDo.Infrastructure.Data.Entities;
using OneDo.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Core
{
    public class CommandBus : ICommandBus
    {
        private readonly IContainer container;

        public CommandBus(IDataService dataService)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(dataService).As<IDataService>().ExternallyOwned().SingleInstance();
            builder.Register(c => c.Resolve<IDataService>().GetRepository<FolderData>()).AsImplementedInterfaces().AsSelf();
            builder.Register(c => c.Resolve<IDataService>().GetRepository<NoteData>()).AsImplementedInterfaces().AsSelf();

            builder.RegisterType<FolderCommandHandler>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<NoteCommandHandler>().AsImplementedInterfaces().SingleInstance();

            container = builder.Build();
        }

        public async Task Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handler = container.Resolve<ICommandHandler<TCommand>>();
            if (handler == null)
            {
                throw new NotSupportedException($"Command '{typeof(TCommand).Name}' is not supported");
            }

            await handler.Handle(command);
        }
    }
}
