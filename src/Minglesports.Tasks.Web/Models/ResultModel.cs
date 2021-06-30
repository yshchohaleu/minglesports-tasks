using System.Collections.Generic;
using System.Linq;

namespace Minglesports.Tasks.Web.Models
{
    public record ResultModel
    {
        public List<ResultMessage> Errors { get; } = new();
        public bool Success => !Errors.Any();

        public void AddError(string code, string message)
        {
            Errors.Add(new ResultMessage(code, message));
        }
    }

    public record DataResultModel<T> (T Data) : ResultModel;

    public record ResultMessage (string Code, string Message);
}