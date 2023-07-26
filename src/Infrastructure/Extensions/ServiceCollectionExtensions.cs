using Confluent.Kafka;
using Contracts.Interface;
using Domain.Interfaces.Infrastructure;
using Infrastructure.Connections;
using Infrastructure.Interfaces.Connections;
using Infrastructure.Settings;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Infrastructure.Extensions;

/// <summary>
/// Расширение для подключения сервисов Infrastructure
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Подключение сервисов
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, InfrastructureSettings settings)
    {
        // База данных
        services.AddDbContext<Context>(options => options.UseNpgsql(settings.ConnectionStringDb));
        services.AddScoped<DbContext, Context>();
        services.AddScoped<IRepository, Repository>();
        
        // Проверка работоспособности
        services.AddHealthChecks()
            .AddNpgSql(settings.ConnectionStringDb)
            .AddRabbitMQ(new Uri(settings.RabbitMqSettings.ConnectionAmqp))
            .AddRedis(settings.RedisSettings.ConnectionString)
            .AddKafka(new ProducerConfig
            {
                BootstrapServers = settings.KafkaSettings.BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = settings.KafkaSettings.SaslUsername,
                SaslPassword = settings.KafkaSettings.SaslPassword
            }, settings.KafkaSettings.TopicHealth);
        
        // Подключение шин
        //services.AddScoped<IConnectionRabbitMq, ConnectionRabbitMq>();
        //services.AddMassTransitRabbitMq(settings);
        //services.AddScoped<IConnectionKafka, ConnectionKafka>();
        //services.AddMassTransitKafka(settings);
        
        // Подключение gRPC
        //services.AddScoped<IConnectionGrpc, ConnectionGrpc>();
        
        // Подключение Http c политиками от Microsoft Polly
        /*services.AddHttpClient("HttpService")
            .AddPolicyHandler(GetRetryPolicy());*/

        // Подключение Redis
        services.AddScoped<IConnectionRedis, ConnectionRedis>();

        return services;
    }

    /// <summary>
    /// Подключение MassTransit RabbitMq
    /// </summary>
    private static IServiceCollection AddMassTransitRabbitMq(this IServiceCollection services, InfrastructureSettings settings)
    {
        services.AddMassTransit(builder =>
        {
            builder.UsingRabbitMq((context, config) =>
            {
                config.Host
                (
                    settings.RabbitMqSettings.ConnectionString,
                    settings.RabbitMqSettings.VirtualHost, 
                    options =>
                {
                    options.Username(settings.RabbitMqSettings.Login);
                    options.Password(settings.RabbitMqSettings.Password);
                });
                config.ConfigureEndpoints(context);
            });
        });
        return services;
    }
    
    /// <summary>
    /// Подключение MassTransit Kafka
    /// </summary>
    private static IServiceCollection AddMassTransitKafka(this IServiceCollection services, InfrastructureSettings settings)
    {
        services.AddMassTransit(builder =>
        {
            builder.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });

            builder.AddRider(rider =>
            {
                rider.AddProducer<IMessage>(settings.KafkaSettings.TopicName);
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
                });
            });
        });
        return services;
    }
    
    /// <summary>
    /// Подключение политик
    /// </summary>
    /// <returns>Расширение для политик</returns>
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(3));
    }
}