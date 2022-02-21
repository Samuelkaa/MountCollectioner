using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MountCollectioner.Models.Lodestone
{
    public class CharacterSelectable
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Server")]
        public string Server { get; set; }
    }
}
