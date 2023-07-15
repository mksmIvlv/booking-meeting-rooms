using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Interfaces;
using Notification.Application.Services;

namespace Notification.Application.Extensions;

/// <summary>
/// Расширение для подключения сервисов Notification.Application
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Подключение сервисов
    /// </summary>
    public static IServiceCollection AddNotificationApplication(this IServiceCollection services)
    {
        // Получение сообщений с помощью RabbitMq
        //services.AddScoped<IConsumerBus, RabbitMqService>();
        
        // Получение сообщений с помощью Kafka
        //services.AddScoped<IConsumerBus, KafkaService>();
        
        // Отправка уведомлений в телеграм конкретным пользователям
        services.AddScoped<INotification, NotificationTelegramService>();

        return services;
    }
}