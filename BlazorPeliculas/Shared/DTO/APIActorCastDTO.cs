using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTO
{
    public class APIActorCastDTO
    {
        [JsonPropertyName("adult")]
        public bool Adult { get; set; }

        [JsonPropertyName("gender")]
        public int Gender { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("known_for_department")]
        public string KnownForDepartment { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("original_name")]
        public string OriginalName { get; set; }

        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }

        [JsonPropertyName("profile_path")]
        public string ProfilePath { get; set; }

        [JsonPropertyName("cast_id")]
        public int CastId { get; set; }

        [JsonPropertyName("character")]
        public string Character { get; set; }

        [JsonPropertyName("credit_id")]
        public string CreditId { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }
    }
}
