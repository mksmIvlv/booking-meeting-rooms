using Application.Mediatr.Features.Models;
using Application.Mediatr.Interfaces.Commands;
using Application.Models.Dto;
using AutoMapper;
using Domain.Interfaces.Infrastructure;
using Domain.Models;

namespace Application.Mediatr.Features.Commands;

public class AddNewItemHandler : ICommandHandler<PostAddNewItemCommand, MeetingRoomDto>
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

    public AddNewItemHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    #endregion
    
    #region Метод
    
    /// <summary>
    /// Добавление нового предмета в комнату
    /// </summary>
    /// <param name="command">Команда</param>
    /// <param name="cancellationToken">Токен</param>
    /// <returns>Информацию о комнате</returns>
    public async Task<MeetingRoomDto> Handle(PostAddNewItemCommand command, CancellationToken cancellationToken)
    {
        var item = new Item(command.NameItem, command.Description);

        var meetingRoom = await _repository.AddNewItemAsync(command.IdRoom, item, command.ItemPrice);
        
        return _mapper.Map<MeetingRoomDto>(meetingRoom);
    }

    #endregion
}