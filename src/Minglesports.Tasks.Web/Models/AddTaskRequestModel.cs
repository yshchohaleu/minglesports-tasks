using System;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Commands;

namespace Minglesports.Tasks.Web.Models
{
    public class AddTaskRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DeadlineUtc { get; set; }

        public ResultModel TryConvertToCommand(string id, out AddTaskCommand command)
        {
            var result = new ResultModel();
            result
                .IsNullOrEmpty(Name, nameof(Name))
                .MaxLength(Name, 100, nameof(Name))
                .IsGuid(id, nameof(id));

            var taskId = !string.IsNullOrEmpty(id) ? TaskId.FromString(id) : TaskId.New();
            command = new AddTaskCommand(taskId, TaskName.Define(Name), DeadlineUtc, Description);

            return result;
        }
    }
}