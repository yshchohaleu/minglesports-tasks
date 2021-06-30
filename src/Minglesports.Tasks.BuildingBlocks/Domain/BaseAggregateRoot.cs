using System.Collections.Generic;
using System.Linq;
using Minglesports.Tasks.BuildingBlocks.Messages;

namespace Minglesports.Tasks.BuildingBlocks.Domain
{
    public abstract class BaseAggregateRoot<T> : BaseEntity<T>, IAggregateRoot
    {
        public virtual void ClearUncommittedMessages()
        {
            ClearUncommittedEvents();
        }

        #region Events

        private readonly List<IEvent> _uncommittedEvents = new ();
        public virtual IEnumerable<IEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents.AsEnumerable();
        }

        public virtual void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        protected virtual void PublishEvent(IEvent @event)
        {
            _uncommittedEvents.Add(@event);
        }

        #endregion
    }
}