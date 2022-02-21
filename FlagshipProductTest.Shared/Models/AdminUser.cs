using System;

namespace FlagshipProductTest.Shared.Models
{
    public class AdminUser : BaseModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public Gender Gender { get; set; }
        public Title Title { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int FailedLoginCount { get; set; }
        public ProfileStatus ProfileStatus { get; set; }
        public bool IsLocked(int count)
        {
            return FailedLoginCount >= count;
        }
    }
}
