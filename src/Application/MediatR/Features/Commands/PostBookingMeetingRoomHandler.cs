using Application.Interfaces;
using Application.Mediatr.Features.Models;
using Application.Mediatr.Interfaces.Commands;
using Application.Models.Dto;
using AutoMapper;

namespace Application.Mediatr.Features.Commands;

/// <summary>
/// Обработчик бронирования комнаты
/// </summary>
public class PostBookingMeetingRoomHandler: ICommandHandler<PostBookingMeetingRoomCommand, BookingMeetingRoomDto>
{
    #region Поля

    /// <summary>
    /// Сервис для работы с комнатами
    /// </summary>
    private readonly IRoomService _roomService;

    /// <summary>
    /// Маппинг моделей
    /// </summary>
    private readonly IMapper _mapper;

    #endregion
    
    #region Конструктор

    public PostBookingMeetingRoomHandler(IRoomService roomService, IMapper mapper)
    {
        _roomService = roomService;
        _mapper = mapper;
    }

    #endregion

    #region Метод
    
    /// <summary>
    /// Бронирование комнаты
    /// </summary>
    /// <param name="command">Команда</param>
    /// <param name="cancellationToken">Токен</param>
    /// <returns>Информацию о бронировании</returns>
    public async Task<BookingMeetingRoomDto> Handle(PostBookingMeetingRoomCommand command, CancellationToken cancellationToken)
    {
        // Получить дату
        var dateMeeting = DateOnly.Parse(command.DateMeeting);
        // Получить время начала бронирования
        var startTimeMeeting = TimeOnly.Parse(command.StartTimeMeeting);
        // Получить время конца бронирования
        var endTimeMeeting = TimeOnly.Parse(command.EndTimeMeeting);
        
        var bookingMeetingRoom = await _roomService.BookingRoomAsync(command.IdRoom, dateMeeting, startTimeMeeting, endTimeMeeting);

        return _mapper.Map<BookingMeetingRoomDto>(bookingMeetingRoom);
    }

    #endregion
}