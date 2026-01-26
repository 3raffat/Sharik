using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharik.Domain.Skills.UserSkills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Infrastructure.Data.Configuration
{
    public sealed class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
    {
        public void Configure(EntityTypeBuilder<UserSkill> builder)
        {
            builder.ToTable("UserSkills");

            builder.HasKey(us => us.Id);

            builder.Property(us => us.SkillLevel)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(us => us.PointPerHour)
                   .IsRequired();

            builder.HasOne(us => us.Skill)
                   .WithMany(s => s.UserSkills)
                   .HasForeignKey(us => us.SkillId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(us => us.User)
                   .WithMany(us=>us.UserSkills)
                   .HasForeignKey(us => us.UserId)
                   .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
