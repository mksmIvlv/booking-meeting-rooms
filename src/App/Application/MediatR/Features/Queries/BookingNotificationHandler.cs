using Application.Interfaces;
using Application.Mediatr.Features.Models;
using Application.Mediatr.Interfaces.Queries;
using Contracts.Interface;
using Contracts.Model;
using MediatR;

namespace Application.Mediatr.Features.Queries;

/// <summary>
/// Обработчик отправки оповещения
/// </summary>
public class BookingNotificationHandler : IQueryHandler<PostBookingNotificationQuery, Unit>
{
    #region Поле

    /// <summary>
    /// Сервис для отправки оповещений
    /// </summary>
    private readonly IPublishBusService<IMessage> _publishBusService;

    #endregion

    #region Конструктор

    public BookingNotificationHandler(IPublishBusService<IMessage> publishBusService)
    {
        _publishBusService = publishBusService;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Отправка оповещения
    /// </summary>
    /// <param name="query">Запрос</param>
    /// <param name="cancellationToken">Токен</param>
    public async Task<Unit> Handle(PostBookingNotificationQuery query, CancellationToken cancellationToken)
    {
        var messageBooking = new MessageNotification
        (
            465309919,
            "Комната забронирована"
        );
        await _publishBusService.SendMessageAsync(messageBooking);
        
        return await Unit.Task;
    }

    #endregion
}