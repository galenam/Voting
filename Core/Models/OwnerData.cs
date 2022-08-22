namespace Models;

public class OwnerData
{
    public int FlatNumber { get; private set; }
    public decimal FlatSquare { get; private set; }

    public LivingQuater LivingQuaterType { get; private set; }
    public FlatType TypeOfFlat { get; private set; }
    public string Name { get; private set; }
    public decimal SquareOfPart { get; private set; }
    public decimal PercentOfTheWholeHouse { get; private set; }
}