﻿using BlazorPeliculas.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlazorPeliculas.Shared.DTO
{
	public class HomePageDTO
	{
		public List<Pelicula>? PeliculasEnCartelera { get; set; }
        public List<Pelicula>? ProximosEstrenos { get; set; }
    }
}