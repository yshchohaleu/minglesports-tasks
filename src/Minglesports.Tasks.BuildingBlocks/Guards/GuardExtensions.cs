using System;
using Ardalis.GuardClauses;

namespace Minglesports.Tasks.BuildingBlocks.Guards
{
    public static class GuardExtensions
    {
        public static string Length(this IGuardClause clause, string value, int maxLength, string parameterName)
        {
            clause.Null(value, parameterName);

            if (value.Length > maxLength)
            {
                throw new ArgumentException($"Parameter '{parameterName}' should not exceed {maxLength} character");
            }

            return value;
        }

        public static DateTime? GreaterThan(this IGuardClause _, DateTime? date, DateTime? compareDate, string parameterName)
        {
            if (date != null && compareDate != null && date.Value.Date > compareDate.Value.Date)
            {
                throw new ArgumentException($"'{parameterName}' should be greater than '{compareDate.Value.Date}'");
            }

            return date;
        }
    }
}