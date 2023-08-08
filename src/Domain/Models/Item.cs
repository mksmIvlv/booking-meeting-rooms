namespace Domain.Models;

/// <summary>
/// Предмет, который находится в комнате
/// </summary>
public class Item
{
    #region Свойства

    /// <summary>
    /// Id предмета
    /// </summary>
    public Guid Id { get; }
    
    /// <summary>
    /// Название предмета
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Описание предмета
    /// </summary>
    public string? Description { get; }

    #endregion
    
    #region Конструктор
    
    private Item() { }

    public Item(string name, string? description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    #endregion
}