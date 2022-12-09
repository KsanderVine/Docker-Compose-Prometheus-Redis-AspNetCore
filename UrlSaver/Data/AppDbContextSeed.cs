using Microsoft.EntityFrameworkCore;
using UrlSaver.Models;

namespace UrlSaver.Data
{
    public static class AppDbContextSeed
    {
        public static async Task Seed (IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();

            await SeedDataAsync(scope, env);
        }

        private static async Task SeedDataAsync (IServiceScope scope, IWebHostEnvironment env)
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (env.IsProduction())
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not apply migrations: {ex.Message}");
                }
            }

            if (env.IsDevelopment())
            {
                // *****************************
                // * Some data for dev reasons *
                // *****************************

                await context.Urls.AddRangeAsync(new List<Url> ()
                {
                    new Url() { Original = "https://youtube.com/", CreatedAt = DateTime.UtcNow },
                    new Url() { Original = "https://www.hackerrank.com/", CreatedAt = DateTime.UtcNow },
                    new Url() { Original = "https://github.com/", CreatedAt = DateTime.UtcNow },
                    new Url() { Original = "https://learn.microsoft.com/", CreatedAt = DateTime.UtcNow },
                });
                await context.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }
    }
}
