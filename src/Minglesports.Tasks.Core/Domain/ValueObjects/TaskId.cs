using System;
using Minglesports.Tasks.BuildingBlocks.Domain;

namespace Minglesports.Tasks.Core.Domain.ValueObjects
{
    public class TaskId : SingleValueObject<Guid>
    {
        private TaskId(Guid value) : base(value)
        {
        }

        public static TaskId New() => new (Guid.NewGuid());
        public static TaskId FromString(string value) => new (Guid.Parse(value));
    }
}