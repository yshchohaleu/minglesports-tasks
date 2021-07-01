using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.Exceptions;
using Xunit;

namespace Minglesports.Tasks.Tests.Domain
{
    public class TodoListAggregateTests
    {
        private readonly IFixture _fixture = new Fixture()
            .Customize(new TodoListDomainCustomization());

        [Fact]
        public void GivenTodoList_WhenNewTaskIsAdded_TaskShouldBeAdded()
        {
            var todoList = TodoListAggregate.Create(_fixture.Create<UserId>());

            var taskId = _fixture.Create<TaskId>();
            var taskName = _fixture.Create<TaskName>();
            var today = DateTime.Now;
            var tomorrow = DateTime.Now.AddDays(1);
            var description = _fixture.Create<string>();

            // act
            todoList.AddTask(taskId, taskName, tomorrow, today, description);

            // assert
            todoList.Tasks.Should().HaveCount(1);
            var task = todoList.Tasks.Single();

            task.EntityId.Should().Be(taskId);
            task.Name.Should().Be(taskName);
            task.Status.Should().Be(TaskStatus.Pending);
            task.DeadlineUtc.Should().Be(tomorrow);
            task.CreateAtUtc.Should().Be(today);
            task.Description.Should().Be(description);
        }

        [Fact]
        public void GivenTodoList_WhenNewTaskIsAddedTwice_OnlyOneTaskShouldBeAdded()
        {
            var todoList = TodoListAggregate.Create(_fixture.Create<UserId>());

            var taskId = _fixture.Create<TaskId>();
            var taskName = _fixture.Create<TaskName>();
            var today = DateTime.Now;
            var tomorrow = DateTime.Now.AddDays(1);
            var description = _fixture.Create<string>();

            // act
            todoList.AddTask(taskId, taskName, tomorrow, today, description);
            todoList.AddTask(taskId, taskName, tomorrow, today, description);

            // assert
            todoList.Tasks.Should().HaveCount(1);
        }

        [Fact]
        public void GivenTodoList_WhenTaskIsUpdated_TaskShouldBeUpdated()
        {
            var todoList = _fixture.Create<TodoListAggregate>();
            var taskId = todoList.Tasks.Single().EntityId;

            var taskName = _fixture.Create<TaskName>();
            var deadline = _fixture.Create<DateTime>();
            var description = _fixture.Create<string>();
            var status = TaskStatus.Completed;

            // act
            todoList.UpdateTask(taskId, taskName, deadline, status, description);

            // assert
            todoList.Tasks.Should().HaveCount(1);
            var task = todoList.Tasks.Single();

            task.Name.Should().Be(taskName);
            task.Status.Should().Be(status);
            task.DeadlineUtc.Should().Be(deadline);
            task.Description.Should().Be(description);
        }

        [Fact]
        public void GivenTodoList_WhenTaskIsUpdatedButNotFound_ExceptionShouldBeThrown()
        {
            var todoList = _fixture.Create<TodoListAggregate>();

            // act
            Action act = () => todoList.UpdateTask(
                _fixture.Create<TaskId>(),
                _fixture.Create<TaskName>(),
                _fixture.Create<DateTime>(),
                TaskStatus.Completed
            );

            // assert
            act.Should().Throw<NotFoundException>();
        }

        [Fact]
        public void GivenTodoList_WhenTaskIsDeleted_TaskShouldBeDeleted()
        {
            var todoList = _fixture.Create<TodoListAggregate>();
            var taskId = todoList.Tasks.Single().EntityId;

            // act
            todoList.DeleteTask(taskId);

            // assert
            todoList.Tasks.Should().BeEmpty();
        }

        [Fact]
        public void GivenTodoList_WhenTaskIsDeletedTwice_OperationShouldSucceed()
        {
            var todoList = _fixture.Create<TodoListAggregate>();
            var taskId = todoList.Tasks.Single().EntityId;

            // act
            todoList.DeleteTask(taskId);
            todoList.DeleteTask(taskId);

            // assert
            todoList.Tasks.Should().BeEmpty();
        }
    }
}