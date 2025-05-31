using InventoryBackend.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace InventoryBackend.Service
{
    public class tokenProvider(IConfiguration configuration)
    {
        public string Create(userAccounts user)
        {
            string secretKey = configuration["jwt:Secret"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                    new Claim(JwtRegisteredClaimNames.Sub, user.userID.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.userName),
                ]),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["jwt:Issuer"],
                Audience = configuration["jwt:Audience"]
            };
            var handler = new JsonWebTokenHandler();
            string token = handler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
