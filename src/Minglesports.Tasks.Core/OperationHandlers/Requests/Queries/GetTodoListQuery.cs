using System;
using System.Linq;
using MediatR;
using Minglesports.Tasks.Core.Domain;

namespace Minglesports.Tasks.Core.OperationHandlers.Requests.Queries
{
    public record GetTodoListQuery : IRequest<TodoListModel>;

    public record TodoListModel (string Id, TaskModel[] Tasks)
    {
        public static TodoListModel FromDomain(TodoListAggregate todo)
        {
            return new(todo.EntityId.ToString(), todo.Tasks.Select(
                    task => new TaskModel(
                        task.EntityId,
                        task.Name,
                        task.DeadlineUtc,
                        task.Description,
                        task.Status.ToString(),
                        task.CreateAtUtc)
                ).ToArray());
        }
    }

    public record TaskModel(string Id, string Name, DateTime DeadlineUtc, string Description, string Status, DateTime CreateAtUtc);
}