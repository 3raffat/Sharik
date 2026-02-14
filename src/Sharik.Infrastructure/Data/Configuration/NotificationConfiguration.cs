using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharik.Domain.Notifications;

namespace Sharik.Infrastructure.Data.Configuration
{
    public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {

            builder.ToTable("Notification");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Type)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(n => n.CreatedAt)
                   .IsRequired();

            builder.Property(n => n.IsRead)
                   .IsRequired();


            builder.HasOne(n => n.User)
                .WithMany(n => n.notification)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(n => !n.IsRead);
        }
    }
}
