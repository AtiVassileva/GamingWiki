using System;
using System.Linq;
using System.Threading.Tasks;
using GamingWiki.Data;
using GamingWiki.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static GamingWiki.Web.Common.WebConstants;

namespace GamingWiki.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var serviceProvider = scopedServices.ServiceProvider;

            MigrateDatabase(serviceProvider);

            SeedAreas(serviceProvider);
            SeedClasses(serviceProvider);
            SeedGenres(serviceProvider);
            SeedCategories(serviceProvider);
            SeedPlatforms(serviceProvider);
            SeedAdministrator(serviceProvider);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
        private static void SeedAreas(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider
                .GetRequiredService<ApplicationDbContext>();

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
        
        private static void SeedClasses(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider
                .GetRequiredService<ApplicationDbContext>();

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

        private static void SeedGenres(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider
                .GetRequiredService<ApplicationDbContext>();

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
        
        private static void SeedCategories(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider
                .GetRequiredService<ApplicationDbContext>();

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

        private static void SeedPlatforms(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider
                .GetRequiredService<ApplicationDbContext>();

            if (dbContext.Platforms.Any())
            {
                return;
            }

            dbContext.Platforms.AddRange(new []
            {
                new Platform{Name = "PlayStation"},
                new Platform{Name = "Xbox"},
                new Platform{Name = "Nintendo Switch"},
                new Platform{Name = "PC"},
                new Platform{Name = "Mobile"},
            });

            dbContext.SaveChanges();
        }

        private static void SeedAdministrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@gwk.com";
                    const string adminPassword = "admin123";

                    var user = new IdentityUser
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                    };

                    await userManager.CreateAsync(user, adminPassword);
                    await userManager.AddToRoleAsync(user, role.Name);

                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
