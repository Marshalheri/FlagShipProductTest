using FlagshipProductTest.Shared.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.Services
{
    public class CryptoProvider : ICryptoProvider
    {
        private const int HASH_LENGTH = 32, ITERATION_COUNT = 128;
        private readonly SystemSettings _settings;
        public CryptoProvider(IOptions<SystemSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<bool> AreEqualAsync(string plainText, string hashedText, string salt)
        {
            return await GenerateHashAsync(plainText, salt) == hashedText;
        }

        public async Task<string> GenerateHashAsync(string input, string salt)
        {
            var generator = new Rfc2898DeriveBytes(input, Convert.FromBase64String(salt), ITERATION_COUNT);
            var hash = generator.GetBytes(HASH_LENGTH);
            return Convert.ToBase64String(hash);
        }

        public async Task<string> GenerateSaltAsync()
        {
            var salt = new byte[HASH_LENGTH];
            var provider = new RNGCryptoServiceProvider();
            provider.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public async Task<JwtToken> GenerateJwtToken(User user)
        {
            var expirationTime = user.LastLogin.Value.AddMinutes(_settings.Jwt.ExpirationMinutes);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("Username", user.Username),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Email, user.EmailAddress)
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Jwt.SecurityKey)),
                    SecurityAlgorithms.HmacSha256
                )
            };
            var jwtToken = new JwtToken
            {
                Access_Token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                ExpirationTime = expirationTime
            };

            return jwtToken;
        }
    }
}
