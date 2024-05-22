using AutoMapper;
using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTO;
using BlazorPeliculas.Shared.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server.Controllers
{
	[ApiController]
	[Route("api/actores/detalles")]
	public class ActoresDetallesController : ControllerBase
	{
		private readonly ApplicationDbContext context;
		private readonly IAlmacenadorArchivos almacenadorArchivos;
		private readonly IMapper mapper;
		private readonly string contenedor = "personas";

		public ActoresDetallesController(ApplicationDbContext context)
		{
			this.context = context;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Actor>> Get(int id)
		{
			var actor = await context.Actores.FirstOrDefaultAsync(actor => actor.Id == id);

			if (actor is null)
			{
				return NotFound();
			}

			return actor;
		}

		[HttpGet("peliculas/{id}")]
		public async Task<ActionResult<List<PeliculaGrupoDTO>>> GetPeliculasPorActor(int id)
		{
			var peliculas = await context.PeliculasActores
				.Include(pa => pa.Pelicula)
				.Where(pa => pa.ActorId == id)
				.Select(pa => pa.Pelicula)
				.Select(p => new PeliculaGrupoDTO
				{
					Id = p.Id,
					Titulo = p.Titulo,
					Poster = p.Poster
				})
				.ToListAsync();

			return peliculas;
		}
	}
}
