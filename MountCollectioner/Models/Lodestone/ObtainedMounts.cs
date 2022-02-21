using System.Text.Json.Serialization;

namespace MountCollectioner.Models.Lodestone
{
    public class ObtainedMounts
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("ids")]
        public uint[] ids { get; set; }
    }
}
