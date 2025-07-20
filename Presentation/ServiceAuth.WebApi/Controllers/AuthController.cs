using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceAuth.Domain.Entities;
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
        private readonly IMapper _mapper;

        public AuthController(AuthService authService,
            ITokenService tokenService,
            IMapper mapper)
        {
            _authService = authService 
                ?? throw new ArgumentNullException(nameof(authService));
            _tokenService = tokenService 
                ?? throw new ArgumentNullException(nameof(tokenService));
            _mapper = mapper 
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Идентификатор отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RegisterResponse>> Register
            ([FromBody] RegisterRequest request, 
              CancellationToken cancellationToken)
        {
            var account = _mapper.Map<Account>(request);
            var response = await _authService.Register(account, cancellationToken);
            return _mapper.Map<RegisterResponse>(response);
        }

        /// <summary>
        /// Вход по паролю
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Идентификатор отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LoginResponse>> LoginByPassword([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var account = _mapper.Map<Account>(request);
            var response = await _authService.LoginByPassword(account, cancellationToken);
            var token = _tokenService.GenerateToken(response);
            return new LoginResponse(response.Id, response.Email.ToString(), token);
        }

        /// <summary>
        /// Обновляет пароль
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Идентификатор отмены</param>
        [HttpPut("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task UpdatePassword([FromBody] UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            await _authService.UpdatePasswordAsync
                (request.AccountId, request.OldPassword, request.NewPassword, cancellationToken);
        }

        /// <summary>
        /// Сбрасывает пароль
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Идентификатор отмены</param>
        [HttpPut("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var account = _mapper.Map<Account>(request);
            await _authService.ResetPasswordAsync(account, cancellationToken);
        }

        /// <summary>
        /// Проверяет, зарегистрирован ли пользователь
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Идентификатор отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<bool> IsRegisterUserAsync([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            return await _authService.IsRegisterUserAsync(request.Email, cancellationToken);
        }

        /// <summary>
        /// Проверяет, является ли пароль тем же самым
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Идентификатор отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<bool> IsTheSameUserPasswordAsync([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var account = _mapper.Map<Account>(request);
            return await _authService.IsTheSameUserPasswordAsync(account, cancellationToken);
        }

        /// <summary>
        /// Удаляет аккаунт по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор аккаунта</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeleteAccount([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await _authService.DeleteAccountAsync(id, cancellationToken);
        }
    }
}
