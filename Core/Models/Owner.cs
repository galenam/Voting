using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("owner")]
public class Owner
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }
    public OwnerFlat Flat { get; set; }
}

