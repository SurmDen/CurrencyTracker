using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Domain.Entites
{
    public class Valute : BaseEntity
    {
        public string ValuteName { get; set; } = string.Empty;

        public List<Currency> Currencies { get; set; }
    }
}
