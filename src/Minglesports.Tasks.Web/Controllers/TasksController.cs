using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Commands;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Queries;
using Minglesports.Tasks.Web.Models;
using Minglesports.Tasks.Web.Services;

namespace Minglesports.Tasks.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = Guard.Against.Null(mediator, nameof(mediator));
        }

        [HttpGet]
        public async Task<DataResultModel<TodoListModel>> GetAsync()
        {
            var todoList = await _mediator.Send(new GetTodoListQuery());
            return new DataResultModel<TodoListModel>(todoList);
        }

        [HttpPost]
        public async Task<ActionResult<ResultModel>> AddTaskAsync(
            [FromHeader(Name = ApiConstants.HttpHeaders.TaskIdTransactionHeader)] string id,
            [FromBody] AddTaskRequestModel request)
        {
            var result = request.TryConvertToCommand(id, out var command);
            if (!result.Success)
                return BadRequest(result);

            await _mediator.Send(command);
            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ResultModel>> UpdateTaskAsync([FromRoute] string id,
            [FromBody] UpdateTaskRequestModel request)
        {
            var result = request.TryConvertToCommand(id, out var command);
            if (!result.Success)
                return BadRequest(result);

            await _mediator.Send(command);
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ResultModel>> DeleteTaskAsync([FromRoute] string id)
        {
            var result = new ResultModel();
            result.IsGuid(id, nameof(id));

            if (!result.Success)
                return BadRequest(result);

            await _mediator.Send(new DeleteTaskCommand(TaskId.FromString(id)));
            return Ok(result);
        }
    }
}