using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MountCollectioner.Serialize
{
    public class MountObtains
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        /*[JsonPropertyName("related_type")]
        public string? RelatedType { get; set; }

        [JsonPropertyName("related_id")]
        public int RelatedId { get; set; }*/
    }
}
