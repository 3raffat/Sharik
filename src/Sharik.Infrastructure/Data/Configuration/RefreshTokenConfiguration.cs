using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharik.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Infrastructure.Data.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token).HasMaxLength(200);

            builder.HasIndex(rt => rt.Token).IsUnique();

            builder.Property(rt => rt.UserId).IsRequired();

            builder.Property(rt => rt.ExpiresOnUtc).IsRequired();
        }
    }
}
