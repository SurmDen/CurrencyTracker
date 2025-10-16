using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Application.Interfaces
{
    public interface ICurrencyLoader
    {
        // Загружаем курсы валют и сохраняем в БД
        public Task LoadCurrenciesAndSaveAsync(int daysCount);
    }
}
