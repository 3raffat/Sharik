using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharik.Domain.Ratings;

namespace Sharik.Infrastructure.Data.Configuration
{
    public sealed class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("Ratings");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Score)
                   .HasMaxLength(5)
                   .IsRequired();

            builder.Property(r => r.Comment)
                   .HasMaxLength(500);

            builder.Property(r => r.Type)
                   .HasConversion<string>()
                   .IsRequired();

            builder.HasOne(r => r.Exchange)
                   .WithMany(e => e.Ratings)
                   .HasForeignKey(r => r.ExchangeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Rater)
                   .WithMany()
                   .HasForeignKey(r => r.RaterId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.RatedUser)
                   .WithMany()
                   .HasForeignKey(r => r.RatedUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(r => !r.IsDeleted);
        }
    }
}
