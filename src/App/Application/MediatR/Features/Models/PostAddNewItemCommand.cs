using Application.Mediatr.Interfaces.Commands;
using Application.Models.Dto;

namespace Application.Mediatr.Features.Models;

public class PostAddNewItemCommand : ICommand<MeetingRoomDto>
{
    #region Свойства

    public Guid IdRoom { get; }
    
    public string NameItem { get; }
    
    public string? Description { get; }
    
    public decimal? ItemPrice { get; }

    #endregion

    #region Конструктор

    public PostAddNewItemCommand(Guid idRoom, string nameItem, string? description, decimal? itemPrice)
    {
        IdRoom = idRoom;
        NameItem = nameItem;
        Description = description;
        ItemPrice = itemPrice;
    }

    #endregion
}