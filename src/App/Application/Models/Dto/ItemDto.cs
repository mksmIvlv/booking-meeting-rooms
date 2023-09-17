namespace Application.Models.Dto;

public class ItemDto
{
    #region Свойства

    /// <summary>
    /// Название предмета
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание предмета
    /// </summary>
    public string? Description { get; set; }

    #endregion
}