namespace Domain.Models;

/// <summary>
/// Модель промежуточной таблицы
/// </summary>
public class ItemInMeetingRoom
{
    #region Свойства

    /// <summary>
    /// Id предмета
    /// </summary>
    public Guid IdItem { get; private set; }
    
    /// <summary>
    /// Навигационное свойство с моделью предмета
    /// </summary>
    public Item Item { get; private set; }
    
    /// <summary>
    /// Id комнаты
    /// </summary>
    public Guid IdMeetingRoom { get; private set; }
    
    /// <summary>
    /// Навигационное свойство с моделью комнаты
    /// </summary>
    public MeetingRoom MeetingRoom { get; private set; }
    
    /// <summary>
    /// Цена предмета
    /// </summary>
    public decimal? ItemPrice { get; private set; }
    
    #endregion

    #region Конструктор

    private ItemInMeetingRoom() { }

    public ItemInMeetingRoom(Item item, decimal? itemPrice)
    {
        Item = item;
        ItemPrice = itemPrice;
    }

    #endregion
}