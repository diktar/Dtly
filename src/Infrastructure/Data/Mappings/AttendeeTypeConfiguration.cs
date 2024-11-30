using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class AttendeeTypeConfiguration : IEntityTypeConfiguration<Attendee> 
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder
            .ToTable("Attendees")
            .HasKey(a => a.Id);

        builder
            .Property(a => a.Id)
            .IsRequired();

        builder
            .Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(a => a.IsAttending);
        
        // TODO: for now attendee can have one event, review this later
        builder
            .HasOne(a => a.Event)
            .WithMany(b => b.Attendees);
    }
}