using System;
using System.Threading.Tasks;
using Minglesports.Tasks.BuildingBlocks.Persistence;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;

namespace Minglesports.Tasks.Core.Ports
{
    public interface ITodoListUnitOfWork : IUnitOfWork
    {
        Task<TodoListAggregate> GetOrCreateAsync(TodoListIdentifier id, Func<TodoListAggregate> createFunc);
        Task<TodoListAggregate> GetAsync(TodoListIdentifier id);
    }
}