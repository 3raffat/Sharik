using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharik.Domain.Exchanges;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Infrastructure.Data.Configuration
{
    public sealed class ExchangeConfiguration : IEntityTypeConfiguration<Exchange>
    {
        public void Configure(EntityTypeBuilder<Exchange> builder)
        {
            builder.ToTable("Exchanges");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Type)
                 .HasConversion<string>()
                   .IsRequired();
            builder.Property(e => e.ExchangeStatus)
                     .HasConversion<string>()
                     .IsRequired();

            builder.HasOne(e => e.Requester)
                   .WithMany(e=>e.RequestedExchanges)
                   .HasForeignKey(e => e.RequesterId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Provider)
                   .WithMany(e=>e.ProvidedExchanges)
                   .HasForeignKey(e => e.ProviderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.SkillOffered)
                   .WithMany(e=>e.OfferedExchanges)
                   .HasForeignKey(e => e.SkillOfferedId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.SkillRequested)
                     .WithMany(e=>e.RequestedExchanges)
                     .HasForeignKey(e => e.SkillRequestedId)
                     .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
