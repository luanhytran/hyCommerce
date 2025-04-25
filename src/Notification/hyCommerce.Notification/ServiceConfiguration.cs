using hyCommerce.Notification.Providers;
using hyCommerce.Notification.Providers.SendGrid;
using hyCommerce.Notification.Providers.Smtp;
using SendGrid.Extensions.DependencyInjection;

namespace hyCommerce.Notification
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddMailSender(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<SmtpOptions>().Bind(configuration.GetSection("MessageSender"));
            services.AddOptions<SendGridOptions>().Bind(configuration.GetSection("MessageSender"));
            services.AddOptions<EmailTemplateId>().Bind(configuration.GetSection("EmailTemplateIds"));

            var provider = configuration["MessageSender:Provider"] 
                ?? throw new Exception("The 'MessageSender:Provider' is not configured");

            if (provider == MessageSenderProviders.Smtp.ToString())
            {
                services.AddScoped<IEmailSender, SmtpSender>();
            }

            if (provider == MessageSenderProviders.Sendgrid.ToString())
            {
                services.AddSendGrid(options => options.ApiKey = configuration["MessageProvider:ApiKey"]
                    ?? throw new Exception("The 'MessageSender:ApiKey' is not configured"));

                services.AddScoped<IEmailSender, SendGridSender>();
            }

            return services;
        }
    }
}
