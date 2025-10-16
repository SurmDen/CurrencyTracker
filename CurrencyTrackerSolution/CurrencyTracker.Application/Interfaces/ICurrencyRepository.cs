using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Application.Interfaces
{
    public interface ICurrencyRepository
    {
        // Получаем валюты с курсами (с возможностью фильтрации по дате и названию валюты)
        public Task<List<Valute>> GetValutesWithCurrenciesAsync(string? valuteName = null, DateTime? date = null);

        // Получаем последний курс валюты
        public Task<Currency> GetLatestCurrencyAsync(string valuteName);

        // Загружаем валюты с курсами из API цунтробанка в БД
        public Task LoadValutesWithCurrenciesAsync(List<ValCursDTO> valutes);
    }
}
