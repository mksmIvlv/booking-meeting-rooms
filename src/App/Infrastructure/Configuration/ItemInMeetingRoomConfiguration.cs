using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

/// <summary>
/// Конфигурация промежуточной таблицы
/// </summary>
public class ItemInMeetingRoomConfiguration : IEntityTypeConfiguration<ItemInMeetingRoom>
{
    #region Метод

    /// <summary>
    /// Конфигурация
    /// </summary>
    public void Configure(EntityTypeBuilder<ItemInMeetingRoom> builder)
    {
        builder.HasKey(q => new { q.IdMeetingRoom, q.IdItem });
        builder.ToTable("ItemInMeetingRoom");
        
        // Связь с MeetingRoom
        builder
            .HasOne(q => q.MeetingRoom)
            .WithMany(q => q.ItemsInMeetingRooms)
            .HasForeignKey(q => q.IdMeetingRoom);
        
        // Связь с Item
        builder
            .HasOne(q => q.Item)
            .WithMany()
            .HasForeignKey(q => q.IdItem);
    }

    #endregion
}