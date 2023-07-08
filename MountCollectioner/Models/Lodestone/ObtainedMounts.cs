using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MountCollectioner.Models.Lodestone
{
    public class ObtainedMounts
    {
        [JsonPropertyName("id")]
        public uint ObtainedMountId { get; set; }
    }
    
}
