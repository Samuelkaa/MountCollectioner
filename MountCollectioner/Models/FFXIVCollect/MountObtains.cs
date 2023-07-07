using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MountCollectioner.Models
{
    public class MountObtains
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
