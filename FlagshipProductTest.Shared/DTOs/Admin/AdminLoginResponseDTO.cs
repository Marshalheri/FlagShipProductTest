using FlagshipProductTest.Shared.DTOs.User;
using System;

namespace FlagshipProductTest.Shared.DTOs.Admin
{
    public class AdminLoginResponseDTO
    {
        public string Access_Token { get; set; }
        public DateTime ExpirationTime { get; set; }
        public UserBaseDTO AdminUserDetails { get; set; }
    }
}
