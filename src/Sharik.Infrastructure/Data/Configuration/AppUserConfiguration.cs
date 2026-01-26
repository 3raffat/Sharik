using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharik.Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Infrastructure.Data.Configuration
{
    public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(au => au.Id);

            builder.Property(au => au.UserName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(au => au.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(au => au.FirstName)
                   .HasMaxLength(50);

            builder.Property(au => au.LastName)
                   .HasMaxLength(50);

            builder.Property(au => au.Bio)
                   .HasMaxLength(500);  

            builder.Property(au => au.TotalPointsEarned)
                   .HasDefaultValue(50)
                   .IsRequired();

            builder.Property(au => au.ProfileStatus)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(au => au.Rating)
                   .HasPrecision(3, 2);
        }
    }
}
