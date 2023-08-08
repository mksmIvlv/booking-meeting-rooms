using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

/// <summary>
/// Конфигурация предметов
/// </summary>
public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    #region Метод

    /// <summary>
    /// Конфигурация
    /// </summary>
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(q => q.Name)
            .IsRequired();
        builder.Property(q => q.Description)
            .HasMaxLength(50);
    }

    #endregion
}