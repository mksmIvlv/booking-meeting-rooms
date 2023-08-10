using Application.Interfaces;
using Contracts.Interface;
using Infrastructure.Settings;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Application.Services.RabbitMq;

/// <summary>
/// Сервис для отправки сообщений в шину с помощью MassTransit
/// </summary>
public class MassTransitRabbitMqService<T>: IPublishBusService<T> where T: IMessage
{
    #region Свойства

    /// <summary>
    /// Подключение для отправки сообщений в очередь
    /// </summary>
    private readonly IBus _bus;

    /// <summary>
    /// Подключение к RabbitMq
    /// </summary>
    private readonly InfrastructureSettings _settings;

    #endregion
    
    #region Конструктор

    public MassTransitRabbitMqService(IBus bus, IOptions<InfrastructureSettings> settings)
    {
        _bus = bus;
        _settings = settings.Value;
    }

    #endregion
    
    #region Методы

    /// <summary>
    /// Отправка сообщений в очередь
    /// </summary>
    /// <param name="classMessage">Класс - сообщение</param>
    public async Task SendMessageAsync(T classMessage)
    {
        var endPoint = await _bus.GetSendEndpoint
        (
            new Uri($"{
                
                _settings.RabbitMqSettings.NameProvider}://{
                _settings.RabbitMqSettings.ConnectionString}/{
                _settings.RabbitMqSettings.Queue}")
        );
        
        await endPoint.Send(classMessage);
    }

    #endregion
}