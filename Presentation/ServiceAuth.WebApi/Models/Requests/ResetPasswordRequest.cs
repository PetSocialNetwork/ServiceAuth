#pragma warning disable CS8618 

namespace ServiceAuth.WebApi.Models.Requests
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
