using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;
using Movie_Web.Models;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Movie_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add session
            builder.Services.AddSession();

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopRight;
            });

            var stringConnectDb = builder.Configuration.GetConnectionString("dbMovie");
            builder.Services.AddDbContext<MoviesContext>(options => options.UseSqlServer(stringConnectDb));
            builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));


            //Them cache

            builder.Services.AddMemoryCache();

            builder.Services.AddHttpContextAccessor();

            // Configure Cookiee
            builder.Services.AddAuthentication("CookieAuthentication")
                .AddCookie("CookieAuthentication", config =>
                {
                    config.Cookie.Name = "UserLoginCookie";
                    config.ExpireTimeSpan = TimeSpan.FromDays(1);
                    config.LoginPath = "/dang-nhap.html";
                    config.LogoutPath = "/dang-xuat.html";
                    config.AccessDeniedPath = "/not-found.html";
                });
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.ExpireTimeSpan = TimeSpan.FromDays(150);
            });


            var app = builder.Build();

            // use Session
            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseNotyf();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller}/{action=Index}/{id?}");


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
