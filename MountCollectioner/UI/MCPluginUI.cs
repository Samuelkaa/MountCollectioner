using Dalamud.Game;
using Dalamud.Utility;

using ImGuiNET;
using ImGuiScene;

using Lumina.Excel.GeneratedSheets;

using MountCollectioner.Requests;
using MountCollectioner.Serialize;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

using NetStone;
using Dalamud.Logging;
using System.Timers;

namespace MountCollectioner
{
    class MCPluginUI : IDisposable
    {
        private MCPluginConf configuration;

        private bool visible = false;
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        private bool settingsVisible = false;
        public bool SettingsVisible
        {
            get { return this.settingsVisible; }
            set { this.settingsVisible = value; }
        }

        private MountDataResponse mountsDataResponse;
        private IEnumerable<Mount> mounts;
        private Mount selectedMount;
        private TextureWrap selectedMountIcon;
        private List<Mount> sortedMounts;

        public string searchFieldString = String.Empty;
        private string lastSearchFieldString;
        private List<Mount> searchSortedMounts;

        private List<TextureWrap> icons = new List<TextureWrap>();

        private ImFontPtr fontPtr;
        private uint characterId;

        private LodestoneClient lodestoneClient;
        private string localPlayerName;
        private string localPlayerWorld;

        public bool loading;
        private readonly System.Timers.Timer timer = new () {Interval = 1000};
        public const int CooldownTime = 10;
        public int CurrentCooldown = 0;


        public MCPluginUI(MCPluginConf configuration, List<TextureWrap> icons)
        {
            timer.Elapsed += delegate
            {
                CurrentCooldown -= 1;
                if (CurrentCooldown <= 0)
                {
                    timer.Stop();
                }
            };

            this.configuration = configuration;
            this.icons = icons;
            mounts = MCPlugin.Data.GetExcelSheet<Mount>();
            this.sortedMounts = SortMounts();
            MCPlugin.PluginInterface.UiBuilder.BuildFonts += this.BuildFonts;
            MCPlugin.Framework.Update += HandleCharacterInfo;
        }

        public void Dispose()
        {
            MCPlugin.PluginInterface.UiBuilder.BuildFonts -= this.BuildFonts;
            MCPlugin.Framework.Update -= HandleCharacterInfo;
            GC.SuppressFinalize(this);
        }

        public void Draw()
        {
            DrawMainWindow();
        }

        public void DrawMainWindow()
        {
            if (!Visible)
            {
                return;
            }

            if (this.searchFieldString != this.lastSearchFieldString)
            {
                if (!string.IsNullOrEmpty(this.searchFieldString))
                {
                    searchSortedMounts = sortedMounts.Where(m => m.Singular.ToString().ToUpperInvariant().Contains(searchFieldString.ToUpperInvariant())).ToList();
                }
                else
                {
                    this.searchSortedMounts = this.sortedMounts.ToList();
                }

                lastSearchFieldString = this.searchFieldString;
            }

            if (sortedMounts == null)
            {
                sortedMounts = SortMounts();
            }

            ImGui.SetNextWindowSize(new Vector2(1000, 640), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(800, 600), new Vector2(1920, 1080));
            if (ImGui.Begin("Mount Collectioner", ref this.visible, ImGuiWindowFlags.NoScrollbar))
            {
                ImGui.BeginChild("leftMenu", new Vector2(ImGui.GetWindowSize().X / 3, ImGui.GetWindowSize().Y - 40), true);

                ImGui.SetNextItemWidth(ImGui.GetItemRectSize().X - 15);
                ImGui.InputTextWithHint("", "Search mount by name", ref searchFieldString, 256);

                ImGui.Separator();

                if (ImGui.Checkbox("Hide collected mounts", ref this.configuration.hideCollectedMounts))
                {

                }
                else
                {
                    ImGui.Separator();

                    foreach (var mount in searchSortedMounts)
                    {
                        if (mount.Singular != String.Empty)
                        {
                            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                            if (ImGui.Selectable(ti.ToTitleCase(mount.Singular) + $" {mount.RowId}", selectedMount == mount))
                            {
                                ChangeSelectedMount(mount.RowId);
                            }
                            ImGui.SameLine();
                            ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 5);
                            ImGui.SetCursorPosX(ImGui.GetCursorPosX() - 5);
                            ImGui.Image(icons[1].ImGuiHandle, new Vector2(10, 10));
                        }
                    }

                    ImGui.EndChild();
                }

                ImGui.SameLine();
                if (selectedMount != null && mountsDataResponse != null)
                {
                    ImGui.BeginChild("item", new Vector2(ImGui.GetWindowSize().X / 2 + 180, ImGui.GetWindowSize().Y - 40), true);
                    ImGui.Image(selectedMountIcon.ImGuiHandle, new Vector2(60, 60));

                    ImGui.SameLine();
                    ImGui.SetCursorPosY(30);
                    ImGui.PushFont(fontPtr);
                    ImGui.Text($"{mountsDataResponse.Name} {configuration.CharacterId}");
                    ImGui.PopFont();

                    ImGui.SetCursorPosY(ImGui.GetCursorPosY() - 25);
                    ImGui.Text($"{mountsDataResponse.Movement}");

                    ImGui.Separator();

                    ImGui.TextWrapped($"{mountsDataResponse.EnchancedDescription}");

                    ImGui.Separator();

                    if (mountsDataResponse == null)
                    {
                        ImGui.TextWrapped($"Obtaining method: None");
                    }
                    else
                    {
                        ImGui.TextWrapped($"Obtaining method: {mountsDataResponse.Sources[0].Type}");
                    }
                    ImGui.Text($"Source: ");
                    ImGui.SameLine();
                    ImGui.TextWrapped($"{mountsDataResponse.Sources[0].Text}");

                    //ImGui.SetCursorPosX(ImGui.GetContentRegionMax().X / 2 - 100);
                    //ImGui.Image(MCPlugin.PluginInterface.UiBuilder.LoadImage(mountsDataResponse.Image).ImGuiHandle, new Vector2(200, 200));

                    ImGui.SetCursorPosY(ImGui.GetWindowContentRegionMax().Y - 20 - ImGui.GetTextLineHeightWithSpacing());
                    ImGui.Separator();
                    ImGui.SetCursorPosY(ImGui.GetWindowContentRegionMax().Y - 15 - ImGui.GetTextLineHeightWithSpacing());
                    if (ImGui.Button("Page on https://ffxiv.consolegameswiki.com"))
                    {
                        
                    }
                }

                ImGui.SetCursorPosY(ImGui.GetWindowContentRegionMax().Y + 10 - ImGui.GetTextLineHeightWithSpacing());
                ImGui.Text("Data provided by FFXIV Collect (https://ffxivcollect.com) and FFXIVAPI (https://xivapi.com)");
            }
            ImGui.End();
        }

        private void HandleCharacterInfo(Framework framework)
        {
            var lp = MCPlugin.ClientState.LocalPlayer;
            if (lp == null) return;

            this.localPlayerName = lp.Name.TextValue;
            this.localPlayerWorld = lp.HomeWorld.GameData.Name;
        }

        private unsafe void BuildFonts()
        {
            var fontPath = Path.Combine(MCPlugin.PluginInterface.DalamudAssetDirectory.FullName, "UIRes", "NotoSansCJKjp-Medium.otf");
            this.fontPtr = ImGui.GetIO().Fonts.AddFontFromFileTTF(fontPath, 24.0f);
        }

        private void ChangeSelectedMount(uint mountId)
        {
            selectedMount = mounts.Single(m => m.RowId == mountId);

            var iconId = selectedMount.Icon;
            var iconTexFile = MCPlugin.Data.GetIcon(iconId);
            this.selectedMountIcon?.Dispose();
            this.selectedMountIcon = MCPlugin.PluginInterface.UiBuilder.LoadImageRaw(iconTexFile.GetRgbaImageData(), iconTexFile.Header.Width, iconTexFile.Header.Height, 4);

            GetMount(mountId);
        }

        private List<Mount> SortMounts()
        {
            try
            {
                var mounts = MCPlugin.Data.GetExcelSheet<Mount>();
                var sortMounts = mounts.Where(m => m.Singular != String.Empty).OrderBy(m => m.Singular.ToString().ToUpperInvariant());
                var sortedMounts = new List<Mount>();

                foreach (var mount in sortMounts)
                {
                    sortedMounts.Add(mount);
                }

                return sortedMounts;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        private void GetCharacterId()
        {
            
        }

        private void GetMount(uint selectedMountId)
        {
            Task.Run(async () =>
            {
                this.mountsDataResponse = await FFXIVCollectRequest
                  .GetMountsData(selectedMountId, CancellationToken.None)
                  .ConfigureAwait(false);
            });
        }
    }
}
