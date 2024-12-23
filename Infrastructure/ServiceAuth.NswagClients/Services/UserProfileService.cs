using PetSocialNetwork.ServiceUser;
using ServiceAuth.Domain.Interfaces;

namespace ServiceAuth.NswagClients.Services
{

    public class UserProfileService : IUserProfileService
    {

        private readonly IUserProfileApiClient _userProfileClient;

        public UserProfileService(IUserProfileApiClient userProfileClient)
        {
            _userProfileClient = userProfileClient;
        }

        public async Task<Guid> AddUserProfileAsync(Guid accountId, CancellationToken cancellationToken)
        {
            var response = await _userProfileClient.AddUserProfileAsync(new AddUserProfileRequest() { AccountId = accountId }, cancellationToken);
            return response.Id;
        }
    }


}
