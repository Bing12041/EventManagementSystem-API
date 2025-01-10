using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.API.Data;

public class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<RSVP> RSVPs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserID);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
        });

        // Event configuration
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventID);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.EndDate).IsRequired();
            
            // Change cascade delete behavior
            entity.HasOne(e => e.User).WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category).WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Location).WithOne(l => l.Event)
                .HasForeignKey<Event>(e => e.LocationID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Category configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.CategoryID);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
        });

        // Location configuration
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(l => l.LocationID);
            entity.Property(l => l.Address).IsRequired().HasMaxLength(150);
            entity.Property(l => l.City).IsRequired().HasMaxLength(50);
            entity.Property(l => l.State).IsRequired().HasMaxLength(50);
            entity.Property(l => l.Country).IsRequired().HasMaxLength(50);
        });

        // RSVP configuration
        modelBuilder.Entity<RSVP>(entity =>
        {
            entity.HasKey(r => new { r.UserID, r.EventID });
            entity.Property(r => r.Status).IsRequired().HasMaxLength(20);

            // Change cascade delete behavior for both FKs
            entity.HasOne(r => r.User).WithMany(u => u.RSVPs)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            entity.HasOne(r => r.Event).WithMany(e => e.RSVPs)
                .HasForeignKey(r => r.EventID)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction
        });

        // Seed data for categories
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryID = 1, Name = "Party" },
            new Category { CategoryID = 2, Name = "Birthday" },
            new Category { CategoryID = 3, Name = "Meetup" }
        );
    }
}