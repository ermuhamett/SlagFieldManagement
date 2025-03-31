namespace SlagFieldManagement.Domain.ValueObjects;

public record SlapWeight(decimal Value)
{
    public static SlapWeight Zero => new(0);
    public static SlapWeight FromKilograms(decimal kilograms) => new(kilograms);

    public override string ToString() => $"{Value} kg";
}