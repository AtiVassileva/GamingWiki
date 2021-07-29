namespace GamingWiki.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;
    
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Creator> Creators { get; set; }

        public DbSet<Discussion> Discussions { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameCreator> GamesCreators { get; set; }

        public DbSet<GamePlatform> GamesPlatforms { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Platform> Platforms { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Trick> Tricks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameCreator>()
                .HasKey(gc => new { gc.CreatorId, gc.GameId });

            modelBuilder.Entity<GamePlatform>()
                .HasKey(gp => new { gp.GameId, gp.PlatformId });

            modelBuilder.Entity<UserDiscussion>()
                .HasKey(ud => new { ud.UserId, ud.DiscussionId });

            
            base.OnModelCreating(modelBuilder);
        }
    }
}
