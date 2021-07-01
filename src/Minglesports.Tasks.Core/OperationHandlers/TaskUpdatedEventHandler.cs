using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Events;

namespace Minglesports.Tasks.Core.OperationHandlers
{
    public class TaskUpdatedEventHandler : INotificationHandler<TaskUpdatedEvent>
    {
        private readonly ILogger<TaskUpdatedEventHandler> _logger;

        public TaskUpdatedEventHandler(ILogger<TaskUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(TaskUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling TaskUpdatedEventHandler... TaskId=[{Id}]", notification.Id);
            return Task.CompletedTask;
        }
    }
}