using System;
using MediatR;
using Minglesports.Tasks.BuildingBlocks.Messages;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;

namespace Minglesports.Tasks.Core.OperationHandlers.Requests.Events
{
    public record TaskUpdatedEvent(TaskId Id, TaskName Name, string Description, DateTime Deadline, TaskStatus Status)
        : INotification, IEvent;
}