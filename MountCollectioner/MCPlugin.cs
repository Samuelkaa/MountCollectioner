using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using ImGuiScene;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MountCollectioner
{
    public sealed class MCPlugin : IDalamudPlugin
    {
        public string Name => "Mount Collectioner";

        private const string mainCommand = "/mc";

        private const string settingsCommand = "/mcsettings";

        [PluginService]
        internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;

        [PluginService]
        internal static Framework Framework { get; private set; } = null!;

        [PluginService]
        internal static ClientState ClientState { get; private set; } = null!;

        [PluginService]
        internal static DataManager Data { get; private set; } = null!;


        private CommandManager CommandManager { get; init; }
        private MCPluginConf Configuration { get; init; }
        private MCPluginUI PluginUi { get; init; }
        private ConfigUI ConfigUI { get; init; }

        private List<TextureWrap> icons = new List<TextureWrap>();

        public MCPlugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = PluginInterface.GetPluginConfig() as MCPluginConf ?? new MCPluginConf();

            IconsInit();

            this.PluginUi = new MCPluginUI(this.Configuration, icons);
            this.ConfigUI = new ConfigUI(this.Configuration);

            this.CommandManager.AddHandler(mainCommand, new CommandInfo(OnCommand)
            {
                HelpMessage = "Open plugin window"
            });

            this.CommandManager.AddHandler(settingsCommand, new CommandInfo(OnSettingsCommand)
            {
                HelpMessage = "Open plugin settings window"
            });

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.PluginUi.Dispose();
            PluginInterface.SavePluginConfig(this.Configuration);
            this.CommandManager.RemoveHandler(mainCommand);
            GC.SuppressFinalize(this);
        }

        private void OnCommand(string command, string args)
        {
            this.PluginUi.Visible = !this.PluginUi.Visible;
        }

        private void OnSettingsCommand(string command, string args)
        {
            this.ConfigUI.SettingsVisible = !this.ConfigUI.SettingsVisible;
        }

        private void DrawUI()
        {
            this.PluginUi.Draw();
            this.ConfigUI.Draw();
        }

        private void DrawConfigUI()
        {
            this.ConfigUI.SettingsVisible = !this.ConfigUI.SettingsVisible;
        }

        private void IconsInit()
        {
            icons.Add(PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.DirectoryName!, "images", "Y.png")));
            icons.Add(PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.DirectoryName!, "images", "N.png")));
        }
    }
}
