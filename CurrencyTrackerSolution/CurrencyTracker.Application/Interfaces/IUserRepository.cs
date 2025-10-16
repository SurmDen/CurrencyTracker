using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Application.Interfaces
{
    public interface IUserRepository
    {
        public Task CreateUserAsync(CreateUserDTO createUserDTO);

        public Task<User> GetUserByNameAndEmailAsync(SignInDTO signInDTO);
    }
}
