using System.Text.Json.Serialization;

namespace NVCMS.API.ReadGoogleSheet.Models;

public class ZnsTemplateParam
{
    public long Id { get; set; }
    public long ZnsTemplateId { get; set; }
    public string ParamName { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public string ParamType { get; set; } = "STRING";
    public int? MaxLength { get; set; }
    public int? MinLength { get; set; }
    public bool AcceptNull { get; set; }
    public int SortOrder { get; set; }
    public string? DisplayName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public ZnsTemplate? Template { get; set; }
}
