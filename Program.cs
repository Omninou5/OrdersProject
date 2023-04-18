using ManagementApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace ManagementApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connection = builder.Configuration.GetConnectionString("DefaultConnection"); //
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection)); //

            builder.Services.AddControllersWithViews(); //

            builder.Services.AddLocalization(); // Добавление службы локализации

            var app = builder.Build();

            app.UseRequestLocalization("en-GB"); // Установка региональных параметров en-GB


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection(); //

            app.UseStaticFiles(); //

            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}"); //

            app.Run();
        }
    }
}