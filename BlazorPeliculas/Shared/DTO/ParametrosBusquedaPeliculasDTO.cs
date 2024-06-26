﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTO
{
    public class ParametrosBusquedaPeliculasDTO
    {
        public int Pagina { get; set; } = 1;
        public int CantidadResgistros { get; set; } = 18;
        public PaginacionDTO PaginacionDTO
        {
            get
            {
                return new PaginacionDTO { Pagina = Pagina, CantidadRegistros = CantidadResgistros };
            }
        }
        public string? Titulo { get; set; }
        public int GeneroId { get; set; }
        public bool EnCartelera { get; set; }
        public bool Estrenos { get; set; }
        public bool MasVotadas { get; set; }
    }
}
