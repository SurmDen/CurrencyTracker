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
        public Task<List<Valute>> GetValutesWithCurrenciesAsync();

        public Task<Currency> GetValuteWithLatestCurrencyAsync(long id);

        public Task LoadValutesWithCurrenciesAsync(List<ValCursDTO> valutes);
    }
}
