﻿using BlazorPeliculas.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTO
{
    public class PeliculaActualizacionDTO
    {
        public Pelicula Pelicula { get; set; }
        public List<Actor>  Actores { get; set; } = new List<Actor>();
        public List<Genero> GenerosSeleccionados { get; set; } = new List<Genero>();
        public List<Genero> GenerosNoSeleccionados { get; set; } = new List<Genero>();
    }
}
