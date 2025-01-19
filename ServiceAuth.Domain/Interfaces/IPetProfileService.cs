namespace ServiceAuth.Domain.Interfaces
{
    public interface IPetProfileService
    {
        Task DeletePetProfilesAsync(Guid accountId, CancellationToken cancellationToken);
    }
}
