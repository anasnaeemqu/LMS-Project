using mvcLab.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;
using mvcLab.ErrorHandler;
using mvcLab.Repository.IRepository;
using mvcLab.Repository;
using Microsoft.AspNetCore.Identity;

namespace mvcLab
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
            {
                op.Password.RequireLowercase = false;
                op.Password.RequireUppercase = false;
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequiredLength = 4;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IRepository<Department>, Repository<Department>>();
            builder.Services.AddScoped<IRepository<Course>, Repository<Course>>();
            builder.Services.AddScoped<IRepository<DeptCourse>, Repository<DeptCourse>>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            // 🔹 Seed Roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roles = { "Student", "Instructor", "Admin" };

                foreach (var role in roles)
                {
                    if (!roleManager.RoleExistsAsync(role).Result)
                    {
                        roleManager.CreateAsync(new IdentityRole(role)).Wait();
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
