using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTO
{
    public class APIActorCastDataDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("cast")]
        public List<APIActorCastDTO> Cast { get; set; }
    }
}
