using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.OperationHandlers.Requests;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Commands;
using Minglesports.Tasks.Core.Ports;
using NotFoundException = Minglesports.Tasks.Core.Exceptions.NotFoundException;

namespace Minglesports.Tasks.Core.OperationHandlers
{
    internal class UpdateTaskCommandHandler : AsyncRequestHandler<UpdateTaskCommand>
    {
        private readonly ITodoListUnitOfWork _unitOfWork;
        private readonly IUserContextProvider _userContextProvider;

        public UpdateTaskCommandHandler(
            ITodoListUnitOfWork unitOfWork,
            IUserContextProvider userContextProvider)
        {
            _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
            _userContextProvider = Guard.Against.Null(userContextProvider, nameof(userContextProvider));
        }

        protected override async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var user = _userContextProvider.UserContext.User;
            var todoList = await _unitOfWork.GetAsync(TodoListId.Define(user.UserId));

            if (todoList == null)
                throw new NotFoundException($"Todo list for user {user.UserId} not found");

            todoList.UpdateTask(request.Id, request.Name, request.DeadlineUtc, request.Description);

            await _unitOfWork.CommitAsync();
        }
    }
}