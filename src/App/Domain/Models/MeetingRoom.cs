using Domain.Models.Base;

namespace Domain.Models;

/// <summary>
/// Комната
/// </summary>
public class MeetingRoom : ModelBase<MeetingRoom>
{
    #region Свойства

    /// <summary>
    /// Id комнаты
    /// </summary>
    public Guid Id { get; }
    
    /// <summary>
    /// Название комнаты
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Описание комнаты
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Расписание комнаты (все брони)
    /// </summary>
    public List<BookingMeetingRoom> BookingMeetingRooms { get; private set; } = new();

    /// <summary>
    /// Связь с промежуточной таблицей
    /// </summary>
    public List<ItemInMeetingRoom> ItemsInMeetingRooms { get; private set; } = new();

    #endregion

    #region Конструктор
    
    private MeetingRoom() { }

    public MeetingRoom(string name, string? description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    #endregion

    #region Методы

    /// <summary>
    /// Бронирование комнаты
    /// </summary>
    /// <param name="dateMeeting">Дата</param>
    /// <param name="startTimeMeeting">Время начала</param>
    /// <param name="endTimeMeeting">Время конца</param>
    /// <returns>Комнату с данными</returns>
    public BookingMeetingRoom BookingRoom(DateOnly dateMeeting, TimeOnly startTimeMeeting, TimeOnly endTimeMeeting)
    {
        // Граница даты, уменьшена для теста
        var tempDateMeeting = dateMeeting >= new DateOnly(2020, 01, 01) &&
                              dateMeeting <= new DateOnly(2030, 12, 31);
        // Нижняя граница для записи
        var leftTimeMeeting = new TimeOnly(00, 00, 00);
        // Верхняя граница для записи
        var rightTimeMeeting = new TimeOnly(23, 59, 59);
        
        if (startTimeMeeting < endTimeMeeting &&
            leftTimeMeeting <= startTimeMeeting &&
            endTimeMeeting <= rightTimeMeeting &&
            tempDateMeeting)
        {
            // Если комнат нет, сразу добавляем
            if (BookingMeetingRooms.Count == 0)
            {
                var returnBookingMeetingRoom = new BookingMeetingRoom(dateMeeting, startTimeMeeting, endTimeMeeting, Id);
                BookingMeetingRooms.Add(returnBookingMeetingRoom);

                return returnBookingMeetingRoom;
            }
            else
            {
                // Если время начала пересекается с временем начала уже забронированной комнатой
                var leftBorderBookingMeetingRoom = BookingMeetingRooms
                    .FirstOrDefault(e => (dateMeeting == e.DateMeeting) &&
                                         (startTimeMeeting >= e.StartTimeMeeting && startTimeMeeting < e.EndTimeMeeting));
                
                // Если время конца пересекается с временем конца уже забронированной комнатой
                var rightBorderBookingMeetingRoom = BookingMeetingRooms
                    .FirstOrDefault(e => (dateMeeting == e.DateMeeting) &&
                                         (endTimeMeeting > e.StartTimeMeeting  && endTimeMeeting <= e.EndTimeMeeting));
                
                // Если внутри границ времени есть уже забронированная комната
                var middleBorderBookingMeetingRoom = BookingMeetingRooms
                    .FirstOrDefault(e => (dateMeeting == e.DateMeeting) &&
                                         (e.StartTimeMeeting >= startTimeMeeting && e.EndTimeMeeting <= endTimeMeeting));
                
                // Если все границы null, значит можно добавить комнату
                if (leftBorderBookingMeetingRoom == null &&
                    middleBorderBookingMeetingRoom == null &&
                    rightBorderBookingMeetingRoom == null)
                {
                    var returnBookingMeetingRoom = new BookingMeetingRoom(dateMeeting, startTimeMeeting, endTimeMeeting, Id);
                    BookingMeetingRooms.Add(returnBookingMeetingRoom);

                    return returnBookingMeetingRoom;
                }
                else
                {
                    throw new Exception("забронировать комнату нельзя." +
                                                " Время бронирования новой комнаты пересекается с ранее забронированными комнатами.");
                }
            }
        }
        else
        {
            throw new Exception("не верное время или дата бронирования.");
        }
    }

    /// <summary>
    /// Разбронирование комнаты
    /// </summary>
    /// <param name="currentDateOnly">Текущая дата</param>
    /// <param name="currentTimeOnly">Текущее время</param>
    public void UnbookingRoom(DateOnly currentDateOnly, TimeOnly currentTimeOnly)
    {
        var bookingMeetingRooms = BookingMeetingRooms
            .Where(e => (e.DateMeeting < currentDateOnly) 
                        || (e.DateMeeting == currentDateOnly && e.EndTimeMeeting < currentTimeOnly))
            .ToList();

        foreach (var item in bookingMeetingRooms)
        {
            BookingMeetingRooms.Remove(item);
        }
    }

    /// <summary>
    /// Добавление предметов в комнату
    /// </summary>
    /// <param name="item">Предмет</param>
    public void AddItem(Item item, decimal itemPrice)
    {
        ItemsInMeetingRooms.Add(new ItemInMeetingRoom(item, itemPrice));
    }

    /// <summary>
    /// Удаление связи с предметом
    /// </summary>
    /// <param name="item">Предмет</param>
    public void RemoveItem(Guid idItem)
    {
        var itemsInMeetingRooms = ItemsInMeetingRooms.FirstOrDefault(q => q.IdItem == idItem);
        ItemsInMeetingRooms.Remove(itemsInMeetingRooms);
    }
    
    #endregion

    #region Protected методы
    
    /// <summary>
    /// Сравнение компонентов
    /// </summary>
    /// <param name="other">С каким объектом сравнивается</param>
    /// <returns>True - объекты одинаковые, false - разные</returns>
    protected override bool ComponentsEquals(MeetingRoom other)
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