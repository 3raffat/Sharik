using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharik.Domain.Messages;

namespace Sharik.Infrastructure.Data.Configuration
{
    public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {

            builder.ToTable("Message");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.ExchangeId)
                .IsRequired();

            builder.Property(m => m.SenderId)
                .IsRequired();

            builder.Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.SentAt)
                .IsRequired();

            builder.HasOne(m => m.Exchange)
                .WithMany(m => m.Messages)
                .HasForeignKey(m => m.ExchangeId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
