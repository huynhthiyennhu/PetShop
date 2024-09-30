using Microsoft.EntityFrameworkCore;
using PetShop.Models;

namespace PetShop.data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PetImage> PetImages { get; set; }
        //public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
       // public DbSet<Discount> Discounts { get; set; }
       // public DbSet<OrderDiscount> OrderDiscounts { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationships
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);


            

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

             modelBuilder.Entity<RolePermission>()
                  .HasOne(rp => rp.Role)
                 .WithMany(r => r.RolePermissions)
                 .HasForeignKey(rp => rp.RoleId);

             modelBuilder.Entity<RolePermission>()
               .HasOne(rp => rp.Permission)
                 .WithMany(p => p.RolePermissions)
                  .HasForeignKey(rp => rp.PermissionId);

            modelBuilder.Entity<Pet>()
               .HasMany(p => p.PetImages)
               .WithOne(pi => pi.Pet)
               .HasForeignKey(pi => pi.PetId);

           
        }
    }

}
