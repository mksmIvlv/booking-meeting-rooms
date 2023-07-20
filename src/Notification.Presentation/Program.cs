using Notification.Application.Extensions;
using Notification.Application.Services;
using Notification.Infrastructure.Extensions;
using Notification.Infrastructure.Settings;
using Notification.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Получение экземпляра класса
var infrastructureSettings =
    builder.Configuration.GetSection(nameof(NotificationInfrastructureSettings)).Get<NotificationInfrastructureSettings>();

// Получение сборки с консьюмерами
var consumersAssembly = typeof(MessageNotificationService).Assembly;

// Использование IOption<>
builder.Services.Configure<NotificationInfrastructureSettings>
    (
        builder.Configuration.GetSection(nameof(NotificationInfrastructureSettings))
    );

builder.Services
    .AddNotificationApplication()
    .AddNotificationInfrastructure(infrastructureSettings, consumersAssembly)
    .AddNotificationPresentation();
    

var app = builder.Build();

//app.MapGrpcService<GrpcService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();