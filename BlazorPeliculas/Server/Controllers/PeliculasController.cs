﻿using AutoMapper;
using BlazorPeliculas.Server.Helpers;
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
    [Route("api/peliculas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            var limite = 6;

            var peliculasEnCartelera = await context.Peliculas.Where(pelicula => pelicula.EnCartelera).Take(limite)
                .OrderByDescending(pelicula => pelicula.FechaLanzamiento).ToListAsync();

            var fechaActual = DateTime.Today;

            var proximosEstrenos = await context.Peliculas.Where(pelicula => pelicula.FechaLanzamiento > fechaActual).Take(limite)
                .OrderBy(pelicula => pelicula.FechaLanzamiento).ToListAsync();

            var resultado = new HomePageDTO
            {
                PeliculasEnCartelera = peliculasEnCartelera,
                ProximosEstrenos = proximosEstrenos,
            };

            return resultado;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaVisualizarDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas.Where(pelicula => pelicula.Id == id)
                .Include(pelicula => pelicula.GenerosPelicula)
                    .ThenInclude(gp => gp.Genero)
                .Include(pelicula => pelicula.PeliculasActor.OrderBy(pa => pa.Orden))
                    .ThenInclude(pa => pa.Actor)
                .FirstOrDefaultAsync();

            if(pelicula == null)
            {
                return NotFound();
            }
            
            var promedioVoto = 0.0;
            var votoUsuario = 0;

            if (await context.VotosPeliculas.AnyAsync(x => x.PeliculaId == id))
            {
                promedioVoto = await context.VotosPeliculas.Where(x => x.PeliculaId == id).AverageAsync(x => x.Voto);

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var usuario = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);

                    if (usuario is null)
                    {
                        return BadRequest("Usuario no encontrado");
                    }

                    var usuarioId = usuario.Id;

                    var votoUsuarioDB = await context.VotosPeliculas.FirstOrDefaultAsync(x => x.PeliculaId == id && x.UsuarioId == usuarioId);

                    if (votoUsuarioDB is not null)
                    {
                        votoUsuario = votoUsuarioDB.Voto;
                    }
                }
            }

            var modelo = new PeliculaVisualizarDTO();
            modelo.Pelicula = pelicula;
            modelo.Generos = pelicula.GenerosPelicula.Select(gp => gp.Genero).ToList();
            modelo.Actor = pelicula.PeliculasActor.Select(pa => new Actor
            {
                Nombre = pa.Actor.Nombre,
                Foto = pa.Actor.Foto,
                Personaje = pa.Personaje,
                Id = pa.ActorId
            }).ToList();

            modelo.PromedioVotos = promedioVoto;
            modelo.VotoUsuario = votoUsuario;

            return modelo;
        }

        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Pelicula>>> Get([FromQuery] ParametrosBusquedaPeliculasDTO modelo)
        {
            //Ejecución diferida para crear un queryable
            var peliculasQueryable = context.Peliculas.AsQueryable();
            //Filtramos solo si el titulo es diferente de null o ""
            if (!string.IsNullOrWhiteSpace(modelo.Titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.Titulo.Contains(modelo.Titulo));
            }
            //Filtramos solo si está en cartelera
            if (modelo.EnCartelera)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.EnCartelera);
            }
            //Filtramos solo si está en estrenos
            if (modelo.Estrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(x => x.FechaLanzamiento >= hoy);
            }
            //Filtramos por el genero según su id
            if (modelo.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.GenerosPelicula
                        .Select(y => y.GeneroId)
                        .Contains(modelo.GeneroId));
            }

            if (modelo.MasVotadas)
            {
                peliculasQueryable = peliculasQueryable.OrderByDescending(p => p.VotosPeliculas.Average(vp => vp.Voto));
            }

            await HttpContext.InsertarParametrosPaginacionEnRespuesta(peliculasQueryable, modelo.CantidadResgistros);

            //Aqui es donde ejecutamos el queryable, antes solo lo "armamos"
            var peliculas = await peliculasQueryable.Paginar(modelo.PaginacionDTO).ToListAsync();
            return peliculas;
        }

        [HttpGet("actualizar/{id}")]
        //Es el put de actualizar más el get para cargar la pantalla de cargar de nuevo la pantalla de actualizar 
        public async Task<ActionResult<PeliculaActualizacionDTO>> PutGet(int id)
        {
            var peliculaActionResult = await Get(id);
            if (peliculaActionResult.Result is NotFoundResult){ return NotFound(); }

            var peliculavisualizarDTO = peliculaActionResult.Value;
            var generosSeleccionadosIds = peliculavisualizarDTO.Generos.Select(x => x.Id).ToList();
            var generosNoSeleccionados = await context.Generos
                .Where(x => !generosSeleccionadosIds.Contains(x.Id)).ToListAsync();

            var modelo = new PeliculaActualizacionDTO();
            modelo.Pelicula = peliculavisualizarDTO.Pelicula;
            modelo.GenerosNoSeleccionados = generosNoSeleccionados;
            modelo.GenerosSeleccionados = peliculavisualizarDTO.Generos;
            modelo.Actores = peliculavisualizarDTO.Actor;
            return modelo;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Pelicula pelicula)
        {
            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var poster = Convert.FromBase64String(pelicula.Poster);
                pelicula.Poster = await almacenadorArchivos.GuardarArchivo(poster, ".jpg", contenedor);
            }

            EscribirOrdenActores(pelicula);

            context.Add(pelicula);
            await context.SaveChangesAsync();
            return pelicula.Id;
        }

        private static void EscribirOrdenActores(Pelicula pelicula)
        {
            if (pelicula.PeliculasActor is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActor.Count; i++)
                {
                    pelicula.PeliculasActor[i].Orden = i + 1;
                }
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(Pelicula pelicula)
        {
            var peliculaDB = await context.Peliculas
                .Include(x => x.GenerosPelicula)
                .Include(x => x.PeliculasActor)
                .FirstOrDefaultAsync(x => x.Id == pelicula.Id);

            if (peliculaDB is null)
            {
                return NotFound();
            }

            // al asignar el mapeo de uno a otro nos ahorramos mucho código
            peliculaDB = mapper.Map(pelicula, peliculaDB);

            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var posterImagen = Convert.FromBase64String(pelicula.Poster);
                peliculaDB.Poster = await almacenadorArchivos.EditarArchivo(posterImagen, ".jpg", contenedor, peliculaDB.Poster);
            }

            EscribirOrdenActores(peliculaDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            context.Remove(pelicula);
            await context.SaveChangesAsync();
            await almacenadorArchivos.EliminarArchivo(pelicula.Poster, contenedor);

            return NoContent();
        }
    }
}

