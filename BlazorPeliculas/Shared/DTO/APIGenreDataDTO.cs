using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTO
{
    public class APIGenreDataDTO
    {
        [JsonPropertyName("genres")]
        public List<APIGenreDTO> Genres { get; set; }
    }
}
