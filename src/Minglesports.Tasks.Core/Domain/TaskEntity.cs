using System;
using Ardalis.GuardClauses;
using Minglesports.Tasks.BuildingBlocks.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;

namespace Minglesports.Tasks.Core.Domain
{
    public class TaskEntity : IEntity<TaskId>
    {
        private TaskEntity()
        {
        }

        public TaskId EntityId { get; private init; }
        public TaskName Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreateAtUtc { get; private init; }
        public DateTime DeadlineUtc { get; private set; }
        public TaskStatus Status { get; private set; }

        public static TaskEntity Create(
            TaskId id,
            TaskName name,
            DateTime deadlineUtc,
            DateTime now,
            string description = null)
        {
            Guard.Against.Null(name, nameof(name));
            Guard.Against.Default(deadlineUtc, nameof(deadlineUtc));

            return new()
            {
                EntityId = id,
                Name = name,
                DeadlineUtc = deadlineUtc,
                Description = description,

                Status = TaskStatus.Pending,
                CreateAtUtc = now
            };
        }

        public void Update(TaskName name, DateTime deadlineUtc, string description)
        {
            Guard.Against.Null(name, nameof(name));
            Guard.Against.Default(deadlineUtc, nameof(deadlineUtc));

            Name = name;
            DeadlineUtc = deadlineUtc;
            Description = description;
        }
    }
}