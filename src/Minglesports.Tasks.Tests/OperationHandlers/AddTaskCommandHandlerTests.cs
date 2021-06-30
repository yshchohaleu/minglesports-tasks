using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Minglesports.Tasks.Application.OperationHandlers;
using Minglesports.Tasks.BuildingBlocks;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Commands;
using Minglesports.Tasks.Core.Ports;
using Moq;
using Xunit;
using TaskStatus = Minglesports.Tasks.Core.Domain.TaskStatus;

namespace Minglesports.Tasks.Tests.OperationHandlers
{
    public class AddTaskCommandHandlerTests
    {
        [Fact]
        public async Task GivenAddTaskCommand_WhenHandled_TaskShouldBeAdded()
        {
            var userId = UserId.Define("me");
            var now = DateTime.UtcNow;

            var todoList = TodoListAggregate.Create(userId);
            var fixture = new TestFixture()
                .WithCurrentUserId(userId)
                .WithTodoList(todoList)
                .SetupNowDateTime(now);

            var taskId = fixture.Create<TaskId>();
            var taskName = fixture.Create<TaskName>();
            var tomorrow = now.AddDays(1);
            var description = fixture.Create<string>();

            var sut = fixture.GetHandler();

            // act
            await sut.Handle(new AddTaskCommand(taskId, taskName, tomorrow, description), CancellationToken.None);

            // assert
            fixture.AssertCommitWasCalled();
            todoList.Tasks.Where(t => t.EntityId == taskId).Should().HaveCount(1);
            var task = todoList.Tasks.Single(t => t.EntityId == taskId);
            task.Description.Should().Be(description);
            task.Name.Should().Be(taskName);
            task.Status.Should().Be(TaskStatus.Pending);
            task.DeadlineUtc.Should().Be(tomorrow);
            task.CreateAtUtc.Should().Be(now);

        }

        private class TestFixture : Fixture
        {
            private readonly Mock<ITodoListUnitOfWork> _todoListUnitOfWork;
            public readonly StubUserContextProvider UserContextProvider = StubUserContextProvider.Random();
            private DateTime? _now = null;

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
                _todoListUnitOfWork.Setup(x => x.GetOrCreateAsync(It.IsAny<TodoListIdentifier>(),
                        It.IsAny<Func<TodoListAggregate>>()
                    ))
                    .ReturnsAsync(todoList);

                return this;
            }

            public TestFixture SetupNowDateTime(DateTime now)
            {
                _now = now;
                return this;
            }

            public void AssertCommitWasCalled()
            {
                _todoListUnitOfWork.Verify(x => x.CommitAsync(), Times.AtLeastOnce());
            }

            public IRequestHandler<AddTaskCommand> GetHandler()
            {
                return new AddTaskCommandHandler(_todoListUnitOfWork.Object, UserContextProvider,
                    _now.HasValue ? TimeProvider.WithPresetValue(_now.Value) : new TimeProvider());
            }
        }
    }
}