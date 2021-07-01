using System;
using Minglesports.Tasks.Core.Domain.ValueObjects;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Commands;
using Minglesports.Tasks.Web.Models.Result;

namespace Minglesports.Tasks.Web.Models
{
    public class AddTaskRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DeadlineUtc { get; set; }

        public ResultModel TryConvertToCommand(out AddTaskCommand command)
        {
            var result = new ResultModel();
            result
                .IsNullOrEmpty(Name, nameof(Name))
                .MaxLength(Name, 100, nameof(Name));

            command = new AddTaskCommand(TaskId.New(), TaskName.Define(Name), DeadlineUtc, Description);

            return result;
        }
    }
}