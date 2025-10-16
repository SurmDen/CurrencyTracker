using CurrencyTracker.Application.DTOs;
using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entites;
using CurrencyTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateUserAsync(CreateUserDTO createUserDTO)
        {
            User? existsUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == createUserDTO.Email);

            if (existsUser != null)
            {
                throw new InvalidOperationException($"User with email {createUserDTO.Email} already exists");
            }

            existsUser = await _context.Users.
                FirstOrDefaultAsync(u => u.Password == createUserDTO.Password);

            if (existsUser != null)
            {
                throw new InvalidOperationException("User with entered password already exists");
            }

            await _context.Users.AddAsync(new User()
            {
                Email = createUserDTO.Email,
                FirstName = createUserDTO.FirstName,
                LastName = createUserDTO.LastName,
                Password = createUserDTO.Password
            });

            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByNameAndEmailAsync(SignInDTO signInDTO)
        {
            User? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == signInDTO.Email && u.Password == signInDTO.Password);

            if (user == null)
            {
                throw new InvalidOperationException("User with sign in parameters not found");
            }

            return user;
        }
    }
}
