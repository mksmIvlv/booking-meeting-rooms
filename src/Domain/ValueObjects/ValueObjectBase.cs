namespace Domain.ValueObjects;

/// <summary>
/// Базовый класс для valueObject
/// </summary>
/// <typeparam name="T">Тип параметризации</typeparam>
public abstract class ValueObjectBase<T>
{
    #region Методы, должны имплементировать наследники

    /// <summary>
    /// Сравнение компонентов
    /// </summary>
    /// <param name="other">С каким объектом сравнивается</param>
    /// <returns>True - объекты одинаковые, false - разные</returns>
    protected abstract bool ComponentsEquals(T other);
    
    /// <summary>
    /// Получение hashCode компонентов
    /// </summary>
    /// <returns>Число</returns>
    protected abstract int? ComponentsHashCode();

    #endregion

    #region Базовые методы
    
    /// <summary>
    /// Оператор равенства
    /// </summary>
    /// <param name="left">Левый параметр</param>
    /// <param name="right">Правый параметр</param>
    /// <returns>True - равны</returns>
    public static bool operator ==(ValueObjectBase<T> left, ValueObjectBase<T> right)
    {
        return Equals(left, right);
    }
    
    /// <summary>
    /// Оператор не равенства
    /// </summary>
    /// <param name="left">Левый параметр</param>
    /// <param name="right">Правый параметр</param>
    /// <returns>True - не равны</returns>
    public static bool operator !=(ValueObjectBase<T> left, ValueObjectBase<T> right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Сравнение с таким же типом
    /// </summary>
    public bool Equals(T? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return ComponentsEquals(other);
    }
    
    /// <summary>
    /// Сравнение с object
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((T) obj);
    }
    
    /// <summary>
    /// Получение hashCode
    /// </summary>
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (ComponentsHashCode() ?? 0);
            
            return hash;
        }
    }

    #endregion
}