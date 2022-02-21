using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.Services
{
    public class JwtToken
    {
        public string Access_Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
