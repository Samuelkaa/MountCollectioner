using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MountCollectioner.Serialize.Lodestone
{
    public class CharacterInformation
    {
        [JsonPropertyName("Mounts")]
        public List<ObtainedMounts> ObtainedMounts { get; set; }
    }
}
