using BlazorPeliculas.Client;
using BlazorPeliculas.Client.Auth;
using BlazorPeliculas.Client.Repositories;
using CurrieTechnologies.Razor.SweetAlert2;
using Markdig.Parsers.Inlines;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorPeliculas.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //Necesitamos que el modelo sea singleton debido a que enviamos un JSON web token a traves del HTTP client
            //para que al configurar una instancia este se propague por toda la aplicación y poder mandar este token que es un string que identifica al usuario
            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<ServicesSingleton>();
            //services.AddTransient<ServicesTransient>();
            // En la app inyecta un Irepository pero en triemp ode ejecución va a inyectrar el repository
            services.AddSweetAlert2();
            //EscapeInlineParser scoped porque para que sea compatible con el Httpclient de arriba que esta en scoped tienen que ser ambos igual
            services.AddScoped<IRepository, Repository>();
            services.AddAuthorizationCore();

            //Añadimos el servicio una vez para poder reutilizwrlo y que no dé error
            services.AddScoped<ProveedorAutenticacionJWT>();
            services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacionJWT>(proveedor => proveedor.GetRequiredService<ProveedorAutenticacionJWT>());
            services.AddScoped<ILoginService, ProveedorAutenticacionJWT>(proveedor => proveedor.GetRequiredService<ProveedorAutenticacionJWT>());
            services.AddScoped<RenovadorToken>();
        }
    }
}
