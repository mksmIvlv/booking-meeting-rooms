using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

/// <summary>
/// Конфигурация комнаты
/// </summary>
public class MeetingRoomConfiguration : IEntityTypeConfiguration<MeetingRoom>
{
    #region Метод

    /// <summary>
    /// Конфигурация
    /// </summary>
    public void Configure(EntityTypeBuilder<MeetingRoom> builder)
    {
        builder.HasKey(q => q.IdRoom);
        builder.Property(q => q.IdRoom)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasIndex(q => q.NameRoom)
            .HasDatabaseName("NameIndex")
            .IsUnique();
        builder.Property(q => q.NameRoom)
            .IsRequired();
        
        builder.Property(q => q.DescriptionRoom)
            .HasMaxLength(50);
        
        builder
            .HasMany(q => q.BookingMeetingRooms)
            .WithOne()
            .HasForeignKey(q=>q.MeetingRoomId);
    }

    #endregion
}