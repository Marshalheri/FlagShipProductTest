using FlagshipProductTest.Shared.Models;
using FlagshipProductTest.Shared.Services;
using System;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared
{
    public interface ICryptoProvider
    {
        Task<string> GenerateSaltAsync();
        Task<string> GenerateHashAsync(string input, string salt);
        Task<bool> AreEqualAsync(string plainText, string hashedText, string salt);
        Task<JwtToken> GenerateJwtToken(User user);
    }
}
