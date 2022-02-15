using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;

namespace MountCollectioner
{
    public sealed class MCPlugin : IDalamudPlugin
    {
        public string Name => "Mount Collectioner";

        private const string mainCommand = "/mounts";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        private MCPluginConf Configuration { get; init; }
        private MCPluginUI PluginUi { get; init; }

        public MCPlugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as MCPluginConf ?? new MCPluginConf();
            this.Configuration.Initialize(this.PluginInterface);

            this.PluginUi = new MCPluginUI(this.Configuration);

            this.CommandManager.AddHandler(mainCommand, new CommandInfo(OnCommand)
            {
                HelpMessage = "Open plugin window"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.PluginUi.Dispose();
            this.CommandManager.RemoveHandler(mainCommand);
        }

        private void OnCommand(string command, string args)
        {
            this.PluginUi.Visible = !this.PluginUi.Visible;
        }

        private void DrawUI()
        {
            this.PluginUi.Draw();
        }

        private void DrawConfigUI()
        {
            this.PluginUi.Visible = !this.PluginUi.Visible;
        }
    }
}
