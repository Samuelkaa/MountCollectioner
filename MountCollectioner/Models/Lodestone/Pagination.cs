using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MountCollectioner.Models.Lodestone
{
    public class Pagination
    {
        [JsonPropertyName("Page")]
        public int Page { get; set; }

        [JsonPropertyName("PageTotal")]
        public int Total { get; set; }

        [JsonPropertyName("Results")]
        public int Results { get; set; }

        [JsonPropertyName("ResultsPerPage")]
        public int ResultsPerPage { get; set; }

        [JsonPropertyName("ResultsTotal")]
        public int ResultsTotal { get; set; }
    }
}
