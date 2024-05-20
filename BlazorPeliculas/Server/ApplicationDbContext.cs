using BlazorPeliculas.Shared.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlazorPeliculas.Server
{
    //Vamos a heredar de IdentityDbContext y no de DbContext
    //ya que identity contiene un conjunto de identidades que crean las tablas de usuario, roles, esencial para el sistema de usuarios
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Esta linea de código es esencial, no se puede borrar
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GeneroPelicula>().HasKey(x => new { x.PeliculaId, x.GeneroId });
            modelBuilder.Entity<PeliculaActor>().HasKey(x => new { x.PeliculaId, x.ActorId });
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Favoritas)
                .WithMany();
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.PorVer)
                .WithMany();
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Vistas)
                .WithMany();
        }

        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Actor> Actores => Set<Actor>();
        public DbSet<Pelicula> Peliculas => Set<Pelicula>();
        public DbSet<VotoPelicula> VotosPeliculas => Set<VotoPelicula>();
        public DbSet<GeneroPelicula> GenerosPeliculas => Set<GeneroPelicula>();
        public DbSet<PeliculaActor> PeliculasActores => Set<PeliculaActor>();
    }
}
