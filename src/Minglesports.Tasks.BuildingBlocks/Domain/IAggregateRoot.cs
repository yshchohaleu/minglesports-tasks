using System.Collections.Generic;
using Minglesports.Tasks.BuildingBlocks.Messages;

namespace Minglesports.Tasks.BuildingBlocks.Domain
{
    public interface IAggregateRoot
    {
        IEnumerable<IEvent> GetUncommittedEvents();
        void ClearUncommittedMessages();
    }
}