using System;

namespace ValheimPlusReforged.Utilities;

public static class FloatingPoint
{
    private const float Tolerance = 0.0001f;

    public static bool fuzzyEquals(float f1, float f2, float tolerance = Tolerance)
    {
        return Math.Abs(f1 - f2) < tolerance;
    }
}
