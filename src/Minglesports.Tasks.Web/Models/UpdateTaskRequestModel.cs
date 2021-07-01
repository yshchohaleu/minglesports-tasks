using System;
using Minglesports.Tasks.Core.Domain;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Commands;
using Minglesports.Tasks.Web.Models.Result;

namespace Minglesports.Tasks.Web.Models
{
    public class UpdateTaskRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DeadlineUtc { get; set; }
        public string Status { get; set; }

        public ResultModel TryConvertToCommand(string id, out UpdateTaskCommand command)
        {
            var result = new ResultModel();
            result
                .IsNullOrEmpty(Name, nameof(Name))
                .MaxLength(Name, 100, nameof(Name))
                .ValidEnum<TaskStatus>(Status, nameof(Status));

            var taskId = !string.IsNullOrEmpty(id) ? TaskId.FromString(id) : TaskId.New();
            command = new UpdateTaskCommand(taskId, TaskName.Define(Name), DeadlineUtc, Description,
                Enum.Parse<TaskStatus>(Status, true));

            return result;
        }
    }
}