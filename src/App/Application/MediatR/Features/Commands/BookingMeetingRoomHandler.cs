using Application.Mediatr.Features.Models;
using Application.Mediatr.Interfaces.Commands;
using Application.Models.Dto;
using AutoMapper;
using Domain.Interfaces.Infrastructure;

namespace Application.Mediatr.Features.Commands;

/// <summary>
/// Обработчик бронирования комнаты
/// </summary>
public class BookingMeetingRoomHandler: ICommandHandler<PostBookingMeetingRoomCommand, BookingMeetingRoomDto>
{
    #region Поля

    /// <summary>
    /// Доступ к репозиторию
    /// </summary>
    private readonly IRepository _repository;

    /// <summary>
    /// Маппинг моделей
    /// </summary>
    private readonly IMapper _mapper;

    #endregion
    
    #region Конструктор

    public BookingMeetingRoomHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
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
        
        var bookingMeetingRoom = await _repository.BookingMeetingRoomAsync
            (
                command.IdRoom, 
                dateMeeting, 
                startTimeMeeting, 
                endTimeMeeting
            );

        return _mapper.Map<BookingMeetingRoomDto>(bookingMeetingRoom);
    }

    #endregion
}