using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DTOs.Admin
{
    public class AdminLoginRequestDTO : BaseRequestValidatorDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public override bool IsValid(out string problemSource)
        {
            problemSource = string.Empty;
            if (string.IsNullOrEmpty(Username))
            {
                problemSource = "Username";
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                problemSource = "Password";
                return false;
            }
            return true;
        }
    }
}
