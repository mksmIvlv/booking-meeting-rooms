using Application.Mediatr.Features.Models;
using Application.Mediatr.Interfaces.Queries;
using Application.Models.Dto;
using AutoMapper;
using Domain.Interfaces.Infrastructure;

namespace Application.Mediatr.Features.Queries;

/// <summary>
/// Обработчик получения расписания
/// </summary>
public class GetScheduleSpecificRoomHandler : IQueryHandler<GetScheduleSpecificRoomQuery, MeetingRoomDto>
{
    #region Поля

    /// <summary>
    /// Репозиторий
    /// </summary>
    private readonly IRepository _repository;

    /// <summary>
    /// Маппинг моделей
    /// </summary>
    private readonly IMapper _mapper;

    #endregion

    #region Конструктор

    public GetScheduleSpecificRoomHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    #endregion

    #region Метод
    
    /// <summary>
    /// Получение расписания
    /// </summary>
    /// <param name="query">Запрос</param>
    /// <param name="cancellationToken">Токен</param>
    /// <returns>Информация о комнате</returns>
    public async Task<MeetingRoomDto> Handle(GetScheduleSpecificRoomQuery query, CancellationToken cancellationToken)
    {
        var meetingRoom = await _repository.GetScheduleAsync(query.IdRoom);
            
        return _mapper.Map<MeetingRoomDto>(meetingRoom);
    }

    #endregion
}