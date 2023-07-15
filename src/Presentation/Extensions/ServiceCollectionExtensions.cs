using System.Reflection;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Presentation.Filters.Exceptions;
using Presentation.Services;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Presentation.Extensions;

/// <summary>
/// Расширение для подключения сервисов Presentation
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Подключение сервисов
    /// </summary>
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(config => 
        {
            config.Filters.Add(new ExceptionFilter());
        });
        
        services.AddHostedService<UnbookingMeetingRoomHostedService>();
        services.AddHostedService<ReminderNotificationHostedService>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Бронирование переговорных комнат",
                Description = "Проект ASP.NET Core Web API",
            });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        // Проверка работоспособности
        services.AddHealthCheck();
        
        // Метрики
        services.AddMetric(configuration);

        return services;
    }

    /// <summary>
    /// Подключение проверок работы приложения
    /// </summary>
    private static IServiceCollection AddHealthCheck(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddHealthChecksUI(options =>
        {
            // В течение которого будет запущена проверка работоспособности
            options.SetEvaluationTimeInSeconds(5);
            // Максимальное количество записей, отображаемых в истории
            options.MaximumHistoryEntriesPerEndpoint(10);
            options.AddHealthCheckEndpoint("Presentation Health Checks API", "/health");
        }).AddInMemoryStorage();
        
        return services;
    }

    /// <summary>
    /// Подключение метрик приложения
    /// </summary>
    private static IServiceCollection AddMetric(this IServiceCollection services, IConfiguration configuration)
    {
        // Prometheus
        services.AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter())
            // Jaeger
            .WithTracing(builder => builder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(configuration["Application:ServiceName"]))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter()
                .AddJaegerExporter()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(configuration["Application:JaegerEndpoint"]);
                }));

        var enviromnent = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMNET");
        
        // Elasticsearch
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(ConfigureElasticSink(configuration))
            .Enrich.WithProperty("Enviromnet", enviromnent)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        
        return services;
    }
    
    /// <summary>
    /// Конфигурация Elastic
    /// </summary>
    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration)
    {
        return new ElasticsearchSinkOptions(new Uri(configuration["Application:ElasticsearchEndpoint"]))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}",
            NumberOfReplicas = 1,
            NumberOfShards = 2
        };
    }
}