namespace SlagFieldManagement.Domain.ValueObjects;

public record SlagWeight(decimal Value)
{
    public static SlagWeight Zero => new(0);
    public static SlagWeight FromKilograms(decimal kilograms) => new(kilograms);

    public override string ToString() => $"{Value} kg";
}