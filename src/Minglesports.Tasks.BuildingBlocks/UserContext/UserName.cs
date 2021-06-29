using Ardalis.GuardClauses;
using Minglesports.Tasks.BuildingBlocks.Domain;

namespace Minglesports.Tasks.BuildingBlocks.UserContext
{
    public class UserId : SingleValueObject<string>
    {
        private UserId(string value) : base(value)
        {
        }

        public static UserId Define(string value)
        {
            var userName = Guard.Against.NullOrWhiteSpace(value, nameof(value));
            return new(userName);
        }
    }
}
