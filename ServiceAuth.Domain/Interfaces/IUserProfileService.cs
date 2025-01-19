namespace ServiceAuth.Domain.Interfaces
{
    public interface IUserProfileService
    {
        Task<Guid> AddUserProfileAsync(Guid accountId, CancellationToken cancellationToken);
        Task DeleteUserProfileAsync(Guid accountId, CancellationToken cancellationToken);
    }
}
