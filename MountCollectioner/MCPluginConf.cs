using Dalamud.Configuration;
using Dalamud.Plugin;
using MountCollectioner.Models.Lodestone;
using System;

namespace MountCollectioner
{
    [Serializable]
    public class MCPluginConf : IPluginConfiguration
    {
        public int Version { get; set; } = 1;

        public bool CrossWorld { get; set; }

        public bool HideCollectedMounts { get; set; } = false;

        public bool IsNotFound { get; set; } = false;

        public string CharacterId { get; set; } = String.Empty;

        public CharacterSelectable SelectedCharacter { get; set; }
    }
}
