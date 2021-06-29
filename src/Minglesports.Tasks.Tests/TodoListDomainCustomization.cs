using System;
using System.Threading.Tasks;
using AutoFixture;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;

namespace Minglesports.Tasks.Tests
{
    public class TodoListDomainCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<UserId>(composer =>
                composer.FromFactory(() => UserId.Define($"user|{fixture.Create<string>()}")));

            fixture.Customize<TodoListId>(composer =>
                composer.FromFactory(() => TodoListId.Define(fixture.Create<UserId>())));

            fixture.Customize<TaskName>(composer =>
                composer.FromFactory(() => TaskName.Define($"task|{fixture.Create<string>()}")));

            fixture.Customize<TaskId>(composer =>
                composer.FromFactory(TaskId.New));

            fixture.Customize<TaskEntity>(composer =>
                composer.FromFactory(() =>
                        TaskEntity.Create(
                                fixture.Create<TaskId>(),
                                fixture.Create<TaskName>(),
                                fixture.Create<DateTime>(),
                                fixture.Create<DateTime>(),
                                fixture.Create<string>()
                            )
                    ));
        }
    }
}