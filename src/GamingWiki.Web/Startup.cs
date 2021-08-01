using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GamingWiki.Data;
using GamingWiki.Services;
using GamingWiki.Services.Contracts;
using GamingWiki.Services.MappingConfiguration;
using GamingWiki.Web.Infrastructure;
using static GamingWiki.Web.Common.WebConstants;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;

namespace GamingWiki.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = PasswordRequiredLength;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews(options => options.Filters
                .Add<AutoValidateAntiforgeryTokenAttribute>());

            services.AddTransient<IGameService, GameService>();
            services.AddTransient<ICharacterService, CharacterService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IReplyService, ReplyService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ITrickService, TrickService>();

            services.AddAutoMapper(typeof(MappingProfile));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
        }
    }
}
