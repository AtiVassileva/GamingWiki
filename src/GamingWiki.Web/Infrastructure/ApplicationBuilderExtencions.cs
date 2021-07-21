using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GamingWiki.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var dbContext = scopedServices.ServiceProvider.GetService<ApplicationDbContext>();

            dbContext.Database.Migrate();

            SeedAreas(dbContext);
            SeedClasses(dbContext);
            SeedGenres(dbContext);
            SeedCategories(dbContext);

            return app;
        }

        private static void SeedAreas(ApplicationDbContext dbContext)
        {
            if (dbContext.Areas.Any())
            {
                return;
            }

            dbContext.Areas.AddRange(new []
            {
                new Area {Name = "City"},
                new Area {Name = "Country"},
                new Area {Name = "Space"},
                new Area {Name = "Underwater"},
                new Area {Name = "Desert"},
                new Area {Name = "Forest"},
                new Area {Name = "Jungle"},
                new Area {Name = "Playground"},
                new Area {Name = "Battlefield"},
            });

            dbContext.SaveChanges();
        }
        
        private static void SeedClasses(ApplicationDbContext dbContext)
        {
            if (dbContext.Classes.Any())
            {
                return;
            }

            dbContext.Classes.AddRange(new []
            {
                new Class {Name = "Fighter"}, 
                new Class {Name = "Magician"}, 
                new Class {Name = "Rogue"}, 
                new Class {Name = "Cleric"}, 
                new Class {Name = "Ranger"}, 
                new Class {Name = "Alien"}, 
                new Class {Name = "Ordinary"}, 
                new Class {Name = "Rarer"}, 
                new Class {Name = "Athlete"}, 
                new Class {Name = "Superhero"}, 
                new Class {Name = "Villain"}, 
                new Class {Name = "Criminal"}, 
            });
            
            dbContext.SaveChanges();
        }

        private static void SeedGenres(ApplicationDbContext dbContext)
        {
            if (dbContext.Genres.Any())
            {
                return;
            }

            dbContext.Genres.AddRange(new []
            {
                new Genre{Name = "Action"}, 
                new Genre{Name = "Action Adventure"}, 
                new Genre{Name = "Adventure"}, 
                new Genre{Name = "Role Playing"}, 
                new Genre{Name = "Simulation"}, 
                new Genre{Name = "Strategy"}, 
                new Genre{Name = "Sports"}, 
                new Genre{Name = "Puzzle"}, 
                new Genre{Name = "Idle"}, 
            });

            dbContext.SaveChanges();
        }
        
        private static void SeedCategories(ApplicationDbContext dbContext)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            dbContext.Categories.AddRange(new []
            {
                new Category {Name = "Games"}, 
                new Category {Name = "Characters"}, 
                new Category {Name = "News"}, 
                new Category {Name = "Creators"}, 
                new Category {Name = "Updates"}, 
                new Category {Name = "Others"}, 
            });

            dbContext.SaveChanges();
        }
    }
}
