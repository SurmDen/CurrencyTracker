using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entites;
using CurrencyTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        public CurrencyRepository(ApplicationDbContext context)
        {
            // Получаем контекст БД из DI
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        // Далее реализуем интерфейс согласно ТЗ, подробности той или иной реализации расскажу на собеседовании (там все просто)

        public async Task<List<Valute>> GetValutesWithCurrenciesAsync(string? valuteName = null, DateTime? date = null)
        {
            var query = _context.Valutes
                .Include(x => x.Currencies)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(valuteName))
            {
                query = query.Where(v => v.ValuteName == valuteName);
            }

            if (date.HasValue)
            {
                query = query.Where(v => v.Currencies.Any(c => c.Date == date.Value));
            }

            var valutes = await query.ToListAsync();

            foreach (var valute in valutes)
            {
                if (date.HasValue && valute.Currencies.Any())
                {
                    valute.Currencies = valute.Currencies
                        .Where(c => c.Date == date.Value)
                        .ToList();
                }

                foreach (var currency in valute.Currencies)
                {
                    currency.Valute = null;
                }
            }

            return valutes;
        }

        public async Task<Currency> GetLatestCurrencyAsync(string valuteName)
        {
            valuteName = valuteName.Trim().ToUpper();

            Currency? currency = await _context.Currencies
                .Include(c => c.Valute)
                .AsNoTracking()
                .OrderByDescending(c => c.Date)
                .FirstOrDefaultAsync(c => c.Valute.ValuteName == valuteName);

            if (currency == null)
            {
                throw new InvalidOperationException($"Currency with valute name: {valuteName} was null");
            }

            return currency;
        }

        public async Task LoadValutesWithCurrenciesAsync(List<ValCursDTO> valCursList)
        {
            foreach (var valCurs in valCursList)
            {
                var date = DateTime.ParseExact(valCurs.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                foreach (var valuteDto in valCurs.Valutes)
                {
                    var valute = await _context.Valutes
                        .FirstOrDefaultAsync(v => v.ValuteName == valuteDto.Name);

                    if (valute == null)
                    {
                        valute = new Valute
                        {
                            ValuteName = valuteDto.Name
                        };
                        _context.Valutes.Add(valute);
                    }

                    var existingCurrency = await _context.Currencies
                        .Include(c => c.Valute)
                        .FirstOrDefaultAsync(c =>
                            c.Valute.ValuteName == valuteDto.Name &&
                            c.Date.Date == date.Date);

                    if (existingCurrency != null)
                    {
                        existingCurrency.Value = valuteDto.Value;
                        existingCurrency.Nominal = valuteDto.Nominal;
                        _context.Currencies.Update(existingCurrency);
                    }
                    else
                    {
                        var currency = new Currency
                        {
                            Value = valuteDto.Value,
                            Nominal = valuteDto.Nominal,
                            Date = date,
                            Valute = valute
                        };
                        _context.Currencies.Add(currency);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
