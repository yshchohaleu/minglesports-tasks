using System.Collections.Generic;
using System.Threading.Tasks;
using Minglesports.Tasks.BuildingBlocks.Messages;

namespace Minglesports.Tasks.Tests
{
    public class StubMessageSender : ISendMessages
    {
        public Task PublishEvent<T>(T @event) where T : IEvent
        {
            return Task.CompletedTask;;
        }

        public Task PublishEvents<T>(IEnumerable<T> events) where T : IEvent
        {
            return Task.CompletedTask;;
        }
    }
}