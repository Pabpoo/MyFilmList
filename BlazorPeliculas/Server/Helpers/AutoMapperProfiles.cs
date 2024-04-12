using AutoMapper;
using BlazorPeliculas.Shared.DTO;
using BlazorPeliculas.Shared.Entity;

namespace BlazorPeliculas.Server.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Actor, Actor>().ForMember(x => x.Foto, option => option.Ignore());

            CreateMap<Pelicula, Pelicula>().ForMember(x => x.Poster, option => option.Ignore());

            CreateMap<VotoPeliculaDTO, VotoPelicula>();
        }
    }
}
