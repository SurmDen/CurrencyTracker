using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTracker.Web.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository, ITokenService tokenService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("user/create")]
        public async Task<IActionResult> CreateUserAsync(CreateUserDTO createUserDTO)
        {
            try
            {
                await _userRepository.CreateUserAsync(createUserDTO);

                _logger.LogInformation($"Пользователь {createUserDTO.Email} успешно создан");
                return Ok(new { message = "Пользователь успешно создан" });
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Ошибка создания пользователя");
                return BadRequest(new {message = "Ошибка создания пользователя"});
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(SignInDTO signInDTO)
        {
            User? user = await _userRepository.GetUserByNameAndEmailAsync(signInDTO);

            if (user != null)
            {
                _logger.LogInformation($"Пользователь {signInDTO.Email} совершил вход");

                string token = _tokenService.GetToken(user);

                return Ok(new { token = token });
            }
            else
            {
                
                _logger.LogWarning($"Ошибка входа пользователя {signInDTO.Email}");
                return BadRequest(new { message = "Не удалось совершить вход, проверьте корректность данных" });
            }
        }
    }
}
