using System.Collections.Generic;
using Ardalis.GuardClauses;
using Minglesports.Tasks.BuildingBlocks.Domain;
using Minglesports.Tasks.BuildingBlocks.UserContext;

namespace Minglesports.Tasks.Core.Domain.ValueObjects
{
    public class TodoListIdentifier : ValueObject
    {
        private const string Prefix = "todo";

        public string Value { get; private set; }
        public UserId UserId { get; private set; }

        private TodoListIdentifier()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static TodoListIdentifier Define(UserId userId)
        {
            Guard.Against.NullOrEmpty(userId, nameof(userId));
            return new TodoListIdentifier
            {
                UserId = userId,
                Value = $"{Prefix}|{userId}"
            };
        }

        public override string ToString() => Value;
    }
}