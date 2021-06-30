using System;
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
                {
                    var now = DateTime.UtcNow;
                    var tomorrow = DateTime.UtcNow.AddDays(1);
                    var task = TaskEntity.Create(
                        fixture.Create<TaskId>(),
                        fixture.Create<TaskName>(),
                        tomorrow,
                        now,
                        fixture.Create<string>()
                    );
                    return task;
                }));

            fixture.Customize<TodoListAggregate>(composer =>
                composer.FromFactory(() =>
                    {
                        var now = DateTime.UtcNow;
                        var tomorrow = DateTime.UtcNow.AddDays(1);
                        var todoList = TodoListAggregate.Create(fixture.Create<UserId>());

                        todoList.AddTask(
                            fixture.Create<TaskId>(),
                            fixture.Create<TaskName>(),
                            tomorrow,
                            now,
                            fixture.Create<string>());
                        return todoList;
                    }
                ));
        }
    }
}