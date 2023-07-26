using ChittyChatty.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ChittyChatty.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apartment>().HasKey(p => p.BuildingId);
            modelBuilder.Entity<House>().HasKey(p => p.BuildingId);
            modelBuilder.Entity<Broker>().HasKey(p => p.BrokerId);
            modelBuilder.Entity<BrokerListing>()
            .HasKey(bl => new { bl.BrokerId, bl.BuildId });

            modelBuilder.Entity<BrokerListing>()
            .HasOne<Broker>(bl => bl.Broker)
            .WithMany()
            .HasForeignKey(bl => bl.BrokerId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BrokerListing>()
            .HasOne<Apartment>(bl => bl.Apartment)
            .WithOne()
            .HasForeignKey<BrokerListing>(bl => bl.BuildId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BrokerListing>()
            .HasOne<House>(bl => bl.House)
            .WithOne()
            .HasForeignKey<BrokerListing>(bl => bl.BuildId)
            .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<BrokerListing> BrokerListings { get; set; }
        public DbSet<Broker> Brokers { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<House> Houses { get; set; }
    }
}