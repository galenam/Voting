namespace Models;

public class OwnerFlat
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int FlatNumber { get; set; }
    public decimal FlatSquare { get; set; }
    public FlatType TypeOfFlat { get; set; }
}