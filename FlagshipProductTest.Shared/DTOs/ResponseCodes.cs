
namespace FlagshipProductTest.Shared.DTOs
{
    public class ResponseCodes
    {
        public const string INVALID_INPUT_PARAMETER = "FS001";
        public const string INVALID_USERNAME_FORMAT = "FS002";
        public const string USERNAME_ALREADY_TAKEN = "FS003";
        public const string INVALID_DOB = "FS004";
        public const string NOT_BASE64 = "FS005";
        public const string INVALID_LOGIN_CREDENTIALS = "FS006";
        public const string ACCOUNT_LOCKED = "FS007";
        public const string ACCOUNT_DEACTIVATED = "FS008";
        public const string NO_CONTRIBUTION = "FS009";
        public const string NO_USER_FOUND = "FS010";

        public const string GENERAL_ERROR = "FS999";
    }
}
