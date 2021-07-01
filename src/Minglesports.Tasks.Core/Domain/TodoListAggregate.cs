using System;
using System.Collections.Generic;
using Minglesports.Tasks.BuildingBlocks.Domain;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.Exceptions;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Events;

namespace Minglesports.Tasks.Core.Domain
{
    public class TodoListAggregate : BaseAggregateRoot<TodoListIdentifier>
    {
        private TodoListAggregate()
        {
        }

        private readonly Tasks _tasks = new ();
        public IReadOnlyCollection<TaskEntity> Tasks => _tasks;

        public static TodoListAggregate Create(UserId userId)
        {
            return new()
            {
                EntityId = TodoListIdentifier.Define(userId)
            };
        }

        public void AddTask(TaskId id, TaskName name, DateTime deadlineUtc, DateTime createdAtUtc, string description = null)
        {
            if (!_tasks.Exists(id))
            {
                _tasks.Add(TaskEntity.Create(id, name, deadlineUtc, createdAtUtc, description));
            }
        }

        public void UpdateTask(TaskId id, TaskName name, DateTime deadlineUtc, TaskStatus status, string description = null)
        {
            var task = GetTaskByIdOrThrow(id);
            task.Update(name, deadlineUtc, status, description);

            PublishEvent(new TaskUpdatedEvent(id, name, description, deadlineUtc, status));
        }

        public void DeleteTask(TaskId id)
        {
            if (_tasks.Exists(id))
            {
                var task = _tasks.GetById(id);
                _tasks.Remove(task);
            }
        }

        private TaskEntity GetTaskByIdOrThrow(TaskId id)
        {
            var task = _tasks.GetById(id);
            if (task == null)
            {
                throw new NotFoundException($"Task with Id='{id}' was not found");
            }

            return task;
        }
    }
}