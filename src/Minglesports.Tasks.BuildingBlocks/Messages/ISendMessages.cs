using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minglesports.Tasks.BuildingBlocks.Messages
{
    public interface ISendMessages
    {
        Task PublishEvent<T>(T @event) where T: IEvent;
        Task PublishEvents<T>(IEnumerable<T> events) where T: IEvent;
    }
}