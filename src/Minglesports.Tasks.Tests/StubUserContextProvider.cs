using AutoFixture;
using Minglesports.Tasks.BuildingBlocks.UserContext;

namespace Minglesports.Tasks.Tests
{
    public class StubUserContextProvider : IUserContextProvider
    {
        public CurrentUserContext UserContext { get; set; }

        public static StubUserContextProvider Random()
        {
            var fixture = new Fixture()
                .Customize(new TodoListDomainCustomization());

            return new StubUserContextProvider
            {
                UserContext = new CurrentUserContext(new User(fixture.Create<UserId>()))
            };
        }

    }
}