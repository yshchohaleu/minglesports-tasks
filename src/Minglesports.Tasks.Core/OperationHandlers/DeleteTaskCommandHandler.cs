using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Commands;
using Minglesports.Tasks.Core.Ports;
using NotFoundException = Minglesports.Tasks.Core.Exceptions.NotFoundException;

namespace Minglesports.Tasks.Core.OperationHandlers
{
    internal class DeleteTaskCommandHandler : AsyncRequestHandler<DeleteTaskCommand>
    {
        private readonly ITodoListUnitOfWork _unitOfWork;
        private readonly IUserContextProvider _userContextProvider;

        public DeleteTaskCommandHandler(
            ITodoListUnitOfWork unitOfWork,
            IUserContextProvider userContextProvider)
        {
            _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
            _userContextProvider = Guard.Against.Null(userContextProvider, nameof(userContextProvider));
        }

        protected override async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var user = _userContextProvider.UserContext.User;
            var todoList = await _unitOfWork.GetAsync(TodoListId.Define(user.UserId));

            if (todoList == null)
                throw new NotFoundException($"Todo list for user {user.UserId} not found");

            todoList.DeleteTask(request.Id);

            await _unitOfWork.CommitAsync();
        }
    }
}