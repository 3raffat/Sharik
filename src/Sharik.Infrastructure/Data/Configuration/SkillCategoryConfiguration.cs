using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharik.Domain.Skills.SkillCategories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Infrastructure.Data.Configuration
{
    public sealed class SkillCategoryConfiguration : IEntityTypeConfiguration<SkillCategory>
    {
        public void Configure(EntityTypeBuilder<SkillCategory> builder)
        {
            builder.ToTable("SkillCategories");

            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.Name)
                   .IsRequired()
                   .HasMaxLength(20);
        }
    }
}
