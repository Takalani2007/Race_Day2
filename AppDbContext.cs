using Race_Events2.Model;
using Microsoft.EntityFrameworkCore;


namespace Race_Events2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // 📊 Tables
        public DbSet<User> Users => Set<User>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Weather> Weathers => Set<Weather>();
        public DbSet<Enrolment> Enrolments => Set<Enrolment>();
        public DbSet<Participant> Participants => Set<Participant>();
        public DbSet<Feedback> Feedbacks => Set<Feedback>();
        public DbSet<Result> Results => Set<Result>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔒 Unique email per user
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // 🔗 Cascade delete: when an Event is deleted, its children go too
            modelBuilder.Entity<Weather>()
                .HasOne(w => w.Event)
                .WithMany(e => e.Weathers)
                .HasForeignKey(w => w.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrolment>()
                .HasOne(e => e.Event)
                .WithMany(ev => ev.Enrolments)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Participant>()
                .HasOne(p => p.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Event)
                .WithMany(e => e.Feedbacks)
                .HasForeignKey(f => f.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Result>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Results)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🌱 Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasData(
                new Event { EventId = 1, Name = "Tech Conference", Description = "Annual tech meetup", Date = DateTime.Now.AddDays(10), Location = "Durban" },
                new Event { EventId = 2, Name = "Music Festival", Description = "Live bands", Date = DateTime.Now.AddDays(20), Location = "Cape Town" }
            );

            modelBuilder.Entity<Weather>().HasData(
                new Weather { WeatherId = 1, EventId = 1, Condition = "Sunny", Temperature = 25.5, RecordedAt = DateTime.Now },
                new Weather { WeatherId = 2, EventId = 2, Condition = "Cloudy", Temperature = 18.0, RecordedAt = DateTime.Now }
            );

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, FullName = "Admin User", Email = "admin@mail.com", Password = "admin123", Role = "Admin" },
                new User { UserId = 2, FullName = "John Doe", Email = "john@mail.com", Password = "pass123", Role = "User" }
            );
        }
    }
}
