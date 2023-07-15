using Application.Mediatr.Features.Models;
using MediatR;

namespace Presentation.Services;

/// <summary>
/// HostedService для разбронирования комнат
/// </summary>
public class UnbookingMeetingRoomHostedService: BackgroundService
{
    #region Поле

    /// <summary>
    /// Сервис провайдер
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region Конструктор

    public UnbookingMeetingRoomHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    #endregion

    #region Метод

    /// <summary>
    /// Разбронирование комнат
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // Создать скоуп Mediator
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new PostUnbookingMeetingRoomCommand());
            }
            await Task.Delay(TimeSpan.FromMinutes(3));
        }
    }

    #endregion
}