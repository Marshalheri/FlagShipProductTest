using FlagshipProductTest.Shared.DTOs.Document;
using FlagshipProductTest.Shared.DTOs.Password;
using FlagshipProductTest.Shared.Models;
using System;
using System.Text.RegularExpressions;

namespace FlagshipProductTest.Shared.DTOs.User
{
    public class UserOnboardingRequestDTO : BaseRequestValidatorDTO
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Title Title { get; set; }
        public string Address { get; set; }
        public NewPasswordDTO NewPassword { get; set; }
        public DocumentDTO Passport { get; set; }

        public override bool IsValid(out string sourceModel)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                sourceModel = "PhoneNumber";
                return false;
            }
            if (LastName?.Length > 50 || LastName?.Length < 2)
            {
                sourceModel = "LastName";
                return false;
            }
            if (FirstName?.Length > 50 || FirstName?.Length < 2)
            {
                sourceModel = "FirstName";
                return false;
            }
            if (string.IsNullOrEmpty(EmailAddress) || !IsEmailPatternMatched(EmailAddress))
            {
                sourceModel = "Email Address";
                return false;
            }
            if (string.IsNullOrEmpty(Username))
            {
                sourceModel = "Username";
                return false;
            }
            if (!NewPassword.IsValid(out sourceModel))
            {
                return false;
            }
            if (!Passport.IsValid(out sourceModel))
            {
                return false;
            }
            if (!Enum.IsDefined(typeof(Gender), Gender))
            {
                sourceModel = "Gender";
                return false;
            }
            if (!Enum.IsDefined(typeof(Title), Title))
            {
                sourceModel = "Title";
                return false;
            }
            return true;
        }

        private bool IsEmailPatternMatched(string email)
        {
            var pattern = @"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[,]{0,1}\s*)+$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
    }
}
