using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class EventTypeConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder
            .ToTable("Events")
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Title)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder
            .Property(e => e.Description)
            .HasColumnType("varchar(500)")
            .IsRequired();

        builder
            .Property(e => e.StartTime)
            .IsRequired();

        builder
            .Property(e => e.EndTime)
            .IsRequired();

        builder
            .HasMany(e => e.Attendees)
            .WithOne();
    }
}