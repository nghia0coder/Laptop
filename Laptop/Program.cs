using Laptop.Models;
using Microsoft.EntityFrameworkCore;
using Laptop.Areas.Admin.InterfacesRepositories;
using Laptop.Areas.Admin.Repositories;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Laptop.Models;

namespace Laptop
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<LaptopContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Store"));
=======
			// Add services to the container.
			builder.Services.AddControllersWithViews().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
			});
			builder.Services.AddDbContext<LaptopContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("Store"));
>>>>>>> nghia2

			});
			builder.Services.AddIdentity<AppUserModel, IdentityRole>()
	.AddEntityFrameworkStores<LaptopContext>().AddDefaultTokenProviders();
			builder.Services.Configure<IdentityOptions>(options =>
			{
				// Password settings.
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 4;




				// User settings.
				options.User.RequireUniqueEmail = true;
			});
			builder.Services.AddDistributedMemoryCache();
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout as needed
				options.Cookie.IsEssential = true;
			});

			builder.Services.AddScoped<IBrandRepository, BrandRepository>();
			builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			builder.Services.AddHttpContextAccessor();
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseSession();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "areas",
					pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
				);

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

			using (var scope = app.Services.CreateScope())
			{
				var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var roles = new[] { "Manager", "Staff", "User" };

				foreach (var role in roles)
				{
					if (!await roleManger.RoleExistsAsync(role))
						await roleManger.CreateAsync(new IdentityRole(role));
				}
			}
			using (var scope = app.Services.CreateScope())
			{
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUserModel>>();
				string email = "admin@gmail.com";
				string emailtStaff = "staff@gmail.com";
				string password = "Test@1234";
				string address = "SG";
				if (await userManager.FindByEmailAsync(email) == null)
				{
					var user = new AppUserModel();
					user.UserName = email;
					user.Email = email;
					user.Address = address;
					await userManager.CreateAsync(user, password);
					await userManager.AddToRoleAsync(user, "Manager");
				}
				if (await userManager.FindByEmailAsync(emailtStaff) == null)
				{
					var user = new AppUserModel();
					user.UserName = emailtStaff;
					user.Email = emailtStaff;
					user.Address = address;
					await userManager.CreateAsync(user, password);
					await userManager.AddToRoleAsync(user, "Staff");
				}


			}
			app.Run();

		}
	}
}