namespace ValheimPlusReforged.Patches;

public abstract class Feature
{
    protected Feature(ValheimPlusReforged plugin)
    {
        Plugin = plugin;
    }

    protected readonly ValheimPlusReforged Plugin;

    public abstract void Awake();
    public abstract void Update();
}
