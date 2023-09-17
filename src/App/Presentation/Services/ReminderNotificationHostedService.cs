using Application.Mediatr.Features.Models;
using Infrastructure.Interfaces.Connections;
using MediatR;

namespace Presentation.Services;

/// <summary>
/// HostedService для отправки напоминаний о бронировании
/// </summary>
public class ReminderNotificationHostedService : BackgroundService
{
    #region Поле

    /// <summary>
    /// Сервис провайдер
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    #endregion
    
    #region Конструктор

    public ReminderNotificationHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    #endregion

    #region Метод

    /// <summary>
    /// Отправка напоминаний о бронировании
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // Создать скоуп Redis
            using (IServiceScope scopeRedis = _serviceProvider.CreateScope())
            {
                var connectionRedis = scopeRedis.ServiceProvider.GetRequiredService<IConnectionRedis>();

                // Максимальное время блокировки при сбое
                var expiry = TimeSpan.FromSeconds(30);
        
                // Время блокировки ресурса
                var wait = TimeSpan.FromSeconds(10);
        
                // Попытки получить доступ
                var retry = TimeSpan.FromSeconds(1);
                
                // Создать блокировку
                await using (var redLock = await connectionRedis.RedLockFactory.CreateLockAsync
                                 (connectionRedis.Settings.ConnectionString, expiry, wait, retry))
                {
                    if (redLock.IsAcquired)
                    {
                        // Создать скоуп Mediator
                        using (IServiceScope scope = _serviceProvider.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                            await mediator.Send(new GetReminderNotificationCommand());
                        }
                    }
                    else
                    {
                        throw new Exception("Превышено ожидание для отправки оповещения о бронирование комнаты.");
                    }
                }
            }
            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }

    #endregion
}