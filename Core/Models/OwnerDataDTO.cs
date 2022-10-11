namespace Models;

[OwnerDataValidationAttribure]
public class OwnerDataDTO
{
    public int FlatNumber { get; set; }
    private decimal flatSquare;
    public decimal FlatSquare { get{return flatSquare;}
    set{
        if (value > 100) {flatSquare = value /100;}
        else{
            flatSquare = value;
        }
    } }

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