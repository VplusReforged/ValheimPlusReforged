using BepInEx;
using HarmonyLib;
using Jotunn;
using Jotunn.Managers;
using Jotunn.Utils;
using ValheimPlusReforged.Patches;

namespace ValheimPlusReforged;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency(Main.ModGuid)]
[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
public class ValheimPlusReforged : BaseUnityPlugin
{
    private const string PluginGuid = "com.VPlusReforged.ValheimPlusReforged";
    private const string PluginName = "ValheimPlusReforged";
    private const string PluginVersion = "0.0.1";

    public const string HarmonyId = "mod.valheim_plus_reforged";

    public static readonly Harmony Harmony = new(HarmonyId);

    public readonly ConfigurationManagerAttributes AdminConfig = new() { IsAdminOnly = true };

    private Sections _sections;

    private void Awake()
    {
        Jotunn.Logger.LogInfo("Loading Valheim Plus Reforged");

        _sections = new Sections(this);

        foreach (var feature in _sections.Features)
        {
            feature.Awake();
        }

        foreach (var feature in _sections.Features)
        {
            feature.Update();
        }

        SynchronizationManager.OnConfigurationSynchronized += (_, attr) =>
        {
            Jotunn.Logger.LogDebug("Syncing Server Configs");
            UpdateFeatures();
        };

        SynchronizationManager.OnConfigurationWindowClosed += () =>
        {
            Jotunn.Logger.LogDebug("Syncing Local Configs");
            UpdateFeatures();
        };

        Jotunn.Logger.LogInfo("Valheim Plus Reforged Loaded");

        // To learn more about Jotunn's features, go to
        // https://valheim-modding.github.io/Jotunn/tutorials/overview.html
    }

    private void UpdateFeatures()
    {
        foreach (var feature in _sections.Features)
        {
            feature.Update();
        }
    }
}
