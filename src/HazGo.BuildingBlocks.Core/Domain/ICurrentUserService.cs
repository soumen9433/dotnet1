namespace HazGo.BuildingBlocks.Core.Domain
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string ClientId { get; }
        string FirstName { get; }
        string LastName { get; }
    }
}
