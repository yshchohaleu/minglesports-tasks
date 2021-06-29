using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Minglesports.Tasks.BuildingBlocks.Domain;
using Minglesports.Tasks.BuildingBlocks.UserContext;

namespace Minglesports.Tasks.Core.Domain.ValueObjects
{
    public class TodoListId : ValueObject
    {
        private string _value;
        public string Value
        {
            get => _value;
            private set
            {
                var userId = UserId.Define(value.Split('|').Last());
                UserId = userId;
                _value = value;
            }
        }
        public UserId UserId { get; private set; }

        private TodoListId()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static TodoListId Define(UserId userId)
        {
            Guard.Against.NullOrEmpty(userId, nameof(userId));
            return new TodoListId
            {
                Value = $"todo|{userId}"
            };
        }

        public override string ToString() => _value;
    }
}