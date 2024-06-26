﻿using BlazorPeliculas.Shared.DTO;
using BlazorPeliculas.Shared.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/listaspeliculas")]
    public class ListasPeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public ListasPeliculasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet("esfavorita/{id:int}")]
        public async Task<ActionResult<bool>> EsFavorita(int id)
        {
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var result = context.Users
                .Where(u => u.Id == usuario.Id)
                .SelectMany(u => u.Favoritas)
                .Any(p => p.Id == id);

            return result;
        }

        [HttpGet("esporver/{id:int}")]
        public async Task<ActionResult<bool>> EsPorVer(int id)
        {
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var result = context.Users
                .Where(u => u.Id == usuario.Id)
                .SelectMany(u => u.PorVer)
                .Any(p => p.Id == id);

            return result;
        }

        [HttpGet("esvista/{id:int}")]
        public async Task<ActionResult<bool>> EsVista(int id)
        {
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var result = context.Users
                .Where(u => u.Id == usuario.Id)
                .SelectMany(u => u.Vistas)
                .Any(p => p.Id == id);

            return result;
        }

        [HttpGet("favoritas")]
        public async Task<ActionResult<List<PeliculaGrupoDTO>>> ObtenerFavoritas()
        {
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var peliculas = await context.Users
                .Where(u => u.Id == usuario.Id)
                .SelectMany(u => u.Favoritas)
                .ToListAsync();

            var respuesta = peliculas.Select(p => new PeliculaGrupoDTO
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Poster = p.Poster
            }).ToList();

            return respuesta;
        }

        [HttpGet("porver")]
        public async Task<ActionResult<List<PeliculaGrupoDTO>>> ObtenerPorVer()
        {
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var peliculas = await context.Users
                .Where(u => u.Id == usuario.Id)
                .SelectMany(u => u.PorVer)
                .ToListAsync();

            var respuesta = peliculas.Select(p => new PeliculaGrupoDTO
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Poster = p.Poster
            }).ToList();

            return respuesta;
        }

        [HttpGet("vistas")]
        public async Task<ActionResult<List<PeliculaGrupoDTO>>> ObtenerVistas()
        {
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var peliculas = await context.Users
                .Where(u => u.Id == usuario.Id)
                .SelectMany(u => u.Vistas)
                .ToListAsync();

            var respuesta = peliculas.Select(p => new PeliculaGrupoDTO
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Poster = p.Poster
            }).ToList();

            return respuesta;
        }

        [HttpPost("favoritas")]
        public async Task<ActionResult> AgregarAFavoritas(PeliculasListasDTO peliculaListaDTO)
        {
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == peliculaListaDTO.PeliculaId);

            if (pelicula is null)
            {
                return BadRequest("Película no encontrada");
            }

            usuario.Favoritas.Add(pelicula);
            //En las relaciones many to many es necesario añadir esta linea para que entity framework sepa que se ha modificado la entidad
            context.Entry(usuario).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("porver")]
        public async Task<ActionResult> AgregarAPorVer(PeliculasListasDTO peliculaListaDTO)
        {
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == peliculaListaDTO.PeliculaId);

            if (pelicula is null)
            {
                return BadRequest("Película no encontrada");
            }

            usuario.PorVer.Add(pelicula);
            context.Entry(usuario).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("vistas")]
        public async Task<ActionResult> AgregarAVistas(PeliculasListasDTO peliculaListaDTO)
        {
            var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == peliculaListaDTO.PeliculaId);

            if (pelicula is null)
            {
                return BadRequest("Película no encontrada");
            }

            usuario.Vistas.Add(pelicula);
            context.Entry(usuario).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("eliminardefavoritas/{id:int}")]
        public async Task<ActionResult> EliminarDeFavoritas(int id)
        {
            var usuario = await userManager.Users.Include(x => x.Favoritas).FirstOrDefaultAsync(x => x.NormalizedEmail == HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var pelicula = usuario.Favoritas.FirstOrDefault(p => p.Id == id);

            if (pelicula is null)
            {
                return BadRequest("Película no encontrada");
            }

            //En las relaciones many to many es necesario añadir esta linea para que entity framework sepa que se ha modificado la entidad
            await context.Database.ExecuteSqlAsync($"DELETE FROM [dbo].[ApplicationUserPelicula] WHERE FavoritasId={pelicula.Id} and ApplicationUserId={usuario.Id}");
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("eliminardeporver/{id:int}")]
        public async Task<ActionResult> EliminarDePorVer(int id)
        {
            var usuario = await userManager.Users.Include(x => x.PorVer).SingleAsync(x => x.NormalizedEmail == HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var pelicula = usuario.PorVer.FirstOrDefault(p => p.Id == id);

            if (pelicula is null)
            {
                return BadRequest("Película no encontrada");
            }

            await context.Database.ExecuteSqlAsync($"DELETE FROM [dbo].[ApplicationUserPelicula1] WHERE PorVerId={pelicula.Id} and ApplicationUser1Id={usuario.Id}");
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("eliminardevistas/{id:int}")]
        public async Task<ActionResult> EliminarDeVistas(int id)
        {
            var usuario = await userManager.Users.Include(x => x.Vistas).SingleAsync(x => x.NormalizedEmail == HttpContext.User.Identity.Name);

            if (usuario is null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var pelicula = usuario.Vistas.FirstOrDefault(p => p.Id == id);

            if (pelicula is null)
            {
                return BadRequest("Película no encontrada");
            }

            await context.Database.ExecuteSqlAsync($"DELETE FROM [dbo].[ApplicationUserPelicula2] WHERE VistasId={pelicula.Id} and ApplicationUser2Id={usuario.Id}");
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
