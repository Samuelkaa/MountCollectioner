using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace MountCollectioner
{
    [Serializable]
    public class MCPluginConf : IPluginConfiguration
    {
        public int Version { get; set; } = 1;

        public bool hideCollectedMounts = false;

        public string CharacterId { get; set; } = String.Empty;

        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.pluginInterface!.SavePluginConfig(this);
        }
    }
}
