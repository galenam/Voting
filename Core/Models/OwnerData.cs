namespace Models;

public class OwnerData
{
    public int FlatNumber { get; set; }
    public decimal FlatSquare { get; set; }

    public LivingQuater LivingQuaterType { get; set; }
    public FlatType TypeOfFlat { get; set; }
    public string Name { get; set; }
    public decimal SquareOfPart { get; set; }
    public decimal PercentOfTheWholeHouse { get; set; }

    public override string ToString()
    {
        return @$"FlatNumber={FlatNumber} FlatSquare={FlatSquare} LivingQuaterType={LivingQuaterType} TypeOfFlat={TypeOfFlat} Name={Name} 
        SquareOfPart={SquareOfPart} PercentOfTheWholeHouse={PercentOfTheWholeHouse}";
    }
}