using BlazorPeliculas.Server;
using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace BlazorPeliculas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews()
                // añadimos esta linea para corregir un error que daría en PeliculasController en el endpoint de Task<ActionResult<PeliculaVisualizarDTO>>
                .AddJsonOptions(opciones => opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddRazorPages();

            var connectionString = builder.Configuration.GetConnectionString("BlazorPeliculas");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            builder.Services.AddTransient<IAlmacenadorArchivos, AlamcenadorArchivosLocal>();
            builder.Services.AddHttpContextAccessor();

            // con esto configuramos automapper en el proyecto, que nos permite mapear unos objetos a otros
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Para validar emisores, no lo necesitamos
                    ValidateIssuer = false,
                    // Para validar audiencia(receptores), tampoco lo necesitamos
                    ValidateAudience = false,
                    // Validamos el tiempo de vida del token
                    ValidateLifetime = true,
                    // Valida la llave de firma del emisor, su clave secreta, esto implica que nadie pueda modificar el json wbe token
                    // y alterar los claims para que nuestro sistema de por válido algo que no queremos
                    ValidateIssuerSigningKey = true,
                    // configuramos la llave secreta del token
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["jwtkey"])),
                    ClockSkew = TimeSpan.Zero
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            // Lo necesitamos para poder usar autorización y autenticación
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            CreateDbIfNotExists(app);

            app.Run();
        }
        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    DbInitializer.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
}
