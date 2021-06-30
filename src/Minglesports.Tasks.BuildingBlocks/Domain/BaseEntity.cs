using System.ComponentModel.DataAnnotations;

namespace Minglesports.Tasks.BuildingBlocks.Domain
{
    public abstract class BaseEntity<TId>
    {
        public TId EntityId { get; protected set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
