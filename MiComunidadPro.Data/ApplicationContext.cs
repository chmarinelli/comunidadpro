using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Runtime.Serialization;
using MiComunidadPro.Common.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Threading.Tasks;
using System.Threading;
using MiComunidadPro.Business.Entities.DBO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MiComunidadPro.Business.Entities.LOCATION;
using MiComunidadPro.Business.Entities.IDENTITY;

namespace MiComunidadPro.Data
{
    public class ApplicationContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        private readonly IUserProfile _UserProfile;

        private bool CanUseSessionContext { get; set; }

        #region Constructors

        public ApplicationContext()
        {
            CanUseSessionContext = true;
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options, IUserProfile userProfile)
            : base(options)
        {
            _UserProfile = userProfile;

            CanUseSessionContext = true;
        }

        #endregion

        #region Tables

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationConfiguration> LocationConfigurations { get; set; }
        public virtual DbSet<UserLocation> UserLocations { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<LocationPermission> LocationPermissions { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Charge> Charges { get; set; }

        #endregion

        #region Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Map Composite Keys Entities Here
            //TODO: Set the .HasNoKey() to views and stored procedures

            //Disable the cascade delete behavior
            var cascadeFKs = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys())
                            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

            modelBuilder.Entity<UserRole>().HasOne(d => d.User)
                .WithMany(p => p.Roles)
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<UserRole>().HasOne(d => d.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId);
        }

        #endregion

        #region Save Changes
        public override int SaveChanges()
        {
            // Get the entries that are auditable
            var auditableEntitySet = ChangeTracker.Entries<IAuditableEntity>();

            if (auditableEntitySet != null)
            {
                DateTime currentDate = DateTime.Now;

                // Audit set the audit information foreach record
                foreach (var auditableEntity in auditableEntitySet.Where(c => c.State == EntityState.Added || c.State == EntityState.Modified))
                {
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.CreationDate = currentDate;
                        auditableEntity.Entity.CreateBy = _UserProfile.UserId;
                    }
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Get the entries that are auditable
            var auditableEntitySet = ChangeTracker.Entries<IAuditableEntity>();

            if (auditableEntitySet != null)
            {
                DateTime currentDate = DateTime.Now;

                // Audit set the audit information foreach record
                foreach (var auditableEntity in auditableEntitySet.Where(c => c.State == EntityState.Added || c.State == EntityState.Modified))
                {
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.CreationDate = currentDate;
                        auditableEntity.Entity.CreateBy = _UserProfile.UserId;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}
