using Application.Interfaces;
using Application.Mediatr.Features.Models;
using Application.Mediatr.Interfaces.Commands;
using Contracts.Interface;
using Contracts.Model;
using Domain.Interfaces.Infrastructure;
using MediatR;

namespace Application.Mediatr.Features.Commands;

/// <summary>
/// Обработчик разбронирования комнат
/// </summary>
public class UnbookingMeetingRoomHandler : ICommandHandler<PostUnbookingMeetingRoomCommand, Unit>
{
    #region Поле

    /// <summary>
    /// Доступ к репозиторию
    /// </summary>
    private readonly IRepository _repository;
    
    /// <summary>
    /// Сервис для отправки сообщений в шину
    /// </summary>
    private readonly IPublishBusService<IMessage> _publishBusService;

    #endregion

    #region Конструктор

    public UnbookingMeetingRoomHandler(IRepository repository, IPublishBusService<IMessage> publishBusService)
    {
        _repository = repository;
        _publishBusService = publishBusService;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Разбронирование комнат
    /// </summary>
    /// <param name="command">Команда</param>
    /// <param name="cancellationToken">Токен</param>
    public async Task<Unit> Handle(PostUnbookingMeetingRoomCommand command, CancellationToken cancellationToken)
    {
        // Получить текущую дату
        var currentDateOnly = DateOnly.FromDateTime(DateTime.Now);
        // Получить текущее время
        var currentTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
        
        // Разбронировать комнаты
        await _repository.UnbookingMeetingRoomAsync(currentDateOnly, currentTimeOnly);
        
        // Получить данные для оповещения о разбронировании комнат
        var collectionBooking = await _repository.UnbookingNotificationAsync(currentDateOnly, currentTimeOnly);
        
        // Коллекция комнат для оповещения о разбронировании
        for (int i = 0; i < collectionBooking.Count; i++)
        {
            var message = new MessageNotification
            (
                -1001961900437,
                "Комната разбронирована.\n" +
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