using BlazorPeliculas.Shared.DTO;
using BlazorPeliculas.Shared.Entity;
using MathNet.Numerics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorPeliculas.Server
{
	public class DbInitializer
	{

		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public static async Task Initialize(IServiceProvider serviceProvider)
		{
			//DI
			var applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();


			//Migrations
			var isConnected = false;
			while (isConnected == false)
			{
				try
				{
					await applicationDbContext.Database.MigrateAsync();
					isConnected = true;
				}
				catch (Exception ex)
				{
					var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred migrating the DB.");
				}
				Thread.Sleep(1_000);
			}

			//// Obtener los servicios necesarios para administrar usuarios y roles
			//var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			//var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			//// Verificar si el rol "admin" ya existe, si no, crearlo
			//var roleExists = await roleManager.RoleExistsAsync("admin");
			//if (!roleExists)
			//{
			//	await roleManager.CreateAsync(new IdentityRole { Name = "admin" });
			//}

			//// Verificar si el usuario administrador ya existe, si no, crearlo
			//if (userManager.FindByEmailAsync("admin@admin.es") == null)
			//{
			//	var adminUser = new ApplicationUser
			//	{
			//		UserName = "admin@admin.es",
			//		Email = "admin@admin.es"
			//	};

			//	var result = userManager.CreateAsync(adminUser, "Aa123!");

			//	if (result.IsCompletedSuccessfully)
			//	{
			//		// Asignar el rol "admin" al usuario administrador
			//		await userManager.AddToRoleAsync(adminUser, "admin");
			//	}
			//}
		}
	}
}
