using System.ComponentModel.DataAnnotations;

namespace BlazorPeliculas.Shared.Entity
{
	public class Genero
	{
		public int Id {  get; set; }
		[Required(ErrorMessage ="El campo {0} es requerido")]
        public string Nombre { get; set; }
		public List<GeneroPelicula> GenerosPelicula { get; set; } = new List<GeneroPelicula>();
    }
}
