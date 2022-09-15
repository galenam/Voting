using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("owner")]
public class Owner
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    [Required]
    public string Name { get; set; }
}

