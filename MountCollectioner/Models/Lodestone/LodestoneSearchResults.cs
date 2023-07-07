using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MountCollectioner.Models.Lodestone
{
    public class LodestoneSearchResults
    {
        [JsonPropertyName("Pagination")]
        public Pagination Pagination { get; set; }

        [JsonPropertyName("Results")]
        public List<CharacterSelectable> Character { get; set; }
    }
}
