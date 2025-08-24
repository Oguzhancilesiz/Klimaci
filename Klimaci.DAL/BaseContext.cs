using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Klimaci.Core.Abstracts;
using Klimaci.Entity;
using Klimaci.Mapping;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Klimaci.DAL
{
    public class BaseContext : IdentityDbContext<AppUser, AppRole, Guid>, IEFContext
    {
        public BaseContext(DbContextOptions options) : base(options)
        {

        }
        public override DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        async Task<int> IEFContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            string id = Guid.NewGuid().ToString().Replace("-", "") + DateTime.Now.ToBinary();
            DateTime now = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries<IEntity>())
            {
                // NOT: tek '|' değil '||' olacak (aşağıda 2. maddede var)
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    entry.Entity.ModifiedDate = now;
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedDate = now;
                        entry.Entity.Status = Core.Enums.Status.Active;
                    }
                }
            }
            int rowCount = 0;
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                // İç hatayı koru; dilersen mesajı yüzeye çıkar:
                throw new DbUpdateException(ex.InnerException?.Message ?? ex.Message, ex);
                // En sade hali: just `throw;`
            }

            return rowCount;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Mapping İşlemleri
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseMap<IEntity>).Assembly);


            var adminId = new Guid("b23a6ab8-adc5-4145-8e2f-35adc69bedfb");
            var hasher = new PasswordHasher<AppUser>();
            var adminUser = new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@onlyik.com",
                NormalizedEmail = "ADMIN@ONLYIK.COM",
                EmailConfirmed = true,
                SecurityStamp = new Guid("3c761708-15e3-49f4-9c02-87fa5c71a8a6").ToString("D"),
                Status = Core.Enums.Status.Active,
                CreatedDate = new DateTime(2025, 08, 16),// Örnek olarak bugünün tarihi
                ModifiedDate = new DateTime(2025, 08, 16)


            };

            // Şifre hash’ini oluştur (örnek: Admin123!)
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

            modelBuilder.Entity<AppUser>().HasData(adminUser);
            base.OnModelCreating(modelBuilder);

        }
    }
}
