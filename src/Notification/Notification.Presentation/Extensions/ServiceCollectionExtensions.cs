using Notification.Presentation.Services;

namespace Notification.Presentation.Extensions;

/// <summary>
/// Расширение для подключения сервисов Notification.Presentation
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Подключение сервисов
    /// </summary>
    public static IServiceCollection AddNotificationPresentation(this IServiceCollection services)
    {
        // Если реализация шин базовая, то подключается hosted service
        services.AddHostedService<NotificationHostedService>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}