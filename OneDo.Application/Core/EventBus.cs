using OneDo.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace OneDo.Application.Core
{
    public class EventBus
    {
        private readonly IMessengerHub hub;

        internal EventBus()
        {
            hub = new MessengerHub();
        }

        public void Subscribe<TEvent>(Action<TEvent> action) where TEvent : class, IEvent
        {
            hub.Subscribe<EventMessage<TEvent>>(m => action(m.Event), true);
        }

        public void Subscribe<TEvent>(Action<TEvent> action, Func<TEvent, bool> filter) where TEvent : class, IEvent
        {
            hub.Subscribe<EventMessage<TEvent>>(m => action(m.Event), m => filter(m.Event), true);
        }

        internal void Publish<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            hub.Publish(new EventMessage<TEvent>(@event));
        }

        private class EventMessage<TEvent> : MessageBase where TEvent : class, IEvent
        {
            public TEvent Event { get; }

            public EventMessage(TEvent @event) : base(0)
            {
                if (@event == null)
                {
                    throw new ArgumentNullException(nameof(@event));
                }
                Event = @event;
            }
        }
    }
}
