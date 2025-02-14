using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MVCApplication.Controllers;
using System.Globalization;
using ServiceLayer;

namespace MVCApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<AuctionListingContext, AuctionListingContext>();
            services.AddScoped<IdentityContext, IdentityContext>();
            services.AddScoped<BidContext, BidContext>();
            services.AddScoped<IdentityManager, IdentityManager>();
            services.AddScoped<CarContext, CarContext>();
            services.AddScoped<ListingContext, ListingContext>();
            services.AddScoped<HomeController, HomeController>();

            services.AddDbContext<RevHausDbContext>(op =>
            {
                op.UseSqlServer("Server=DESKTOP-G098CJK\\SQLEXPRESS;Database=RevHaus;Trusted_Connection=True;");
            }, ServiceLifetime.Scoped);

            services.AddIdentity<User, IdentityRole>(iop =>
            {
                iop.Password.RequiredLength = 5;
                iop.Password.RequireNonAlphanumeric = false;
                iop.Password.RequiredUniqueChars = 0;
                iop.Password.RequireUppercase = false;
                iop.Password.RequireLowercase = false;
                iop.Password.RequireDigit = false;

                iop.User.RequireUniqueEmail = true;

                iop.SignIn.RequireConfirmedEmail = false;
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        }
    }
}
