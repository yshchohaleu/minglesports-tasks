using Minglesports.Tasks.BuildingBlocks.UserContext;

namespace Minglesports.Tasks.Web.Services
{
    public class UserContextProvider : IUserContextProvider
    {
        public CurrentUserContext UserContext { get; private set; }

        public void Initialize(CurrentUserContext userContext)
        {
            UserContext = userContext;
        }
    }
}