using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Domain.Entites
{
    public class Currency : BaseEntity
    {
        public decimal Value { get; set; }

        public int Nominal { get; set; }

        public DateTime Date { get; set; }

        public Valute Valute { get; set; }

        public long ValuteId { get; set; }
    }
}
