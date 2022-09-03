namespace Models;

[OwnerDataValidationAttribure]
public class OwnerData
{
    public int FlatNumber { get; set; }
    public decimal FlatSquare { get; set; }

    public LivingQuater LivingQuaterType { get; set; }
    public FlatType TypeOfFlat { get; set; }
    public string Name { get; set; }
    public decimal SquareOfPart
    {
        get; set;
    }
    private decimal percentOfTheWholeHouse;
    public decimal PercentOfTheWholeHouse
    {
        get { return percentOfTheWholeHouse; }
        set
        {
            if (value >= 1)
            {
                percentOfTheWholeHouse = value - (int)value > 0 ? value - (int)value : 0;
            }
            else
            {
                percentOfTheWholeHouse = value;
            }
        }
    }

    public override string ToString()
    {
        return @$"FlatNumber={FlatNumber} FlatSquare={FlatSquare} LivingQuaterType={LivingQuaterType} TypeOfFlat={TypeOfFlat} Name={Name} 
        SquareOfPart={SquareOfPart} PercentOfTheWholeHouse={PercentOfTheWholeHouse}";
    }
}