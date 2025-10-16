using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CurrencyTracker.Infrastructure.Services
{
    public class CrbCurrencyLoader : ICurrencyLoader
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<CrbCurrencyLoader> _logger;

        public CrbCurrencyLoader(ICurrencyRepository currencyRepository, ILogger<CrbCurrencyLoader> logger)
        {
            _currencyRepository = currencyRepository;
            _logger = logger;
        }

        // загружаем данные за daysCount дней начиная с сегодняшнего
        public async Task LoadCurrenciesAndSaveAsync(int daysCount)
        {
            // Регистрируем провайдер кодировок
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            List<ValCursDTO> valCursList = new List<ValCursDTO>();
            HttpClient httpClient = new HttpClient();

            for (int i = 0; i < daysCount; i++)
            {
                DateTime targetDate = DateTime.Now.AddDays(-i);
                string dateStr = targetDate.ToString("dd/MM/yyyy");

                try
                {
                    var url = $"http://www.cbr.ru/scripts/XML_daily.asp?date_req={dateStr}";
                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var byteArray = await response.Content.ReadAsByteArrayAsync();
                        var xmlContent = Encoding.GetEncoding("windows-1251").GetString(byteArray);

                        xmlContent = xmlContent.Replace(",", ".");

                        var serializer = new XmlSerializer(typeof(ValCursDTO));
                        using var reader = new StringReader(xmlContent);

                        if (serializer.Deserialize(reader) is ValCursDTO valCurs)
                        {
                            valCursList.Add(valCurs);
                        }
                    }
                    else
                    {
                        _logger.LogError($"Ошибка загрузки для даты {dateStr}: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ошибка при загрузке данных за {dateStr}: {ex.Message}");
                }

                await Task.Delay(100);
            }

            if (valCursList.Any())
            {
                await _currencyRepository.LoadValutesWithCurrenciesAsync(valCursList);
                _logger.LogInformation($"Сохранено: {valCursList.Count} дней, {valCursList.Sum(v => v.Valutes.Count)} курсов");
            }
            else
            {
                _logger.LogInformation("Не загружено ни одного дня данных");
            }
        }
    }
}
