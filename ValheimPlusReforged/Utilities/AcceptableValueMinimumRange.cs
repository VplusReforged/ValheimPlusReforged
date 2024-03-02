using System;
using BepInEx.Configuration;

namespace ValheimPlusReforged.Utilities;

public class AcceptableValueMinimumRange<T> : AcceptableValueBase where T : IComparable
{
    public AcceptableValueMinimumRange(T minValue) : base(typeof(T))
    {
        _minValue = Objects.RequireNotNull(minValue);
    }

    private T _minValue;

    public override object Clamp(object value) => _minValue.CompareTo(value) > 0 ? _minValue : value;
    public override bool IsValid(object value) => _minValue.CompareTo(value) <= 0;
    public override string ToDescriptionString() => $"# Acceptable value range: Greater than {_minValue}";
}
