using Microsoft.EntityFrameworkCore;
using PostgreTest.Data;

namespace PostgreTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<NorthwindContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler =
                    System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Scaffold-DbContext "Host=localhost;Database=northwind;Username=postgres;Password=2959912Ars" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Models -ContextDir Data -Tables employees

            // https://github.com/pthom/northwind_psql
            // "C:\Program Files\PostgreSQL\18\bin\psql.exe" -U postgres -d northwind -f "C:\Users\ppxgp\OneDrive\Рабочий стол\northwind_psql-master\northwind.sql"

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
