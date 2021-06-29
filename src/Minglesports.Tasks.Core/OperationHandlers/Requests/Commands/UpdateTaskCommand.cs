using System;
using MediatR;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;

namespace Minglesports.Tasks.Core.OperationHandlers.Requests.Commands
{
    public record UpdateTaskCommand(TaskId Id, TaskName Name, DateTime DeadlineUtc, string Description, TaskStatus Status)
        : IRequest;
}