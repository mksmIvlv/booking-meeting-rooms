using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

/// <summary>
/// Конфигурация бронирования комнат
/// </summary>
public class BookingMeetingRoomConfiguration : IEntityTypeConfiguration<BookingMeetingRoom>
{
    #region Метод

    /// <summary>
    /// Конфигурация
    /// </summary>
    public void Configure(EntityTypeBuilder<BookingMeetingRoom> builder)
    {
        builder.HasKey(q => q.IdBooking);
        builder.Property(q => q.IdBooking)
            .ValueGeneratedNever();
        
        builder.Property(q => q.DateMeeting)
            .IsRequired();
        builder.Property(q => q.StartTimeMeeting)
            .IsRequired();
        builder.Property(q => q.EndTimeMeeting)
            .IsRequired();
        builder.Property(q => q.IsNotification)
            .IsRequired();
        builder.Property(q => q.MeetingRoomId)
            .IsRequired();
    }

    #endregion
}