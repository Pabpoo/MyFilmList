using BlazorPeliculas.Shared.DTO;
using BlazorPeliculas.Shared.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/recomendaciones")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RecomendacionesController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public RecomendacionesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("generos")]
        public async Task<ActionResult<List<PeliculaGrupoDTO>>> GetRecomendaciones()
        {
            // 1. Obtén el usuario y sus películas favoritas.
            var usuario = await context.Users
                .Include(u => u.Favoritas)
                .ThenInclude(pf => pf.GenerosPelicula)
                .ThenInclude(gp => gp.Genero)
                .FirstOrDefaultAsync(u => u.Email == HttpContext.User.Identity.Name);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            var peliculasFavoritas = usuario.Favoritas;

            // 2. Extrae los géneros de estas películas.
            var generosFavoritos = peliculasFavoritas
                .SelectMany(p => p.GenerosPelicula.Select(gp => gp.Genero))
                .ToList();

            // 3. Agrupa los géneros y cuenta cuántas veces aparece cada uno para encontrar los tres géneros más comunes.
            var topGeneros = generosFavoritos
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToList();

            // 4. Busca en la base de datos las películas favoritas de otros usuarios que pertenezcan a estos géneros.
            var recomendaciones = await context.Users
                .Where(u => u.Email != HttpContext.User.Identity.Name)
                .SelectMany(u => u.Favoritas)
                .Where(p => p.GenerosPelicula.Any(gp => topGeneros.Contains(gp.Genero)))
                .ToListAsync();

            // 5. Excluye las películas que el usuario ya ha marcado como favoritas.
            recomendaciones = recomendaciones.Except(peliculasFavoritas).Take(9).ToList();

            // 6. Devuelve estas películas como recomendaciones.
            var respuesta = recomendaciones.Select(p => new PeliculaGrupoDTO
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Poster = p.Poster
            }).ToList();

            return respuesta;
        }

        [HttpGet("gruposusuarios")]
        public async Task<ActionResult<List<PeliculaGrupoDTO>>> GetGruposUsuarios()
        {
            // 1. Obtén el usuario y sus películas favoritas.
            var usuario = await context.Users
                .Include(u => u.Favoritas)
                .ThenInclude(pf => pf.GenerosPelicula)
                .ThenInclude(gp => gp.Genero)
                .FirstOrDefaultAsync(u => u.Email == HttpContext.User.Identity.Name);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            var peliculasFavoritas = usuario.Favoritas;

            // 2. Extrae los géneros de estas películas.
            var generosFavoritos = peliculasFavoritas
                .SelectMany(p => p.GenerosPelicula.Select(gp => gp.Genero))
                .ToList();

            // 3. Agrupa los géneros y cuenta cuántas veces aparece cada uno para encontrar los tres géneros más comunes.
            var topGeneros = generosFavoritos
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToList();

            // 4. Busca en la base de datos los usuarios que tienen en el top 3 los mismos géneros.
            var usuarios = await context.Users
                .Include(u => u.Favoritas)
                .ThenInclude(pf => pf.GenerosPelicula)
                .ThenInclude(gp => gp.Genero)
                .Where(u => u.Email != HttpContext.User.Identity.Name)
                .ToListAsync();

            var usuariosSimilares = usuarios
                .Where(u => u.Favoritas.SelectMany(p => p.GenerosPelicula.Select(gp => gp.Genero)).GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .Intersect(topGeneros).Count() == 3)
                .ToList();

            // 5. Busca las películas favoritas de estos usuarios.
            var recomendaciones = usuariosSimilares
                .SelectMany(u => u.Favoritas)
                .Where(p => p.GenerosPelicula.Any(gp => topGeneros.Contains(gp.Genero)))
                .ToList();

            // 6. Excluye las películas que el usuario ya ha marcado como favoritas.
            recomendaciones = recomendaciones.Except(peliculasFavoritas).Take(9).ToList();

            var respuesta = recomendaciones.Select(p => new PeliculaGrupoDTO
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Poster = p.Poster
            }).ToList();

            return respuesta;
        }
    }
}
