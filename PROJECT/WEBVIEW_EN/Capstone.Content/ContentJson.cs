using System.Text.Json;
using System.Text.Json.Serialization;

namespace Capstone.Content;

/// <summary>JSON settings shared by the API (writing) and the website (reading): camelCase, case-insensitive.</summary>
public static class ContentJson
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    /// <summary>Apply the same settings to ASP.NET Core's serializer (API side).</summary>
    public static void Configure(JsonSerializerOptions target)
    {
        target.PropertyNamingPolicy = Options.PropertyNamingPolicy;
        target.PropertyNameCaseInsensitive = true;
        target.DefaultIgnoreCondition = Options.DefaultIgnoreCondition;
    }
}
