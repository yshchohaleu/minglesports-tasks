using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Minglesports.Tasks.BuildingBlocks.Messages
{
    public class MediatrMessageSender : ISendMessages
    {
        private readonly IMediator _mediator;

        public MediatrMessageSender(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task PublishEvent<T>(T @event) where T : IEvent
        {
            return _mediator.Publish(@event);
        }

        public Task PublishEvents<T>(IEnumerable<T> events) where T : IEvent
        {
            return Task.WhenAll(
                events.Select(@event => _mediator.Publish(@event))
            );;
        }
    }
}