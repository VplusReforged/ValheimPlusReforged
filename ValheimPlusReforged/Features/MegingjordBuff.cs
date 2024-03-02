using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using ValheimPlusReforged.Utilities;

namespace ValheimPlusReforged.Features;

// todo looks like the patch location isn't sufficient for live updating the value.
//       there may be multiple SE_Stats instances.
internal class MegingjordBuff : LiveSingleValueUpdaterDelegate<SE_Stats, float>
{
    internal MegingjordBuff() : base(featureName: "megingjord buff") { }

    protected override ConfigEntry<float> ConfigEntry() => ValheimPlusReforged.Config.Bind(
        section: "Player",
        key: "BaseMegingjordBuff",
        defaultValue: 150f,
        configDescription: new ConfigDescription(
            description: "The base carry weight buff of the Megingjord. [Synced with Server]",
            acceptableValues: new AcceptableValueMinimumRange<float>(0f),
            tags: ValheimPlusReforged.AdminConfig
        )
    );

    protected override void Update(SE_Stats instance, float value)
    {
        if (FloatingPoint.FuzzyEquals(instance.m_addMaxCarryWeight, value)) return;
        Logger.LogDebug($"Updating megingjord buff from {instance.m_addMaxCarryWeight} to {value}!");
        instance.m_addMaxCarryWeight = value;
    }
}

[HarmonyPatch(typeof(SE_Stats), nameof(SE_Stats.Setup))]
public static class MegingjordBuffPatch
{
    private static readonly LiveSingleValueUpdaterDelegate<SE_Stats, float> Delegate = new MegingjordBuff();

    [HarmonyPrepare]
    [UsedImplicitly]
    private static void Prepare(MethodBase original) => Delegate.Prepare(original);

    [HarmonyPostfix]
    [UsedImplicitly]
    private static void Postfix(ref SE_Stats __instance) => Delegate.UpdateInstance(ref __instance);
}
