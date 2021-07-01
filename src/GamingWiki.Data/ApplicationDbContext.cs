using GamingWiki.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GamingWiki.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

            public DbSet<ApplicationUser> ApplicationUsers { get; set; }

            public DbSet<Article> Articles { get; set; }

            public DbSet<Character> Characters { get; set; }

            public DbSet<Comment> Comments { get; set; }

            public DbSet<Country> Countries { get; set; }

            public DbSet<Creator> Creators { get; set; }

            public DbSet<Discussion> Discussions { get; set; }

            public DbSet<Game> Games { get; set; }

            public DbSet<GameCreator> GamesCreators { get; set; }

            public DbSet<Message> Messages { get; set; }

            public DbSet<Place> Places { get; set; }

            public DbSet<Reply> Replies { get; set; }


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

                modelBuilder.Entity<UserDiscussion>()
                    .HasKey(ud => new { ud.UserId, ud.DiscussionId });

                base.OnModelCreating(modelBuilder);
            }
        }
    }
