using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn;
using Jotunn.Utils;

namespace ValheimPlusReforged;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency(Main.ModGuid)]
[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
public class ValheimPlusReforged : BaseUnityPlugin
{
    private const string PluginGuid = "com.VPlusReforged.ValheimPlusReforged";
    private const string PluginName = "ValheimPlusReforged";
    private const string PluginVersion = "0.0.1";

    private const string HarmonyId = "mod.valheim_plus_reforged";
    private static readonly Harmony Harmony = new(HarmonyId);

    public static readonly ConfigurationManagerAttributes AdminConfig = new() { IsAdminOnly = true };
    public static readonly ConfigurationManagerAttributes ClientConfig = new() { IsAdminOnly = false };

    public new static ConfigFile Config { get; private set; }

    private void Awake()
    {
        Jotunn.Logger.LogInfo("Loading Valheim Plus Reforged");
        Config = base.Config;
        Harmony.PatchAll();
        Jotunn.Logger.LogInfo("Valheim Plus Reforged Loaded");
    }
}
