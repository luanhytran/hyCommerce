using hyCommerce.Application;
using hyCommerce.Infrastructure.Persistence;
using hyCommerce.Infrastructure.Persistence.Data;
using hyCommerce.Notification;
using Microsoft.EntityFrameworkCore;
using hyCommerce.EventBus;
using hyCommerce.Extensions.Handlers.ErrorHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentity<AppDbContext>(builder.Configuration, builder.Environment);

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddMailSender(builder.Configuration);

builder.Services.AddCap<AppDbContext>(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetService<AppDbContext>();
var userManager = scope.ServiceProvider.GetService<ApplicationUserManager>();

await DbInitializer.Initialize(context, userManager);

app.Run();