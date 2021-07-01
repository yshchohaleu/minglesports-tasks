using System.Linq;
using MediatR;
using Minglesports.Tasks.Core.Domain;

namespace Minglesports.Tasks.Core.OperationHandlers.Requests.Queries
{
    public record GetTodoListQuery : IRequest<TodoListModel>;

    public record TodoListModel (string Id, TaskEntity[] Tasks)
    {
        public static TodoListModel FromDomain(TodoListAggregate todo)
        {
            return new(todo.EntityId.ToString(), todo.Tasks.ToArray());
        }
    }
}