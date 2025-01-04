using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidOperationException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<ActionResult<RegisterResponse>> Register
            ([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var account =
                await _authService.Register(request.Email, request.Password, cancellationToken);

            return Ok(new RegisterResponse(account.Id, account.Email.ToString()));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<ActionResult<LoginResponse>> LoginByPassword([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var account = await _authService.LoginByPassword(request.Email, request.Password, cancellationToken);
            var token = _tokenService.GenerateToken(account);
            return Ok(new LoginResponse(account.Id, account.Email.ToString(), token));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeleteAccount([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            //Транзакция
            await _authService.DeleteAccountAsync(id, cancellationToken);
            //await _userProfileClient.DeleteUserProfileByAccountIdAsync(id, cancellationToken);
            //TODO: отправляем запрос на удаление профиля животного и передаем туда userProfileId
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PasswordNotChangedException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("[action]")]
        public async Task UpdatePassword([FromBody] UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            await _authService.UpdatePasswordAsync
                (request.AccountId, request.OldPassword, request.NewPassword, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPasswordException))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(AccountNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("[action]")]
        public async Task ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            await _authService.ResetPasswordAsync(request.Email, request.NewPassword, cancellationToken);
        }
    }
}
