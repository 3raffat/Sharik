using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharik.Domain.Skills.SkillCategories;

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

            builder.HasQueryFilter(sc => !sc.IsDeleted);
        }
    }
}
