using ChatViewer.BusinessLogic.ChatEvent;
using ChatViewer.Repository;
using Microsoft.EntityFrameworkCore;

namespace ChatViewer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ChatViewerDbContext>(opts =>
            {
                var connString = builder.Configuration.GetConnectionString("ChatViewerDb");
                opts.UseSqlite(connString, options =>
                {
                    options.MigrationsAssembly(typeof(ChatViewerDbContext).Assembly.FullName);
                });
            });

            builder.Services.AddScoped<IChatEventService, ChatEventService>();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                using var context = services.GetRequiredService<ChatViewerDbContext>();
                context.Database.Migrate();
            }
            
            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run("http://localhost:5287");
        }
    }
}