namespace hyCommerce.Notification.Providers
{
    public class EmailRequest<T>
    {
        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsHtml { get; set; }

        public string TemplateId { get; set; }

        public T TemplateData { get; set; }
    }
}
