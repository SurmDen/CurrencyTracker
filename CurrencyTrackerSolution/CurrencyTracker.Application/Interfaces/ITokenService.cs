using CurrencyTracker.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Application.Interfaces
{
    public interface ITokenService
    {
        public string GetToken(User user);
    }
}
