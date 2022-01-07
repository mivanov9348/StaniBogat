using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StaniBogat.Data
{
    public class StaniBogatDbContext : IdentityDbContext
    {
        public StaniBogatDbContext(DbContextOptions<StaniBogatDbContext> options)
            : base(options)
        {
        }

        public DbSet<Quest> Quests { get; set; }

        public DbSet<Scale> Scales { get; set; }

        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Quest>(x =>
            {
                x.HasKey(x => x.Id);

                x.HasOne(x => x.Scale)
                 .WithMany(x => x.Quests)
                 .HasForeignKey(x => x.ScaleId);
            });

            builder.Entity<Scale>(x =>
            {
                x.HasKey(x => x.Id);

                x.HasMany(x => x.Quests)
                 .WithOne(x => x.Scale)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Game>(x =>
            {
                x.HasKey(x => x.Id);

            });

            base.OnModelCreating(builder);  
        }
    }
}