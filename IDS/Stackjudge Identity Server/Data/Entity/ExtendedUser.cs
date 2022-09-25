using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stackjudge_Identity_Server.Data.Entity;

[Table("ExtendedUsers")]
public class ExtendedUser
{
    [Column("Id")]
    [Key]
    public int? Id { get; set; }
    [Column("ProfileUrl")] public string ProfileUrl { get; set; }
}