using System;
using Ardalis.GuardClauses;
using Minglesports.Tasks.BuildingBlocks.Domain;

namespace Minglesports.Tasks.Core.Domain.ValueObjects
{
    public class TaskId : SingleValueObject<string>
    {
        private TaskId(string value) : base(value)
        {
        }

        public static TaskId New() => new (Guid.NewGuid().ToString());
        public static TaskId FromString(string value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));
            return new(value);
        }
    }
}