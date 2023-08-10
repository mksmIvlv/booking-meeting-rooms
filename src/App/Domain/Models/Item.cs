using Domain.Models.Base;

namespace Domain.Models;

/// <summary>
/// Предмет, который находится в комнате
/// </summary>
public class Item : ModelBase<Item>
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
    
    #region Protected методы
    
    /// <summary>
    /// Сравнение компонентов
    /// </summary>
    /// <param name="other">С каким объектом сравнивается</param>
    /// <returns>True - объекты одинаковые, false - разные</returns>
    protected override bool ComponentsEquals(Item other)
    {
        if (Id == other.Id) return true;
        
        return false;
    }

    /// <summary>
    /// Получение hashCode компонентов
    /// </summary>
    /// <returns>Число</returns>
    protected override int? ComponentsHashCode()
    {
        var hash = 0;
        var id = Id.ToString();

        for (int i = 0; i < id.Length ; i++)
        {
            if (id[i] == '-')
            {
                continue;
            }

            hash += (int)id[i];
        }

        return hash;
    }
    
    #endregion
}