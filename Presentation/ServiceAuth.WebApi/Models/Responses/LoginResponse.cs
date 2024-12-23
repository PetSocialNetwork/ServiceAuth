namespace ServiceAuth.WebApi.Models.Responses
{
    public record LoginResponse(Guid Id, string Email, string Token);
}
