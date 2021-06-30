using System;
using Ardalis.GuardClauses;
using Minglesports.Tasks.BuildingBlocks.Domain;
using Minglesports.Tasks.BuildingBlocks.Guards;
using Minglesports.Tasks.Core.Domain.ValueObjects;

namespace Minglesports.Tasks.Core.Domain
{
    public class TaskEntity : BaseEntity<TaskId>
    {
        private TaskEntity()
        {
        }

        public TaskName Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreateAtUtc { get; private init; }
        public DateTime DeadlineUtc { get; private set; }
        public TaskStatus Status { get; private set; }

        public static TaskEntity Create(
            TaskId id,
            TaskName name,
            DateTime deadlineUtc,
            DateTime createdAtUtc,
            string description = null)
        {
            Guard.Against.Null(name, nameof(name));
            Guard.Against.Default(deadlineUtc, nameof(deadlineUtc));
            Guard.Against.GreaterThan(createdAtUtc, deadlineUtc, nameof(deadlineUtc));

            return new()
            {
                EntityId = id,
                Name = name,
                DeadlineUtc = deadlineUtc,
                Description = description,

                Status = TaskStatus.Pending,
                CreateAtUtc = createdAtUtc
            };
        }

        public void Update(TaskName name, DateTime deadlineUtc, TaskStatus status, string description)
        {
            Guard.Against.Null(name, nameof(name));
            Guard.Against.Default(deadlineUtc, nameof(deadlineUtc));

            // check deadline ?

            Name = name;
            DeadlineUtc = deadlineUtc;
            Description = description;
            Status = status;
        }
    }
}