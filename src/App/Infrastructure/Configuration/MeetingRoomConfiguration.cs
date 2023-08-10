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
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasIndex(q => q.Name)
            .HasDatabaseName("NameIndex")
            .IsUnique();
        builder.Property(q => q.Name)
            .IsRequired();
        
        builder.Property(q => q.Description)
            .HasMaxLength(50);
        
        // Связь с BookingMeetingRoom (1-m)
        builder
            .HasMany(q => q.BookingMeetingRooms)
            .WithOne()
            .HasForeignKey(q=> q.IdMeetingRoom);

        // Связь с промежуточной таблицей
        builder
            .HasMany(q => q.ItemsInMeetingRooms)
            .WithOne(q => q.MeetingRoom)
            .HasForeignKey(q => q.IdMeetingRoom);
    }

    #endregion
}