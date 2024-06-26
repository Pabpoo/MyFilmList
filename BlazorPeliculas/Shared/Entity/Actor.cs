﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorPeliculas.Shared.Entity
{
	public class Actor
	{
        public int Id { get; set; }
        public int APIId { get; set; }

        [Required]
		public string Nombre { get; set; }
		public string? Biografia { get; set; }
        public string? Foto { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        [NotMapped]
        public string? Personaje { get; set; }
        public List<PeliculaActor> PeliculasActor { get; set; } = new List<PeliculaActor>();
        public override bool Equals(object? obj)
        {
            if(obj is Actor a2)
            {
                return Id == a2.Id;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
