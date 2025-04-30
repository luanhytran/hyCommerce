namespace hyCommerce.Notification.Providers.Smtp
{
    public class SmtpOptions : BaseOptions
    {
        public string SenderFrom { get; set; }

        public string SenderSmtpClient { get; set; }

        public string SenderPassword { get; set; }
    }
}
