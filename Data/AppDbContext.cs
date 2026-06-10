
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
    using SOAP.Models;


    namespace SOAP.Data
    {
        public class AppDbContext : IdentityDbContext<ApplicationUser>
        {
            public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
            {
            }


            public DbSet<Trip> Trips { get; set; }

            public DbSet<Location> Locations { get; set; }

            public DbSet<TripLocation> TripLocations { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<TripLocation>()
                    .HasOne(tl => tl.Trip)
                    .WithMany(t => t.TripLocations)
                    .HasForeignKey(tl => tl.TripId);

                modelBuilder.Entity<TripLocation>()
                    .HasOne(tl => tl.Location)
                    .WithMany(l => l.TripLocations)
                    .HasForeignKey(tl => tl.LocationId);

                modelBuilder.Entity<Trip>()
                    .HasOne(t => t.User)
                    .WithMany()
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }

