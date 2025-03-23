using Microsoft.AspNetCore.Mvc;
using ServiceAuth.Domain.Services;
using ServiceAuth.WebApi.Models.Requests;
using ServiceAuth.WebApi.Models.Responses;
using ServiceAuth.WebApi.Services;

namespace ServiceAuth.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(AuthService authService,
            ITokenService tokenService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<RegisterResponse>> Register
            ([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var account =
                await _authService.Register(request.Email, request.Password, cancellationToken);

            return Ok(new RegisterResponse(account.Id, account.Email.ToString()));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<LoginResponse>> LoginByPassword([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var account = await _authService.LoginByPassword(request.Email, request.Password, cancellationToken);
            var token = _tokenService.GenerateToken(account);
            return Ok(new LoginResponse(account.Id, account.Email.ToString(), token));
        }

        [HttpDelete("[action]")]
        public async Task DeleteAccount([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _authService.DeleteAccountAsync(id, cancellationToken);
        }

        [HttpPut("[action]")]
        public async Task UpdatePassword([FromBody] UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            await _authService.UpdatePasswordAsync
                (request.AccountId, request.OldPassword, request.NewPassword, cancellationToken);
        }

        [HttpPut("[action]")]
        public async Task ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            await _authService.ResetPasswordAsync(request.Email, request.NewPassword, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<bool> IsRegisterUserAsync([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await _authService.IsRegisterUserAsync(request.Email, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<bool> IsTheSameUserPasswordAsync([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await _authService.IsTheSameUserPasswordAsync(request.Email, request.NewPassword, cancellationToken);
        }
    }
}
