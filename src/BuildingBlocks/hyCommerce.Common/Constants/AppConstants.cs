namespace hyCommerce.Common.Constants;

public static class AppConstants
{
    public const string EMAIL_CONFIRMATION_URL = "{0}/api/account/confirm-email?userId={1}&token={2}";
    public const string RESET_PASSWORD_URL = "{0}/api/account/reset-password?userId={1}&token={2}";
}