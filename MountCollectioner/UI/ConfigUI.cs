using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using MountCollectioner.Models.Lodestone;
using MountCollectioner.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace MountCollectioner
{
    public class ConfigUI
    {
        private MCPluginConf configuration;

        private bool settingsVisible = false;
        public bool SettingsVisible
        {
            get { return this.settingsVisible; }
            set { this.settingsVisible = value; }
        }

        private string characterName = String.Empty;

        private string selectedServer = String.Empty;

        private LodestoneSearchResults characterSearchResult;

        public ConfigUI(MCPluginConf configuration)
        {
            this.configuration = configuration;
        }

        public void Draw()
        {
            DrawSettingsWindow();
        }

        private void DrawSettingsWindow()
        {
            if (!SettingsVisible)
            {
                return;
            }
            if (configuration.SelectedCharacter == null)
            {
                ImGui.SetNextWindowSize(new Vector2(500, 400), ImGuiCond.Always);
                ImGui.SetNextWindowSizeConstraints(new Vector2(500, 400), new Vector2(500, 400));
                if (ImGui.Begin("Settings", ref settingsVisible, ImGuiWindowFlags.NoScrollbar))
                {
                    ImGui.SetNextItemWidth(ImGui.GetWindowSize().X - 150);
                    ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 5);
                    ImGui.InputTextWithHint("", "Character Name for tracking", ref characterName, 256);
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(130);
                    if (ImGui.BeginCombo("##world", selectedServer))
                    {
                        if (ImGui.Selectable("All worlds"))
                        {
                            selectedServer = String.Empty;
                        }
                        foreach (var world in worldsList())
                        {
                            if (ImGui.Selectable(world.Name))
                            {
                                selectedServer = world.Name;
                            }
                        }

                        ImGui.EndCombo();
                    }
                    ImGui.SetCursorPosX(185);
                    if (ImGui.Button("Search", new Vector2(130, 25)))
                    {
                        SearchCharacters(characterName, selectedServer);
                    }
                    ImGui.BeginChild("characterSelector", new Vector2(ImGui.GetWindowSize().X - 15, 300), true);

                    if (characterSearchResult != null)
                    {
                        foreach (var character in characterSearchResult.Character)
                        {
                            if (ImGui.Selectable($"{character.Name}, {character.Server}, ID: {character.ID}"))
                            {
                                configuration.SelectedCharacter = character;
                            }
                        }
                    }

                    ImGui.EndChild();
                }
                ImGui.End();
            }
            else
            {
                if (configuration.IsNotFound == true)
                {
                    ImGui.SetNextWindowSize(new Vector2(300, 225), ImGuiCond.Always);
                    ImGui.SetNextWindowSizeConstraints(new Vector2(300, 225), new Vector2(300, 225));
                    if (ImGui.Begin("Character Info", ref settingsVisible, ImGuiWindowFlags.NoScrollbar))
                    {
                        ImGui.Text($"{configuration.SelectedCharacter.Name}");
                        ImGui.Text($"{configuration.SelectedCharacter.Server}");
                        ImGui.Text($"{configuration.SelectedCharacter.ID}");
                        if (ImGui.Button("Change character"))
                        {
                            configuration.SelectedCharacter = null;
                        }

                        ImGui.Spacing();
                        ImGui.Separator();

                        ImGui.TextWrapped("The selected character was never tracked at");
                        if (ImGui.Button("https://ffxivcollect.com"))
                        {
                            try
                            {
                                _ = Process.Start(new ProcessStartInfo()
                                {
                                    FileName = $"https://ffxivcollect.com",
                                    UseShellExecute = true,
                                });
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        ImGui.TextWrapped("You need to authorize once for tracking collected mounts in plugin window.");
                    }
                }
                else
                {
                    ImGui.SetNextWindowSize(new Vector2(200, 150), ImGuiCond.Always);
                    ImGui.SetNextWindowSizeConstraints(new Vector2(200, 150), new Vector2(200, 150));
                    if (ImGui.Begin("Character Info", ref settingsVisible, ImGuiWindowFlags.NoScrollbar))
                    {
                        ImGui.Text($"{configuration.SelectedCharacter.Name}");
                        ImGui.Text($"{configuration.SelectedCharacter.Server}");
                        ImGui.Text($"{configuration.SelectedCharacter.ID}");
                        if (ImGui.Button("Change character"))
                        {
                            configuration.SelectedCharacter = null;
                        }
                    }
                }
            }
        }

        private List<World> worldsList()
        {
            try
            {
                var currentDC = MCPlugin.ClientState.LocalPlayer.CurrentWorld.GameData.DataCenter;
                var dcWorlds = MCPlugin.Data.GetExcelSheet<World>()
                    .Where(w => w.DataCenter.Row == currentDC.Row)
                    .OrderBy(w => w.Name.ToString().ToUpperInvariant());
                List<World> sortWorld = new List<World>();

                foreach (var world in dcWorlds)
                {
                    sortWorld.Add(world);
                }

                return sortWorld;
            }
            catch (Exception ex)
            {
                return new List<World>();
            }
        }

        private void SearchCharacters(string characterName, string selectedServer)
        {
            Task.Run(async () =>
            {
                this.characterSearchResult = await LodestoneSearchCharRequest
                  .SearchCharacters(characterName, selectedServer, CancellationToken.None)
                  .ConfigureAwait(false);
            });
        }
    }
}
