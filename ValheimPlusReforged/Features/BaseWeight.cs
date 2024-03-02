using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn;
using Jotunn.Managers;
using ValheimPlusReforged.Utilities;

namespace ValheimPlusReforged.Features;

[HarmonyPatch(typeof(Player), nameof(Player.Awake))]
public static class BaseWeight
{
    private static ConfigEntry<float> _configEntry;
    private static Player _playerInstance;

    [HarmonyPrepare]
    [UsedImplicitly]
    private static void Prepare(MethodBase original)
    {
        if (original != null) return; // only call once
        _configEntry = ValheimPlusReforged.Config.Bind(
            section: "Player",
            key: "BasePlayerWeight",
            defaultValue: 300f,
            configDescription: new ConfigDescription(
                // todo only append sync info if not single player.
                description: "The base carry weight of your character. [Synced with Server]",
                acceptableValues: new AcceptableValueMinimumRange<float>(0f),
                tags: ValheimPlusReforged.AdminConfig
            )
        );
        SynchronizationManager.OnConfigurationWindowClosed += UpdateValue;
        SynchronizationManager.OnConfigurationSynchronized += (_, _) => UpdateValue();
    }

    [HarmonyPostfix]
    [UsedImplicitly]
    private static void Postfix(ref Player __instance)
    {
        Logger.LogDebug("Setting player instance");
        _playerInstance = __instance;
        UpdateValue();
    }

    private static void UpdateValue()
    {
        if (_playerInstance == null)
        {
            Logger.LogDebug($"Player instance not set, can't update carry weight.");
            return;
        }

        if (FloatingPoint.FuzzyEquals(_playerInstance.m_maxCarryWeight, _configEntry.Value)) return;
        Logger.LogDebug($"Updating carry weight from {_playerInstance.m_maxCarryWeight} to {_configEntry.Value}!");
        _playerInstance.m_maxCarryWeight = _configEntry.Value;
    }
}
