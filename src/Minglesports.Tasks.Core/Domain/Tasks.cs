using System.Collections.ObjectModel;
using System.Linq;
using Minglesports.Tasks.Core.Domain.ValueObjects;

namespace Minglesports.Tasks.Core.Domain
{
    public class Tasks : Collection<TaskEntity>
    {
        public bool Exists(TaskId id) => this.Any(x => x.EntityId == id);

        public TaskEntity GetById(TaskId id) => this.SingleOrDefault(x => x.EntityId == id);
    }
}