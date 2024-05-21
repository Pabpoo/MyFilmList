using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTO
{
    public class APIPeliculaTrailerDataDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("results")]
        public List<APIPeliculaTrailerDTO> Results { get; set; }
    }
}
