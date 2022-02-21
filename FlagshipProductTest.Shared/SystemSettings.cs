using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared
{
    public class SystemSettings
    {
        public bool UseSwagger { get; set; }
        public bool UseMocks { get; set; }
        public string[] PropertiesToSanitize { get; set; }
        public UsernamePolicy UsernamePolicy { get; set; }
        public Jwt Jwt { get; set; }
        public int MaxLoginCount { get; set; }
    }

    public class UsernamePolicy
    {
        public string RegEx { get; set; }
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }
        public List<string> Message { get; set; }
    }
    public class Jwt
    {
        public string SecurityKey { get; set; }
        public int ExpirationMinutes { get; set; }
    }

}
