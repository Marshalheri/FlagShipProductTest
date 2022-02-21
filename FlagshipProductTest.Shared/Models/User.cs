using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Title Title { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int FailedLoginCount { get; set; }
        public ProfileStatus ProfileStatus { get; set; }
        public bool IsLocked(int count)
        {
            return FailedLoginCount >= count;
        }
    }

    public enum ProfileStatus
    {
        Inactive,
        Active
    }

    public enum Gender
    {
        Male = 1, Female
    }

    public enum Title
    {
        Mr = 1, Mrs, Miss
    }
}
