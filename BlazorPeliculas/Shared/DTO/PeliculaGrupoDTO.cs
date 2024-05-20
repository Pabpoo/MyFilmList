using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTO
{
    public class PeliculaGrupoDTO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Poster { get; set; }
    }
}
