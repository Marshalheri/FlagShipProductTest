namespace FlagshipProductTest.Shared.DTOs.User
{
    public class UserLoginRequestDTO : BaseRequestValidatorDTO
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
