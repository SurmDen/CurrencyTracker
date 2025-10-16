using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        public Task<List<Valute>> GetValutesWithCurrenciesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Currency> GetValuteWithLatestCurrencyAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task LoadValutesWithCurrenciesAsync(List<ValCursDTO> valutes)
        {
            throw new NotImplementedException();
        }
    }
}
