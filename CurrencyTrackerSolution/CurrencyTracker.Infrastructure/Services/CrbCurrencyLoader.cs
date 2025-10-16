using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
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

        public CrbCurrencyLoader(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        // загружаем данные за daysCount дней начиная с сегодняшнего
        public async Task LoadCurrenciesAndSaveAsync(int daysCount)
        {
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
                        var xmlContent = await response.Content.ReadAsStringAsync();
                        var serializer = new XmlSerializer(typeof(ValCursDTO));
                        using var reader = new StringReader(xmlContent);

                        if (serializer.Deserialize(reader) is ValCursDTO valCurs)
                        {
                            valCursList.Add(valCurs);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка загрузки для даты {dateStr}: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке данных за {dateStr}: {ex.Message}");
                }

                await Task.Delay(100);
            }

            // var allValutes = valCursList
            //    .SelectMany(vc => vc.Valutes)
            //    .ToList();

            // Сохраняем загруженные данные в БД
            await _currencyRepository.LoadValutesWithCurrenciesAsync(valCursList);
        }
    }
}
