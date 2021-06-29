namespace Minglesports.Tasks.BuildingBlocks.UserContext
{
    public interface IUserContextProvider
    {
        CurrentUserContext UserContext { get; }
    }
}
