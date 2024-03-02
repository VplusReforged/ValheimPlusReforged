using System;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn;
using ValheimPlusReforged.Utilities;

namespace ValheimPlusReforged.Patches.Features;

public class BaseWeight : Feature
{
    private ConfigEntry<float> _configEntry;
    private Player _playerInstance;

    public BaseWeight(ValheimPlusReforged plugin) : base(plugin)
    {
    }

    public override void Awake()
    {
        _configEntry = Plugin.Config.Bind(
            section: "Player",
            key: "BasePlayerWeight",
            defaultValue: 300f,
            configDescription: new ConfigDescription(
                description: "The base carry weight of your character. [Synced with Server]",
                acceptableValues: new AcceptableValueMinimumRange<float>(0f),
                tags: Plugin.AdminConfig
            )
        );

        BaseWeightPatch.Action = player =>
        {
            Logger.LogDebug("Setting player instance");
            _playerInstance = player;
            UpdateValue();
        };

        ValheimPlusReforged.Harmony.Patch(
            original: AccessTools.Method(typeof(Player), nameof(Player.Awake)),
            postfix: new HarmonyMethod(AccessTools.Method(typeof(BaseWeightPatch), nameof(BaseWeightPatch.Postfix)))
        );
    }

    public override void Update()
    {
        if (_playerInstance == null)
        {
            Logger.LogDebug($"Player instance not set, can't update carry weight.");
            return;
        }

        UpdateValue();
    }


    private void UpdateValue()
    {
        if (FloatingPoint.fuzzyEquals(_playerInstance.m_maxCarryWeight, _configEntry.Value)) return;
        Logger.LogDebug($"Updating carry weight from {_playerInstance.m_maxCarryWeight} to {_configEntry.Value}!");
        _playerInstance.m_maxCarryWeight = _configEntry.Value;
    }

    private static class BaseWeightPatch
    {
        public static Action<Player> Action;

        public static void Postfix(ref Player __instance) => Action(__instance);
    }
}
