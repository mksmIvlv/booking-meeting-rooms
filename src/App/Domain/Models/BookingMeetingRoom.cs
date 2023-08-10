using Domain.Models.Base;

namespace Domain.Models;

/// <summary>
/// Бронирование комнаты
/// </summary>
public class BookingMeetingRoom : ModelBase<BookingMeetingRoom>
{
    #region Свойства

    /// <summary>
    /// Id бронирования
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Дата бронирования
    /// </summary>
    public DateOnly DateMeeting { get; }
    
    /// <summary>
    /// Время начала бронирования
    /// </summary>
    public TimeOnly StartTimeMeeting { get; }
    
    /// <summary>
    /// Время конца бронирования
    /// </summary>
    public TimeOnly EndTimeMeeting { get; }
    
    /// <summary>
    /// Было ли отправленно оповещение о бронировании
    /// </summary>
    public bool IsNotification { get; private set; }
    
    /// <summary>
    /// Id комнаты
    /// </summary>
    public Guid IdMeetingRoom { get; private set; }

    #endregion

    #region Конструктор
    
    private BookingMeetingRoom() { }

    public BookingMeetingRoom(DateOnly dateMeeting, TimeOnly startTimeMeeting, TimeOnly endTimeMeeting, Guid idMeetingRoom)
    {
        Id = Guid.NewGuid();
        DateMeeting = dateMeeting;
        StartTimeMeeting = startTimeMeeting;
        EndTimeMeeting = endTimeMeeting;
        IsNotification = false;
        IdMeetingRoom = idMeetingRoom;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Установить, что о текущем бронировании было отправленно оповещение
    /// </summary>
    public void SetTrueNotification()
    {
        IsNotification = true;
    }

    #endregion
    
    #region Protected методы
    
    /// <summary>
    /// Сравнение компонентов
    /// </summary>
    /// <param name="other">С каким объектом сравнивается</param>
    /// <returns>True - объекты одинаковые, false - разные</returns>
    protected override bool ComponentsEquals(BookingMeetingRoom other)
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