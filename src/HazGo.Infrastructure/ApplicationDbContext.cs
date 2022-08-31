using HazGo.BuildingBlocks.Core.Domain;
using HazGo.BuildingBlocks.Persistence.EF;
using HazGo.Application.Common.Interfaces;
using HazGo.Domain.Entities;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Module = HazGo.Domain.Entities.Module;

namespace HazGo.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime)
            : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public DbSet<City> City { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<RoleModule> RoleModule { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntityBase<int>> entry in ChangeTracker.Entries<AuditableEntityBase<int>>())
            {
                // TODO :  Fetch CurrentUser from token
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                       entry.Entity.CreatedDate = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                       entry.Entity.UpdatedBy = _currentUserService.UserId;
                        entry.Entity.UpdatedDate = _dateTime.Now;
                        break;
                }
            }

            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntityBase<long>> entry in ChangeTracker.Entries<AuditableEntityBase<long>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "currentUserId";
                        entry.Entity.CreatedDate = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = "currentUserId";
                        entry.Entity.UpdatedDate = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.UseLowerCaseForTableName();
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // write log for option has been configured
            }
            optionsBuilder.UseExceptionProcessor();
        }
    }
}
