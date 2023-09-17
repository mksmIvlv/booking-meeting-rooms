namespace Application.Models.Dto;

public class ItemInMeetingRoomDto
{
    #region Свойства

    /// <summary>
    /// Предмет
    /// </summary>
    public ItemDto Item { get; set; }
    
    /// <summary>
    /// Цена предмета
    /// </summary>
    public decimal? Price { get; set; }

    #endregion
}