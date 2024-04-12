using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public List<Pelicula> Favoritas { get; } = new();
        public List<Pelicula> PorVer { get; } = new();
        public List<Pelicula> Vistas { get; } = new();
    }
}
