using System.Threading.Tasks;

namespace Minglesports.Tasks.BuildingBlocks.Persistence
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}