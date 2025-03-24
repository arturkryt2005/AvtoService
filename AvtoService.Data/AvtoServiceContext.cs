using AvtoService.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AvtoService.Data
{
    public class AvtoServiceContext : DbContext
    {
        public AvtoServiceContext(DbContextOptions<AvtoServiceContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<OrderPart> OrderParts { get; set; }
        public DbSet<OrderService> OrderServices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ContactForm> ContactForm { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Client)
                .WithMany()
                .HasForeignKey(c => c.ClientId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany()
                .HasForeignKey(o => o.ClientId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Car)
                .WithMany()
                .HasForeignKey(o => o.CarId);

            modelBuilder.Entity<OrderPart>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderParts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderPart>()
                .HasOne(op => op.Part)
                .WithMany(p => p.OrderParts)
                .HasForeignKey(op => op.PartId);

            modelBuilder.Entity<OrderService>()
                .HasOne(os => os.Order)
                .WithMany(o => o.OrderServices)
                .HasForeignKey(os => os.OrderId);

            modelBuilder.Entity<OrderService>()
                .HasOne(os => os.Service)
                .WithMany(s => s.OrderServices)
                .HasForeignKey(os => os.ServiceId);

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Phone)
                .IsUnique();

            modelBuilder.Entity<Car>()
                .HasIndex(c => c.Vin)
                .IsUnique();

            modelBuilder.Entity<User>()

                .HasIndex(u => u.Login)
                .IsUnique();

            
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders", t => t.HasCheckConstraint("CK_Order_Status", "[Status] IN ('Pending', 'Completed', 'Canceled')"));
            });
        }
    }
}