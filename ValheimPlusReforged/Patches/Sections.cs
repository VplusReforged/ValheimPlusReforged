using System.Collections.Generic;
using System.Collections.ObjectModel;
using ValheimPlusReforged.Patches.Features;

namespace ValheimPlusReforged.Patches;

public class Sections
{
    public Sections(ValheimPlusReforged plugin)
    {
        Features = new ReadOnlyCollection<Feature>(
            new List<Feature>
            {
                // Player
                new BaseWeight(plugin),
            }
        );
    }

    public readonly IList<Feature> Features;
}
