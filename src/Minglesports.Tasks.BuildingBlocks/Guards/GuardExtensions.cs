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
    }
}