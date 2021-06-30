using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Queries;
using Minglesports.Tasks.Core.Ports;

namespace Minglesports.Tasks.Application.OperationHandlers
{
    internal class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, TodoListModel>
    {
        private readonly ITodoListUnitOfWork _unitOfWork;
        private readonly IUserContextProvider _userContextProvider;

        public GetTodoListQueryHandler(
            ITodoListUnitOfWork unitOfWork,
            IUserContextProvider userContextProvider)
        {
            _unitOfWork = Guard.Against.Null(unitOfWork, nameof(unitOfWork));
            _userContextProvider = Guard.Against.Null(userContextProvider, nameof(userContextProvider));
        }

        public async Task<TodoListModel> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
        {
            var user = _userContextProvider.UserContext.User;

            var todoList = await _unitOfWork.GetOrCreateAsync(
                TodoListIdentifier.Define(user.UserId),
                () => TodoListAggregate.Create(user.UserId));

            return TodoListModel.FromDomain(todoList);
        }
    }
}