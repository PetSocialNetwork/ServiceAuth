using PetSocialNetwork.ServicePet;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.NswagClients.Services
{
    public class PetProfileService : IPetProfileService
    {
        private readonly IPetProfileClient _petProfileClient;

        public PetProfileService(IPetProfileClient petProfileClient)
        {
            _petProfileClient = petProfileClient;
        }

        public async Task DeletePetProfilesAsync(Guid accountId, CancellationToken cancellationToken)
        {
            await _petProfileClient.DeleteAllPetProfilesByAccountIdAsync(accountId, cancellationToken);
        }
    }
}
