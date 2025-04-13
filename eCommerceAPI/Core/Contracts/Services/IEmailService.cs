namespace eCommerceAPI.Core.Contracts.Services;

public interface IEmailService
{
    void SendEmail(string to, string subject, string body, bool isHtml);
}