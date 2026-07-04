using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NVCMS.WebView.Data.Models;

[Table("Cap_Location")]
public class CapLocationModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Name")]
    public string? Name { get; set; }

    [Column("ShortName")]
    public string? ShortName { get; set; }

    [Column("ParentId")]
    public int? ParentId { get; set; }

    [Column("Status")]
    public bool? Status { get; set; }

    [Column("Ordernumber")]
    public int? Ordernumber { get; set; }

    [Column("PortalId")]
    public int? PortalId { get; set; }
}
