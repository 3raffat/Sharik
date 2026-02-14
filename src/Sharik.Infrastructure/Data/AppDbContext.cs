using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Auth;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Notifications;
using Sharik.Domain.Ratings;
using Sharik.Domain.Skills;
using Sharik.Domain.Skills.SkillCategories;
using Sharik.Domain.Skills.UserSkills;
using Sharik.Domain.User;
using Sharik.Infrastructure.Auth;

namespace Sharik.Infrastructure.Data
{
    public sealed class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
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
        public DbSet<Notification> Notifications => Set<Notification>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    }
}
