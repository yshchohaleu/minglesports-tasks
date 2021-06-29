using MediatR;
using Minglesports.Tasks.Core.Domain.ValueObjects;

namespace Minglesports.Tasks.Core.OperationHandlers.Requests.Commands
{
    public record DeleteTaskCommand(TaskId Id) : IRequest;
}