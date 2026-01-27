using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Ratings;
using Sharik.Domain.Skills;
using Sharik.Domain.Skills.SkillCategories;
using Sharik.Domain.Skills.UserSkills;
using Sharik.Infrastructure.Auth;

namespace Sharik.Infrastructure.Data
{
    public sealed class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext() { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<UserSkill> UserSkills => Set<UserSkill>();
        public DbSet<SkillCategory> SkillCategories => Set<SkillCategory>();
        public DbSet<Exchange> Exchanges => Set<Exchange>();
        public DbSet<Rating> Ratings => Set<Rating>();

    }
}
