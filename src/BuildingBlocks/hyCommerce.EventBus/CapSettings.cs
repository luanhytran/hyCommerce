namespace hyCommerce.EventBus;

public class CapSettings
{
    public const string Name = "CapSettings";
    public string Provider { get; set; } = "RabbitMq";
    public string ConnectionString { get; set; }
    public string Host { get; set; } = "localhost";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
}

public enum Database
{
    Postgres,
    MSSQL
}