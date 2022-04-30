using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models;
[Table(nameof(Function)),
 MessagePack.MessagePackObject(true)]
public class Function
{
    [Key] public Guid FunctionId { get; set; }
    public string? Name { get; set; }
    [Column("Name_CS")]
    public string? NameCaseSignature { get; set; }
    public string? AccessToken { get; set; }
    public string? Path { get; set; }
    [Column("Path_CS")]
    public string? PathCaseSignature { get; set; }
    public decimal Complexity { get; set; }
    public bool Enabled { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
}
