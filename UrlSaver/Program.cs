using Prometheus;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UrlSaver.Services;
using UrlSaver.Data;

namespace UrlSaver
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemory"));
                builder.Services.AddSingleton<IDistributedCacheService, DevelopmentCacheService>();
            } 
            else
            {
                string sqlConnection = builder.Configuration.GetConnectionString("SqlServer");
                builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConnection));
                builder.Services.AddSingleton<IDistributedCacheService, RedisCacheService>();

                builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
                    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
            }

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IUrlRepository, UrlRepository>();

            builder.Services.AddSingleton<IUrlMetricsService, UrlMetricsService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });

            await AppDbContextSeed.Seed(app, app.Environment);
            app.Run();
        }
    }
}