using MassTransit;
using MessageBroker.Abstractions;

namespace SharedLibrary.Base.MessageBuses
{
    public abstract class EventConsumer<TEvent> : IConsumer<TEvent>
        where TEvent : BaseEvent
    {
        public async Task Consume(ConsumeContext<TEvent> context)
        {
            await this.Handle(context);
        }

        protected abstract Task Handle(ConsumeContext<TEvent> context);
    }
}