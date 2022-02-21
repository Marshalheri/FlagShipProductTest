using System;

namespace FlagshipProductTest.Shared.DTOs.User
{
    public class UserLoginResponseDTO
    {
        public string Access_Token { get; set; }
        public DateTime ExpirationTime { get; set; }
        public UserDTO UserDetails { get; set; }
    }

    public class UserDTO : UserBaseDTO
    {
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
    }

    public class UserBaseDTO
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
