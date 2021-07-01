using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.Exceptions;
using Minglesports.Tasks.Core.OperationHandlers;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Commands;
using Minglesports.Tasks.Core.Ports;
using Moq;
using Xunit;

namespace Minglesports.Tasks.Tests.OperationHandlers
{
    public class DeleteTaskCommandHandlerTests
    {
        [Fact]
        public async Task GivenDeleteTaskCommand_WhenTaskExists_TaskShouldBeDelete()
        {
            var fixture = new TestFixture();

            var todoList = fixture.Create<TodoListAggregate>();
            var sut = fixture
                .WithTodoList(todoList)
                .WithCurrentUserId(todoList.EntityId.UserId)
                .GetHandler();

            var taskId = todoList.Tasks.First().EntityId;

            // act
            await sut.Handle(new DeleteTaskCommand(taskId), CancellationToken.None);

            // assert
            todoList.Tasks.Should().BeEmpty();
            fixture.AssertCommitWasCalled();
        }

        [Fact]
        public async Task GivenDeleteTaskCommand_WhenTaskNotFound_ExceptionShouldBeThrown()
        {
            var sut = new TestFixture()
                .WithCurrentUserId(UserId.Define("me"))
                .GetHandler();

            // assert
            Func<Task> act = async () => { await sut.Handle(new DeleteTaskCommand(TaskId.New()), CancellationToken.None); };
            await act.Should().ThrowAsync<NotFoundException>();
        }

        private class TestFixture : Fixture
        {
            private readonly Mock<ITodoListUnitOfWork> _todoListUnitOfWork;
            public readonly StubUserContextProvider UserContextProvider = StubUserContextProvider.Random();

            public TestFixture()
            {
                Customize(new TodoListDomainCustomization());
                _todoListUnitOfWork = this.Freeze<Mock<ITodoListUnitOfWork>>();

                _todoListUnitOfWork.Setup(x => x.CommitAsync())
                    .Returns(Task.CompletedTask);
            }

            public TestFixture WithCurrentUserId(UserId userId)
            {
                UserContextProvider.UserContext = new CurrentUserContext(new User(userId));
                return this;
            }

            public TestFixture WithTodoList(TodoListAggregate todoList)
            {
                _todoListUnitOfWork.Setup(x => x.GetAsync(It.IsAny<TodoListIdentifier>()))
                    .ReturnsAsync(todoList);

                return this;
            }

            public void AssertCommitWasCalled()
            {
                _todoListUnitOfWork.Verify(x => x.CommitAsync(), Times.AtLeastOnce());
            }

            public IRequestHandler<DeleteTaskCommand> GetHandler()
            {
                return new DeleteTaskCommandHandler(_todoListUnitOfWork.Object, UserContextProvider);
            }
        }
    }
}