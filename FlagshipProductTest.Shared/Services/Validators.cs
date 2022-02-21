using Microsoft.Extensions.Options;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.Services
{
    public class Validators : IValidators
    {
        readonly SystemSettings _settings;
        public Validators(IOptions<SystemSettings> settingsProvider)
        {
            _settings = settingsProvider.Value;
        }
        public async Task<bool> IsUsernameValidAsync(string userName)
        {
            var pattern = _settings.UsernamePolicy.RegEx;
            var regex = new Regex(pattern, RegexOptions.None);
            if (!regex.IsMatch(userName) || userName.Length < _settings.UsernamePolicy.MinimumLength ||
                userName.Length > _settings.UsernamePolicy.MaximumLength)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> IsValidDobAsync(DateTime DOB)
        {
            var currentYear = DateTime.Now.Year;
            var DOBYear = DOB.Year;
            return DOBYear > currentYear;
        }
    }
}
