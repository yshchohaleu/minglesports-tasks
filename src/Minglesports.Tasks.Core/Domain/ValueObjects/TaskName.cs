using Ardalis.GuardClauses;
using Minglesports.Tasks.BuildingBlocks.Domain;
using Minglesports.Tasks.BuildingBlocks.Guards;

namespace Minglesports.Tasks.Core.Domain.ValueObjects
{
    public class TaskName : SingleValueObject<string>
    {
        private TaskName(string value) : base(value)
        {
        }

        public static TaskName Define(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.Length(name, 100, nameof(name));

            return new TaskName(name);
        }
    }
}