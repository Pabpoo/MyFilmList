
namespace BlazorPeliculas.Server.Helpers
{
	public class AlamcenadorArchivosLocal : IAlmacenadorArchivos
	{
		private readonly IWebHostEnvironment env;

		public AlamcenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
			this.env = env;
			HttpContextAccessor = httpContextAccessor;
		}

		public IHttpContextAccessor HttpContextAccessor { get; }

		public Task EliminarArchivo(string ruta, string nombreContenedor)
		{
			var nombreArchivo = Path.GetFileName(ruta);
			var directorioArchivo = Path.Combine(env.WebRootPath, nombreContenedor,  nombreArchivo);

			if(File.Exists(directorioArchivo))
			{
				File.Delete(directorioArchivo);
			}

			return Task.CompletedTask;
		}

		public async Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor)
		{
			var nombreArchivo =$"{Guid.NewGuid()}{extension}";
			var folder = Path.Combine(env.WebRootPath, nombreContenedor);

			if(!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			string rutaGuardado = Path.Combine(folder, nombreArchivo);
			await File.WriteAllBytesAsync(rutaGuardado, contenido);

			var urlActual = $"{HttpContextAccessor.HttpContext.Request.Scheme}://{HttpContextAccessor.HttpContext.Request.Host}";
			//SearchOption pone doble \\ porque la \ se usa como escape, asi que hay que ponerle 2
			var rutaBD = Path.Combine(urlActual, nombreContenedor, nombreArchivo).Replace("\\", "/");
			return rutaBD;
		}
	}
}
