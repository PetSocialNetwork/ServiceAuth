#pragma warning disable CS8618 

namespace ServiceAuth.WebApi.Models.Requests
{
    public class UpdatePasswordRequest
    {
        public Guid AccountId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
