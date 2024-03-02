using System;

namespace ValheimPlusReforged.Utilities;

public static class FloatingPoint
{
    private const float Tolerance = 0.0001f;
    public static bool FuzzyEquals(float f1, float f2, float tolerance = Tolerance) => Math.Abs(f1 - f2) < tolerance;
}
