using Autofac;
using OneDo.Application.Commands;
using OneDo.Application.Commands.Folders;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Common;
using OneDo.Application.Notifications;
using OneDo.Application.Repositories;
using OneDo.Domain.Model.Repositories;
using OneDo.Infrastructure.Data;
using OneDo.Infrastructure.Data.Entities;
using OneDo.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;
using Windows.UI.Notifications;

namespace OneDo.Application.Core
{
    public class CommandBus
    {
        private readonly IContainer container;

        internal CommandBus(EventBus eventBus, IDataService dataService)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(eventBus).AsSelf().ExternallyOwned().SingleInstance();

            builder.RegisterInstance(dataService).As<IDataService>().ExternallyOwned().SingleInstance();
            builder.Register(c => c.Resolve<IDataService>().GetRepository<FolderData>()).AsImplementedInterfaces().AsSelf();
            builder.Register(c => c.Resolve<IDataService>().GetRepository<NoteData>()).AsImplementedInterfaces().AsSelf();
            builder.RegisterType<FolderRepository>().As<IFolderRepository>();
            builder.RegisterType<NoteRepository>().As<INoteRepository>();

            builder.Register(c => ToastNotificationManager.CreateToastNotifier()).As<ToastNotifier>();
            builder.RegisterType<NotificationService>().As<INotificationService>().SingleInstance();

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
