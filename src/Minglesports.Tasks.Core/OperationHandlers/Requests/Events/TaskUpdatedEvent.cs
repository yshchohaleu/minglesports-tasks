using System;
using MediatR;
using Minglesports.Tasks.BuildingBlocks.Messages;
using Minglesports.Tasks.Core.Domain;

namespace Minglesports.Tasks.Core.OperationHandlers.Requests.Events
{
    public record TaskUpdatedEvent(string Id, string Name, string Description, DateTime Deadline, TaskStatus Status)
        : INotification, IEvent;
}