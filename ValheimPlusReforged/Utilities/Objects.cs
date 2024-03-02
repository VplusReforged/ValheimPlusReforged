using System;

namespace ValheimPlusReforged.Utilities;

public static class Objects
{
    public static T RequireNotNull<T>(T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        return obj;
    }
}
