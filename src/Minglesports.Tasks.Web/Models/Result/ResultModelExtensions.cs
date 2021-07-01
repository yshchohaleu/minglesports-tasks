using System;
using System.Linq;
using Minglesports.Tasks.Web.Services;

namespace Minglesports.Tasks.Web.Models.Result
{
    public static class ResultModelExtensions
    {
        public static ResultModel IsNullOrEmpty(this ResultModel model, string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                model.AddError(ApiConstants.ErrorCodes.BadRequest, $"{propertyName} cannot be empty");
            }

            return model;
        }

        public static ResultModel ValidEnum<T>(this ResultModel model, string val, string propertyName)
            where T : struct
        {
            if (!Enum.TryParse(val, true, out T _))
            {
                model.AddError(ApiConstants.ErrorCodes.BadRequest,
                    $"{propertyName} should be [{ string.Join(", ", Enum.GetValues(typeof(T)).Cast<T>())}]");
            }

            return model;
        }

        public static ResultModel MaxLength(this ResultModel model, string val, int maxLength, string propertyName)
        {
            if (string.IsNullOrEmpty(val))
                return model;

            if (val.Length > maxLength)
            {
                model.AddError(ApiConstants.ErrorCodes.BadRequest, $"{propertyName} cannot exceed {maxLength} characters");
            }

            return model;
        }

    }
}