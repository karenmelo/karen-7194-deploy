using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shop.Models;

namespace Shop.Services
{
    public static class TokenSevice
    {
        public static string GenerateToken(User user)
        {
            var tokerHandler = new JwtSecurityTokenHandler(); //responsável por gerar o token de fato
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Id.ToString()), new Claim(ClaimTypes.Role, user.Role.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokerHandler.CreateToken(tokenDescriptor);
            return tokerHandler.WriteToken(token);
        }
    }

}