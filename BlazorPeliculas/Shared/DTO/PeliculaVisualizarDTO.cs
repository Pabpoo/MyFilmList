﻿using BlazorPeliculas.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTO
{
    public class PeliculaVisualizarDTO
    {
        public Pelicula Pelicula { get; set; } = null;
        public List<Genero> Generos { get; set; } = new List<Genero>();
        public List<Actor> Actor { get; set; } = new List<Actor>();
        public int VotoUsuario { get; set; }
        public double PromedioVotos { get; set; }
    }
}
