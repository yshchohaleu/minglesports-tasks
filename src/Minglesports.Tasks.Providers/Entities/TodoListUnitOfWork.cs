using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minglesports.Tasks.BuildingBlocks.Messages;
using Minglesports.Tasks.BuildingBlocks.Persistence;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.Ports;

namespace Minglesports.Tasks.Providers.Entities
{
    public class TodoListUnitOfWork : EfUnitOfWork<TodoListAggregate, TodoListDbContext>,
        ITodoListUnitOfWork
    {
        public TodoListUnitOfWork(TodoListDbContext dbContext, ISendMessages messageSender) : base(dbContext, messageSender)
        {
        }

        public async Task<TodoListAggregate> GetOrCreateAsync(TodoListIdentifier id, Func<TodoListAggregate> createFunc)
        {
            var todoList = await DbContext.TodoLists.SingleOrDefaultAsync(todo => todo.EntityId.Value == id.Value);
            if (todoList == null)
            {
                todoList = createFunc();
                await DbContext.TodoLists.AddAsync(todoList);
            }

            return todoList;
        }

        public Task<TodoListAggregate> GetAsync(TodoListIdentifier id)
        {
            return DbContext.TodoLists.SingleOrDefaultAsync(todo => todo.EntityId.Value == id.Value);
        }
    }
}