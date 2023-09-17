using Application.Interfaces;
using Application.Mediatr.Features.Models;
using Application.Mediatr.Interfaces.Commands;
using Contracts.Interface;
using Contracts.Model;
using Domain.Interfaces.Infrastructure;
using MediatR;

namespace Application.Mediatr.Features.Commands;

/// <summary>
/// Обработчик отправки напоминаний о забронированной комнаты
/// </summary>
public class ReminderNotificationHandler : ICommandHandler<GetReminderNotificationCommand, Unit>
{
    #region Поля

    /// <summary>
    /// Сервис для отправки сообщений в шину
    /// </summary>
    private readonly IPublishBusService<IMessage> _publishBusService;

    /// <summary>
    /// Репозиторий
    /// </summary>
    private readonly IRepository _repository;

    #endregion
    
    #region Конструктор

    public ReminderNotificationHandler(IPublishBusService<IMessage> publishBusService, IRepository repository)
    {
        _publishBusService = publishBusService;
        _repository = repository;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Отправка напоминания
    /// </summary>
    /// <param name="command">Команда</param>
    /// <param name="cancellationToken">Токен</param>
    public async Task<Unit> Handle(GetReminderNotificationCommand command, CancellationToken cancellationToken)
    {
        // Получить текущую дату
        var currentDateOnly = DateOnly.FromDateTime(DateTime.Now);
        // Получить текущее время
        var currentTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
        // Максимально время
        var maxTimeOnly = currentTimeOnly.AddHours(1);
        
        // Коллекция для оповещения
        var collectionBooking = _repository.GetRoomsForNotification(currentDateOnly, currentTimeOnly, maxTimeOnly);
        
        for (int i = 0; i < collectionBooking.Count; i++)
        {
            var message = new MessageNotification
            (
                465309919,
                "Оповещение - напоминание о бронировании.\n" +
                $"Id комнаты: {collectionBooking[i].IdMeetingRoom}.\n" +
                $"Дата: {collectionBooking[i].DateMeeting}.\n" +
                $"Время: {collectionBooking[i].StartTimeMeeting} - {collectionBooking[i].EndTimeMeeting}."
            );
            await _publishBusService.SendMessageAsync(message);
        }
        
        return await Unit.Task;
    }

    #endregion
}