using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CurrencyTracker.Web.Controllers
{
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        public CurrencyController(ILogger<CurrencyController> logger, ICurrencyRepository currencyRepository, IWebHostEnvironment enviroment)
        {
            _currencyRepository = currencyRepository;
            _logger = logger;
            _enviroment = enviroment;
        }

        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<CurrencyController> _logger;
        private readonly IWebHostEnvironment _enviroment;

        [Authorize(Policy = "Bearer")]
        [HttpGet("currencies")]
        public async Task<ActionResult<List<Valute>>> GetCurrencies([FromQuery] string? valute = null, [FromQuery] string? date = null)
        {
            try
            {
                DateTime? parsedDate = null;

                if (!string.IsNullOrEmpty(date))
                {
                    if (DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                    {
                        parsedDate = result;
                    }
                    else
                    {
                        return BadRequest("Неверный формат даты. Используйте dd/MM/yyyy");
                    }
                }

                var valutes = await _currencyRepository.GetValutesWithCurrenciesAsync(valute, parsedDate);

                return Ok(valutes);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка запроса");
                return BadRequest("Ошибка запроса, пожалуйста проверьте корректность передаваемых данных");
            }
        }

        [Authorize(Policy = "Bearer")]
        [HttpGet("currency/{valuteName}")]
        public async Task<ActionResult<Currency>> GetCurrency(string valuteName)
        {
            try
            {
                var currency = await _currencyRepository.GetLatestCurrencyAsync(valuteName);
                return Ok(currency);
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarning($"Валюта '{valuteName}' не найдена");
                return NotFound($"Валюта '{valuteName}' не найдена");
            }
        }

        [Authorize(Policy = "Bearer")]
        [HttpGet("currency/qrcbr")]
        public IActionResult GetQrCode()
        {
            var filePath = Path.Combine(_enviroment.WebRootPath, "QR/index.html");

            return PhysicalFile(filePath, "text/html");
        }
    }
}
