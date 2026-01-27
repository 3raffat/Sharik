using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sharik.Domain.Exchanges;
using Sharik.Domain.Ratings;
using Sharik.Domain.Skills;
using Sharik.Domain.Skills.SkillCategories;
using Sharik.Domain.Skills.UserSkills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Skill> Skills { get; }
        public DbSet<UserSkill> UserSkills { get; }
        public DbSet<SkillCategory> SkillCategories { get;  }
        public DbSet<Exchange> Exchanges { get; }
        public DbSet<Rating> Ratings { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
