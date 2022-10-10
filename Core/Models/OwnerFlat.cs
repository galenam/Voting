using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("owner_flat")]
public class OwnerFlat
{
    [Column("id")]
    public int Id { get; set; }
    [Column("owner_id")]
    public int OwnerId { get; set; }
    [Column("square")]
    public decimal SquareOfPart { get; set; }
    [Column("flat_id")]
    public int FlatId {get;set;}
}