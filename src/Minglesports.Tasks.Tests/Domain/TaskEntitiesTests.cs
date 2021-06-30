using System;
using AutoFixture;
using FluentAssertions;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Xunit;

namespace Minglesports.Tasks.Tests.Domain
{
    public class TaskEntitiesTests
    {
        private readonly IFixture _fixture = new Fixture()
            .Customize(new TodoListDomainCustomization());

        [Fact]
        public void GivenTask_WhenCreatedAndDeadlineInThePast_ExceptionShouldBeThrown()
        {
            var now = DateTime.UtcNow;
            var deadline = now.AddDays(-1);

            // act
            Action act = () => TaskEntity.Create(
                    _fixture.Create<TaskId>(),
                    _fixture.Create<TaskName>(),
                    deadline,
                    now
                );

            // assert
            act.Should().Throw<ArgumentException>()
                .Where(e => e.Message.ToLower()
                    .Contains(nameof(TaskEntity.DeadlineUtc).ToLower()));
        }
    }
}