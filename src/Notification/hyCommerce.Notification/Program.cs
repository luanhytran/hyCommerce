using hyCommerce.Notification;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddMailSender(configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
