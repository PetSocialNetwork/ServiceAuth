#pragma warning disable CS8618 

namespace ServiceAuth.WebApi.Models.Requests
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
