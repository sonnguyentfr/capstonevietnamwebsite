using System.Text.Json.Serialization;

namespace NVCMS.API.ReadGoogleSheet.Models;

public class ZnsTemplateButton
{
    public long Id { get; set; }
    public long ZnsTemplateId { get; set; }
    public int ButtonType { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public ZnsTemplate? Template { get; set; }
}
