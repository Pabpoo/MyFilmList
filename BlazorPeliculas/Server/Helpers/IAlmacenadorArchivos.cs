namespace BlazorPeliculas.Server.Helpers
{
	public interface IAlmacenadorArchivos
	{
		//Devuelve un string que es la URL del archivo, el contenido se produce en bytes, la extensión, como un jpg y la carpeta
		Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor);
		//Elimina un archivo a traves de la ruta y su contenedor
		Task EliminarArchivo(string ruta, string nombrContenedor);
		//Eliminamos la imagen anterior y guardamos el nuevo archivo
		async Task<string> EditarArchivo(byte[] contenido, string extension, string nombreContenedor, string ruta)
		{
			if (ruta is not null)
			{
				await EliminarArchivo(ruta, nombreContenedor);
			}
			return await GuardarArchivo(contenido, extension, nombreContenedor);
		}
	}
}
