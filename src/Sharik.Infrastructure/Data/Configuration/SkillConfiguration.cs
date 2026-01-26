using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharik.Domain.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Infrastructure.Data.Configuration
{
    public sealed class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills");
            
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasOne(s => s.SkillCategory)
                   .WithMany(sc => sc.Skills)
                   .HasForeignKey(s => s.SkillCategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

        
        }
    }
}
