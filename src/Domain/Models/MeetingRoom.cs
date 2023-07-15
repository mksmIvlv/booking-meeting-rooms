namespace Domain.Models;

/// <summary>
/// Комната
/// </summary>
public class MeetingRoom
{
    #region Свойства

    /// <summary>
    /// Id комнаты
    /// </summary>
    public Guid IdRoom { get; }
    
    /// <summary>
    /// Название комнаты
    /// </summary>
    public string NameRoom { get; }
    
    /// <summary>
    /// Описание комнаты
    /// </summary>
    public string? DescriptionRoom { get; private set; }
    
    /// <summary>
    /// Расписание комнаты (все брони)
    /// </summary>
    public List<BookingMeetingRoom> BookingMeetingRooms { get; private set; }

    #endregion

    #region Конструктор

    public MeetingRoom(string nameRoom, string? descriptionRoom)
    {
        IdRoom = Guid.NewGuid();
        NameRoom = nameRoom;
        DescriptionRoom = descriptionRoom;
        BookingMeetingRooms = new List<BookingMeetingRoom>();
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
                var returnBookingMeetingRoom = new BookingMeetingRoom(dateMeeting, startTimeMeeting, endTimeMeeting, IdRoom);
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
                    var returnBookingMeetingRoom = new BookingMeetingRoom(dateMeeting, startTimeMeeting, endTimeMeeting, IdRoom);
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
    
    #endregion
}