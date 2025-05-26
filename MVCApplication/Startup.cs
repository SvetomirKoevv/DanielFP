using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MVCApplication.Controllers;
using System.Globalization;
using ServiceLayer;
using Hangfire;
using Hangfire.SqlServer;
using MVCApplication.Services;

namespace MVCApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<AuctionListingContext>();
            services.AddScoped<IdentityContext>();
            services.AddScoped<BidContext>();
            services.AddScoped<IdentityManager>();
            services.AddScoped<CarContext>();
            services.AddScoped<ListingContext>();
            services.AddScoped<HomeController>();
            services.AddScoped<AuctionService>();
            services.AddScoped<IRecurringJobManager, RecurringJobManager>();

            services.AddDbContext<RevHausDbContext>(options =>
                options.UseSqlServer("Server=TIMI-PCL\\LAPTOP;Database=RevHaus2;Trusted_Connection=True;"),
                ServiceLifetime.Scoped);

            services.AddHangfire(config =>
                config.UseSqlServerStorage("Server=TIMI-PCL\\LAPTOP;Database=RevHaus2;Trusted_Connection=True;"));
            services.AddHangfireServer();

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<RevHausDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Identity/Account/Login";
                options.SlidingExpiration = true;
            });
        }

        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            using var scope = serviceProvider.CreateScope();
            var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

            jobManager.AddOrUpdate<AuctionService>(
                "CloseExpiredAuctions",
                x => x.CloseExpiredAuctions(),
                Cron.Minutely);

            jobManager.AddOrUpdate<AuctionService>(
                "OpenNewAuctions",
                x => x.OpenNewAuctions(),
                Cron.Minutely);
        }
    }
}
