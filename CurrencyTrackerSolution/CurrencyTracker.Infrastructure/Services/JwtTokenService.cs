using CurrencyTracker.Application.Interfaces;
using CurrencyTracker.Domain.Entites;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTracker.Infrastructure.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Name, user.FirstName),
                 new Claim(ClaimTypes.Surname, user.LastName)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var credentials = new SigningCredentials(secretKey, "HS256");

            var tokenOptions = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }
    }
}
