﻿using Application.AutoMapper.Mapping;
using Application.Interfaces;
using Application.Mediatr.Pipelines;
using Application.Services;
using Application.Services.Kafka;
using Application.Services.RabbitMq;
using Contracts.Interface;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Extensions;

/// <summary>
/// Расширение для подключения сервисов Application
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Подключение сервисов
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Сервис для отправки оповещения, RabbitMQ
        services.AddScoped<IPublishBusService<IMessage>, RabbitMqService<IMessage>>();
        //services.AddScoped<IPublishBusService<IMessage>, MassTransitRabbitMqService<IMessage>>();
        
        // Сервис для отправки оповещения, Kafka
        //services.AddScoped<IPublishBusService<IMessage>, KafkaService<IMessage>>();
        //services.AddScoped<IPublishBusService<IMessage>, MassTransitKafkaService<IMessage>>();
        
        // Сервис для отправки оповещения, gRPC
        //services.AddScoped<IPublishBusService<IMessage>, GrpcService<IMessage>>();

        // Сервис для отправки оповещения, Http
        //services.AddScoped<IPublishBusService<IMessage>, HttpService<IMessage>>();

        // AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        // MediatR
        services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(SavingPipelineBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehaviour<,>));
        
        return services;
    }
}