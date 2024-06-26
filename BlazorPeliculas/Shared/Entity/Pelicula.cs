﻿using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace BlazorPeliculas.Shared.Entity
{
	public class Pelicula
	{

        public int Id { get; set; }
        public int APIId { get; set; }
		[Required]
        public string? Titulo { get; set; }
        public string? Resumen { get; set; }
        public bool EnCartelera { get; set; }
        public string? Trailer { get; set; }
        [Required(ErrorMessage = "La fecha de lanzamiento es requerida.")]
        public DateTime? FechaLanzamiento { get; set; }
		public string? Poster { get; set; }
		public List<GeneroPelicula> GenerosPelicula { get; set; } = new List<GeneroPelicula>();
		public List<PeliculaActor> PeliculasActor { get; set; } = new List<PeliculaActor>();
		public List<VotoPelicula> VotosPeliculas { get; set; } = new List<VotoPelicula>();
        public string? TituloCortado 
		{ get
			{
				if (string.IsNullOrWhiteSpace(Titulo))
				{
					return null;
				}
				if (Titulo.Length > 60)
				{
					return Titulo.Substring(0, 60) + "...";
				}
				else
				{
					return Titulo;
				}
			}
		}
	}
}
