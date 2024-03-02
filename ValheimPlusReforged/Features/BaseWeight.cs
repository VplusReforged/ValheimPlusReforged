using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using ValheimPlusReforged.Utilities;

namespace ValheimPlusReforged.Features;

internal class BaseWeight : LiveSingleValueUpdaterDelegate<Player, float>
{
    internal BaseWeight() : base(featureName: "base weight") { }

    protected override ConfigEntry<float> ConfigEntry() => ValheimPlusReforged.Config.Bind(
        section: "Player",
        key: "BasePlayerWeight",
        defaultValue: 300f,
        configDescription: new ConfigDescription(
            description: "The base carry weight of your character. [Synced with Server]",
            acceptableValues: new AcceptableValueMinimumRange<float>(0f),
            tags: ValheimPlusReforged.AdminConfig
        )
    );

    protected override void Update(Player instance, float value)
    {
        if (FloatingPoint.FuzzyEquals(instance.m_maxCarryWeight, value)) return;
        Logger.LogDebug($"Updating carry weight from {instance.m_maxCarryWeight} to {value}!");
        instance.m_maxCarryWeight = value;
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.Awake))]
public static class BaseWeightPatch
{
    private static readonly LiveSingleValueUpdaterDelegate<Player, float> Delegate = new BaseWeight();

    [HarmonyPrepare]
    [UsedImplicitly]
    private static void Prepare(MethodBase original) => Delegate.Prepare(original);

    [HarmonyPostfix]
    [UsedImplicitly]
    private static void Postfix(ref Player __instance) => Delegate.UpdateInstance(ref __instance);
}
