using System;
using System.Threading.Tasks;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Xunit;
using AutoFixture;
using FluentAssertions;
using Minglesports.Tasks.BuildingBlocks;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Providers.Entities;


namespace Minglesports.Tasks.Tests.Repositories
{
    public class TodoListUnitOfWorkTests : SqliteTest
    {
        [Fact]
        public async Task GivenTodoList_WhenFind_ShouldBeReturned()
        {
            var todoList = TodoListAggregate.Create(Fixture.Create<UserId>());
            3.Times(() => todoList.AddTask(
                Fixture.Create<TaskId>(),
                Fixture.Create<TaskName>(),
                Fixture.Create<DateTime>(),
                Fixture.Create<DateTime>(),
                Fixture.Create<string>()
            ));

            var id = todoList.EntityId;

            await DbContext.TodoLists.AddAsync(todoList);
            await DbContext.SaveChangesAsync();

            var todoListUnitOfWork = new TodoListUnitOfWork(DbContext, new StubMessageSender());

            // act
            var result = await todoListUnitOfWork.GetOrCreateAsync(id, null);

            // assert
            result.EntityId.Should().Be(id);
            result.Tasks.Should().BeEquivalentTo(todoList.Tasks);
        }

        [Fact]
        public async Task GivenTodoListId_WhenFindAndDoesNotExists_NewOneShouldBeCreate()
        {
            var id = Fixture.Create<TodoListId>();
            var todoListUnitOfWork = new TodoListUnitOfWork(DbContext, new StubMessageSender());

            // act
            var result = await todoListUnitOfWork.GetOrCreateAsync(id, () =>
                TodoListAggregate.Create(id.UserId));
            await todoListUnitOfWork.CommitAsync();

            // assert
            result.EntityId.Should().Be(id);
            result.Tasks.Should().BeEmpty();

            DbContext.TodoLists.Should().HaveCount(1);
        }
    }
}