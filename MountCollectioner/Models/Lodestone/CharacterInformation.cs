using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MountCollectioner.Models.Lodestone
{
    public class CharacterInformation
    {
        [JsonPropertyName("mounts")]
        public ObtainedMounts ObtainedMounts { get; set; }
    }
}
