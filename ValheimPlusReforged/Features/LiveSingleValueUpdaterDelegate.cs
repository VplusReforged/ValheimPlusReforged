using BepInEx.Configuration;
using BepInEx.Logging;
using Jotunn.Managers;

namespace ValheimPlusReforged.Features;

public abstract class LiveSingleValueUpdaterDelegate<TI, TE>
{
    protected readonly ManualLogSource Logger;
    private readonly string _featureName;
    private TI _instance;
    private ConfigEntry<TE> _configEntry;

    protected LiveSingleValueUpdaterDelegate(string featureName)
    {
        _featureName = featureName;
        Logger = BepInEx.Logging.Logger.CreateLogSource(GetType().FullName);
    }

    protected abstract ConfigEntry<TE> ConfigEntry();
    protected abstract void Update(TI instance, TE value);

    public void Prepare(object original)
    {
        if (original != null) return; // only call once
        _configEntry = ConfigEntry();
        SynchronizationManager.OnConfigurationWindowClosed += UpdateValue;
        SynchronizationManager.OnConfigurationSynchronized += (_, _) => UpdateValue();
    }

    public void UpdateInstance(ref TI instance)
    {
        Logger.LogDebug($"Setting instance for {_featureName}");
        _instance = instance;
        UpdateValue();
    }

    private void UpdateValue()
    {
        if (_instance == null)
        {
            Logger.LogDebug($"Can't update {_featureName} because the instance isn't set.");
            return;
        }

        Update(_instance, _configEntry.Value);
    }
}
