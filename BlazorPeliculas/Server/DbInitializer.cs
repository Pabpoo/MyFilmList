using BlazorPeliculas.Shared.DTO;
using BlazorPeliculas.Shared.Entity;
using MathNet.Numerics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace BlazorPeliculas.Server
{
    public class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            //DI
            var applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();


            //Migrations
            var isConnected = false;
            while (isConnected == false)
            {
                try
                {
                    applicationDbContext.Database.Migrate();
                    isConnected = true;
                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred migrating the DB.");
                }
                Thread.Sleep(1_000);
            }

            // Seeding DB

            #region Seed Géneros
            if (!applicationDbContext.Generos.Any())
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://api.themoviedb.org/3/genre/movie/list?language=es"),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxNjNiYTQ4YWQyNTkyNThiNzE2NDhjNTFhNDQwNWE5NyIsInN1YiI6IjY2NGNhOTZkZjAyY2VlM2Q5ZTFlMjUwNSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.SonjV-vyt0gB5Wqie5RgM4EymnnDw-H7n0DWANxR_r4" },
                    },
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    var generos = JsonSerializer.Deserialize<APIGenreDataDTO>(body);

                    foreach (var genero in generos.Genres)
                    {
                        applicationDbContext.Generos.Add(new Genero { APIId = genero.Id, Nombre = genero.Name });
                        applicationDbContext.SaveChanges();
                    }
                }
            }
            #endregion

            #region Seed Películas
            if (!applicationDbContext.Peliculas.Any())
            {
                for (int i = 1; i <= 15; i++)
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"https://api.themoviedb.org/3/movie/top_rated?language=es-ES&page={i}"),
                        Headers =
                        {
                            { "accept", "application/json" },
                            { "Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxNjNiYTQ4YWQyNTkyNThiNzE2NDhjNTFhNDQwNWE5NyIsInN1YiI6IjY2NGNhOTZkZjAyY2VlM2Q5ZTFlMjUwNSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.SonjV-vyt0gB5Wqie5RgM4EymnnDw-H7n0DWANxR_r4" },
                        },
                    };

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();

                        var peliculas = JsonSerializer.Deserialize<APIMovieDataDTO>(body);
                        var urlPoster = "https://image.tmdb.org/t/p/w600_and_h900_bestv2";

                        foreach (var pelicula in peliculas.Results)
                        {
                            var peliculaTrailer = "";
                            var clientVideo = new HttpClient();
                            var requestVideo = new HttpRequestMessage
                            {
                                Method = HttpMethod.Get,
                                RequestUri = new Uri($"https://api.themoviedb.org/3/movie/{pelicula.Id}/videos?language=es-ES"),
                                Headers =
                                {
                                    { "accept", "application/json" },
                                    { "Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxNjNiYTQ4YWQyNTkyNThiNzE2NDhjNTFhNDQwNWE5NyIsInN1YiI6IjY2NGNhOTZkZjAyY2VlM2Q5ZTFlMjUwNSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.SonjV-vyt0gB5Wqie5RgM4EymnnDw-H7n0DWANxR_r4" },
                                },
                            };
                            using (var responseVideo = await clientVideo.SendAsync(requestVideo))
                            {
                                responseVideo.EnsureSuccessStatusCode();
                                var bodyVideo = await responseVideo.Content.ReadAsStringAsync();
                                var video = JsonSerializer.Deserialize<APIPeliculaTrailerDataDTO>(bodyVideo);
                                if (!video.Results.IsNullOrEmpty())
                                {
                                    peliculaTrailer = video.Results[0].Key;
                                }
                                else
                                {
                                    var clientVideoIngles = new HttpClient();
                                    var requestVideoIngles = new HttpRequestMessage
                                    {
                                        Method = HttpMethod.Get,
                                        RequestUri = new Uri($"https://api.themoviedb.org/3/movie/{pelicula.Id}/videos?language=en-US"),
                                        Headers =
                                        {
                                            { "accept", "application/json" },
                                            { "Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxNjNiYTQ4YWQyNTkyNThiNzE2NDhjNTFhNDQwNWE5NyIsInN1YiI6IjY2NGNhOTZkZjAyY2VlM2Q5ZTFlMjUwNSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.SonjV-vyt0gB5Wqie5RgM4EymnnDw-H7n0DWANxR_r4" },
                                        },
                                    };
                                    using (var responseVideoIngles = await clientVideoIngles.SendAsync(requestVideoIngles))
                                    {
                                        responseVideoIngles.EnsureSuccessStatusCode();
                                        var bodyVideoIngles = await responseVideoIngles.Content.ReadAsStringAsync();
                                        var videosIngles = JsonSerializer.Deserialize<APIPeliculaTrailerDataDTO>(bodyVideoIngles);
                                        if (!videosIngles.Results.IsNullOrEmpty())
                                        {
                                            if (videosIngles.Results.Any(r => r.Type == "Trailer"))
                                            {
                                                var peliculaTrailerResult = videosIngles.Results.FirstOrDefault(r => r.Type == "Trailer" && r.Site == "YouTube");
                                                if (peliculaTrailerResult != null)
                                                {
                                                    peliculaTrailer = peliculaTrailerResult.Key;
                                                }
                                            }
                                            else
                                            {
                                                var peliculaTrailerResult = videosIngles.Results.FirstOrDefault(r => r.Site == "YouTube");
                                                if (peliculaTrailerResult != null)
                                                {
                                                    peliculaTrailer = peliculaTrailerResult.Key;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            var fechaLanzamientoParseada = DateTime.Parse(pelicula.ReleaseDate);
                            var nuevaPelicula = new Pelicula { APIId = pelicula.Id, Poster = urlPoster + pelicula.PosterPath, Titulo = pelicula.Title, Resumen = pelicula.Overview, FechaLanzamiento = fechaLanzamientoParseada, Trailer = peliculaTrailer };
                            applicationDbContext.Peliculas.Add(nuevaPelicula);
                            foreach (var generoId in pelicula.GenreIds)
                            {
                                var generoActual = applicationDbContext.Generos.Single(g => g.APIId == generoId);
                                var generoPelicula = new GeneroPelicula { Pelicula = nuevaPelicula, Genero = generoActual };
                                applicationDbContext.GenerosPeliculas.Add(generoPelicula);
                            }
                            applicationDbContext.SaveChanges();
                        }
                    }
                }
            }
            #endregion

            #region Seed Actores
            if (!applicationDbContext.Actores.Any())
            {
                var peliculasActores = applicationDbContext.Peliculas.ToList();
                var urlActores = "https://image.tmdb.org/t/p/w600_and_h900_bestv2";
                var urlActoresSinFoto = "https://www.themoviedb.org/assets/2/v4/glyphicons/basic/glyphicons-basic-4-user-grey-d8fe957375e70239d6abdd549fd7568c89281b2179b5f4470e2e12895792dfa5.svg";

                foreach (var pelicula in peliculasActores)
                {
                    if (pelicula.APIId < 1)
                    {
                        continue;
                    }
                    else
                    {
                        var client = new HttpClient();
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = new Uri($"https://api.themoviedb.org/3/movie/{pelicula.APIId}/credits?language=es-ES"),
                            Headers =
                            {
                                { "accept", "application/json" },
                                { "Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxNjNiYTQ4YWQyNTkyNThiNzE2NDhjNTFhNDQwNWE5NyIsInN1YiI6IjY2NGNhOTZkZjAyY2VlM2Q5ZTFlMjUwNSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.SonjV-vyt0gB5Wqie5RgM4EymnnDw-H7n0DWANxR_r4" },
                            },
                        };
                        using (var response = await client.SendAsync(request))
                        {
                            response.EnsureSuccessStatusCode();
                            var body = await response.Content.ReadAsStringAsync();
                            var actores = JsonSerializer.Deserialize<APIActorCastDataDTO>(body);
                            var actoresActing = actores.Cast.Where(a => a.KnownForDepartment == "Acting").ToList();

                            for (int i = 0; i < Math.Min(10, actoresActing.Count); i++)
                            {
                                var actorId = actoresActing[i].Id;
                                Actor actorActual;
                                if (!applicationDbContext.Actores.Any(a => a.APIId == actorId))
                                {
                                    var clientActor = new HttpClient();
                                    var requestActor = new HttpRequestMessage
                                    {
                                        Method = HttpMethod.Get,
                                        RequestUri = new Uri($"https://api.themoviedb.org/3/person/{actorId}?language=es-ES"),
                                        Headers =
                                {
                                    { "accept", "application/json" },
                                    { "Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxNjNiYTQ4YWQyNTkyNThiNzE2NDhjNTFhNDQwNWE5NyIsInN1YiI6IjY2NGNhOTZkZjAyY2VlM2Q5ZTFlMjUwNSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.SonjV-vyt0gB5Wqie5RgM4EymnnDw-H7n0DWANxR_r4" },
                                },
                                    };
                                    using (var responseActor = await clientActor.SendAsync(requestActor))
                                    {
                                        responseActor.EnsureSuccessStatusCode();
                                        var bodyActor = await responseActor.Content.ReadAsStringAsync();
                                        var actor = JsonSerializer.Deserialize<APIActorDetailsDTO>(bodyActor);
                                        var urlActoresFotoActual = "";

                                        if (actor.ProfilePath.IsNullOrEmpty())
                                        {
                                            urlActoresFotoActual = urlActoresSinFoto;
                                        }
                                        else
                                        {
                                            urlActoresFotoActual = urlActores + actor.ProfilePath;
                                        }

                                        if (!actor.Birthday.IsNullOrEmpty())
                                        {
                                            var fechaNacimientoParseada = DateTime.Parse(actor.Birthday);
                                            if (!actor.Biography.IsNullOrEmpty())
                                            {
                                                actorActual = new Actor { APIId = actor.Id, Nombre = actor.Name, Biografia = actor.Biography, Foto = urlActoresFotoActual, FechaNacimiento = fechaNacimientoParseada };
                                            }
                                            else
                                            {
                                                var clientActorIngles = new HttpClient();
                                                var requestActorIngles = new HttpRequestMessage
                                                {
                                                    Method = HttpMethod.Get,
                                                    RequestUri = new Uri($"https://api.themoviedb.org/3/person/{actorId}?language=en-US"),
                                                    Headers =
                                                {
                                                    { "accept", "application/json" },
                                                    { "Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxNjNiYTQ4YWQyNTkyNThiNzE2NDhjNTFhNDQwNWE5NyIsInN1YiI6IjY2NGNhOTZkZjAyY2VlM2Q5ZTFlMjUwNSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.SonjV-vyt0gB5Wqie5RgM4EymnnDw-H7n0DWANxR_r4" },
                                                },
                                                };
                                                using (var responseActorIngles = await clientActorIngles.SendAsync(requestActorIngles))
                                                {
                                                    responseActorIngles.EnsureSuccessStatusCode();
                                                    var bodyActorIngles = await responseActorIngles.Content.ReadAsStringAsync();
                                                    var actorIngles = JsonSerializer.Deserialize<APIActorDetailsDTO>(bodyActorIngles);
                                                    actorActual = new Actor { APIId = actor.Id, Nombre = actor.Name, Biografia = actorIngles.Biography, Foto = urlActoresFotoActual, FechaNacimiento = fechaNacimientoParseada };
                                                }
                                            }
                                            applicationDbContext.Actores.Add(actorActual);
                                            var peliculaActor = new PeliculaActor { Pelicula = pelicula, Actor = actorActual, Personaje = actoresActing[i].Character };
                                            applicationDbContext.PeliculasActores.Add(peliculaActor);
                                            applicationDbContext.SaveChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    actorActual = applicationDbContext.Actores.Single(a => a.APIId == actorId);
                                    var peliculaActor = new PeliculaActor { Pelicula = pelicula, Actor = actorActual, Personaje = actoresActing[i].Character };
                                    applicationDbContext.PeliculasActores.Add(peliculaActor);
                                    applicationDbContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }
    }
}



