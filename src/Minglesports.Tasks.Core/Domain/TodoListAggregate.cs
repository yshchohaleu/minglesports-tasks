using System;
using System.Collections.Generic;
using Minglesports.Tasks.BuildingBlocks.Domain;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.Exceptions;

namespace Minglesports.Tasks.Core.Domain
{
    public class TodoListAggregate : BaseAggregateRoot<TodoListId>
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
                EntityId = TodoListId.Define(userId)
            };
        }

        public void AddTask(TaskId id, TaskName name, DateTime deadline, DateTime now, string description = null)
        {
            if (!_tasks.Exists(id))
            {
                _tasks.Add(TaskEntity.Create(id, name, deadline, now, description));
            }
        }

        public void UpdateTask(TaskId id, TaskName name, DateTime deadlineUtc, string description = null)
        {
            var task = GetTaskById(id);
            task.Update(name, deadlineUtc, description);
        }

        public void DeleteTask(TaskId id)
        {
            if (_tasks.Exists(id))
            {
                var task = _tasks.GetById(id);
                _tasks.Remove(task);
            }
        }

        private TaskEntity GetTaskById(TaskId id)
        {
            var task = _tasks.GetById(id);
            if (task == null)
            {
                throw new NotFoundException($"Task with Id = {id} was not found");
            }

            return task;
        }
    }
}