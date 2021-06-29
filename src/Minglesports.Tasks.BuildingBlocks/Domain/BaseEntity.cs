namespace Minglesports.Tasks.BuildingBlocks.Domain
{

    public interface IEntity<TId>
    {
        public abstract TId EntityId { get; }
    }
}