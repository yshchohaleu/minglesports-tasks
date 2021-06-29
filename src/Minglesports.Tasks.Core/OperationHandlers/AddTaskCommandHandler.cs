using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Minglesports.Tasks.BuildingBlocks;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.OperationHandlers.Requests;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Commands;
using Minglesports.Tasks.Core.Ports;

namespace Minglesports.Tasks.Core.OperationHandlers
{
    internal class AddTaskCommandHandler : AsyncRequestHandler<AddTaskCommand>
    {
        private readonly ITodoListUnitOfWork _unitOfWork;
        private readonly IUserContextProvider _userContextProvider;
        private readonly ITimeProvider _timeProvider;

        public AddTaskCommandHandler(
            ITodoListUnitOfWork unitOfWork,
            IUserContextProvider userContextProvider,
            ITimeProvider timeProvider)
        {
            _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
            _userContextProvider = Guard.Against.Null(userContextProvider, nameof(userContextProvider));
            _timeProvider = Guard.Against.Null(timeProvider, nameof(timeProvider));
        }

        protected override async Task Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var user = _userContextProvider.UserContext.User;
            var todoList = await _unitOfWork.GetOrCreateAsync(
                TodoListId.Define(user.UserId),
                () => TodoListAggregate.Create(user.UserId));

            todoList.AddTask(request.Id, request.Name, request.DeadlineUtc, _timeProvider.UtcNow, request.Description);

            await _unitOfWork.CommitAsync();
        }
    }
}