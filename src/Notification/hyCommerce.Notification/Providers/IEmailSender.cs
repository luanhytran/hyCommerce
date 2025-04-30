namespace hyCommerce.Notification.Providers
{
    public interface IEmailSender
    {
        Task SendEmailAsync<T>(EmailRequest<T> request);
    }
}
