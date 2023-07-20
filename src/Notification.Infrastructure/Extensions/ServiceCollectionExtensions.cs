using System.Reflection;
using Confluent.Kafka;
using Contracts.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Notification.Infrastructure.Connections;
using Notification.Infrastructure.Interfaces.Connections;
using Notification.Infrastructure.Settings;

namespace Notification.Infrastructure.Extensions;

/// <summary>
/// Расширение для подключения сервисов Notification.Infrastructure
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Подключение сервисов
    /// </summary>
    /// <param name="services">Сервисы</param>
    /// <param name="settings">Настройки infr. слоя</param>
    /// <param name="consumers">Сборка с консьюмерами</param>
    /// <returns>Подключенные сервисы</returns>
    public static IServiceCollection AddNotificationInfrastructure(
        this IServiceCollection services, 
        NotificationInfrastructureSettings settings, 
        Assembly consumers)
    {
        // Подключение RabbitMq
        //services.AddScoped<IConnectionRabbit, ConnectionRabbit>();
        //services.AddMassTransitRabbitMq(settings, consumers);
        
        // Подключение Kafka
        //services.AddScoped<IConnectionKafka, ConnectionKafka>();
        //services.AddMassTransitKafka(settings, consumers);
        
        // Работа с помощью gRPC
        //services.AddGrpc();

        // Подключение Telegram
        services.AddScoped<IConnectionTelegram, ConnectionTelegram>();

        return services;
    }
    
     /// <summary>
     /// Подключение MassTransit с RabbitMq
     /// </summary>
     /// <param name="services">Сервисы</param>
     /// <param name="settings">Настройки infr. слоя</param>
     /// <param name="consumers">Сборка с консьюмерами</param>
     /// <returns>Подключенные сервисы</returns>
    private static IServiceCollection AddMassTransitRabbitMq(
         this IServiceCollection services, 
         NotificationInfrastructureSettings settings, 
         Assembly consumers)
    {
        services.AddMassTransit(builder =>
        {
            builder.SetKebabCaseEndpointNameFormatter();

            builder.AddConsumers(consumers);

            builder.UsingRabbitMq((context, config) =>
            {
                config.Host
                (
                    settings.RabbitMqSettings.ConnectionStringRabbitMQ, 
                    settings.RabbitMqSettings.VirtualHost, 
                    options =>
                {
                    options.Username(settings.RabbitMqSettings.LoginRabbitMQ);
                    options.Password(settings.RabbitMqSettings.PasswordRabbitMQ);
                });
                config.ReceiveEndpoint(settings.RabbitMqSettings.Queue, ep =>
                {
                    ep.PrefetchCount = 16;
                    ep.UseMessageRetry(r => r.Interval(2, 100));
                    ep.ConfigureConsumers(context);
                });
            });
        });
        return services;
    }
    
    /// <summary>
    /// Подключение MassTransit с Kafka
    /// </summary>
    /// <param name="services">Сервисы</param>
    /// <param name="settings">Настройки infr. слоя</param>
    /// <param name="consumers">Сборка с консьюмерами</param>
    /// <returns>Подключенные сервисы</returns>
    private static IServiceCollection AddMassTransitKafka(
        this IServiceCollection services, 
        NotificationInfrastructureSettings settings, 
        Assembly consumers)
    {
        services.AddMassTransit(builder =>
        {
            builder.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
            
            builder.AddRider(rider =>
            {
                rider.AddConsumers(consumers);

                rider.UsingKafka((context, host) =>
                {
                    host.Host(settings.KafkaSettings.BootstrapServers, h =>
                    {
                        h.UseSasl(sasl =>
                        {
                            sasl.Username = settings.KafkaSettings.SaslUsername;
                            sasl.Password = settings.KafkaSettings.SaslPassword;
                            sasl.Mechanism = SaslMechanism.Plain;
                        });
                    });
                    host.SecurityProtocol = SecurityProtocol.SaslSsl;

                    host.TopicEndpoint<MessageNotification>(
                        settings.KafkaSettings.TopicName, 
                        settings.KafkaSettings.GroupId, 
                        ep =>
                        {
                            ep.ConfigureConsumers(context);
                        });
                });
            });
        });
        return services;
    }
}