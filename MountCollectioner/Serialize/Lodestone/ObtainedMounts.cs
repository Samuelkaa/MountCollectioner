using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MountCollectioner.Serialize.Lodestone
{
    public class ObtainedMounts
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }
    }
}
