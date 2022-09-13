namespace Models;

public class OwnerFlat
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int FlatId { get; set; }
    public decimal Square { get; set; }
}