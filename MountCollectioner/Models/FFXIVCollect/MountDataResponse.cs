using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MountCollectioner.Models
{
    public class MountDataResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("enhanced_description")]
        public string? EnchancedDescription { get; set; }

        [JsonPropertyName("tooltip")]
        public string? Tooltip { get; set; }

        [JsonPropertyName("movement")]
        public string? Movement { get; set; }

        [JsonPropertyName("seats")]
        public int Seats { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("order_group")]
        public int OrderGroup { get; set; }

        [JsonPropertyName("patch")]
        public string? Patch { get; set; }

        [JsonPropertyName("owned")]
        public string? Owned { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("sources")]
        public IList<MountObtains> Sources { get; set; } = new List<MountObtains>();
    }
}
