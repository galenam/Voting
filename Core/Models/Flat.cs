using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("flat")]
public class Flat
{
    [Column("id")]
    public int Id { get; set; }
    [Column("number")]
    public int FlatNumber { get; set; }
    [Column("square")]
    public decimal FlatSquare { get; set; }
    [Column("type_id")]
    public FlatType TypeOfFlat { get; set; }
}