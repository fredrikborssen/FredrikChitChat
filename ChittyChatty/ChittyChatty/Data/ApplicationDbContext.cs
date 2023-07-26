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
            modelBuilder.Entity<BrokerListingApartment>()
                .HasKey(bl => bl.BuildingId );

            modelBuilder.Entity<BrokerListingHouse>()
                .HasKey(bl =>  bl.BuildingId );

            modelBuilder.Entity<BrokerListingApartment>()
                .HasOne<Broker>(bl => bl.Broker)
                .WithMany()
                .HasForeignKey(bl => bl.BrokerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BrokerListingHouse>()
                .HasOne<Broker>(bl => bl.Broker)
                .WithMany()
                .HasForeignKey(bl => bl.BrokerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BrokerListingApartment>()
                .HasOne<Apartment>(bl => bl.Apartment)
                .WithOne()
                .HasForeignKey<BrokerListingApartment>(bl => bl.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BrokerListingHouse>()
                .HasOne<House>(bl => bl.House)
                .WithOne()
                .HasForeignKey<BrokerListingHouse>(bl => bl.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<BrokerListingApartment> BrokerListingApartments { get; set; }
        public DbSet<BrokerListingHouse> BrokerListingHouses { get; set; }
        public DbSet<Broker> Brokers { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<House> Houses { get; set; }
    }
}