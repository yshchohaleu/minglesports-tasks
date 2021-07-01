using System;
using System.Collections.ObjectModel;
using System.Linq;
using Minglesports.Tasks.Core.OperationHandlers.Requests.Queries;

namespace Minglesports.Tasks.Web.Models
{
    public class GetTasksResponseModel : Collection<TaskResponseModel>
    {
        public static GetTasksResponseModel FromOperationModel(TodoListModel model)
        {
            var response = new GetTasksResponseModel();
            foreach (var task in model.Tasks)
            {
                response.Add(new TaskResponseModel
                (
                    task.EntityId,
                    task.Name,
                    task.Description,
                    task.Status.ToString(),
                    task.DeadlineUtc,
                    task.CreateAtUtc
                ));
            }

            return response;
        }
    }

    public record TaskResponseModel(
        string Id,
        string Status,
        string Name,
        string Description,
        DateTime DeadlineUtc,
        DateTime CreatedAtUtc
    );
}