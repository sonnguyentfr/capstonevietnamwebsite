namespace NVCMS.WebView.Data.Common;

public static class PhoneHelper
{
    /// <summary>Normalize any Vietnamese phone format to 84xxxxxxxxx.</summary>
    public static string Normalize(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return string.Empty;

        var p = phone.Trim()
            .Replace(" ", "")
            .Replace(".", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");

        if (p.StartsWith("+84"))
            p = "84" + p[3..];
        else if (p.StartsWith("0084"))
            p = "84" + p[4..];
        else if (p.StartsWith("84"))
            ;   // already normalized
        else if (p.StartsWith("0"))
            p = "84" + p[1..];
        else if (p.Length == 9)
            p = "84" + p;

        return p;
    }

    /// <summary>Returns true when phone normalizes to exactly 11 digits starting with 84.</summary>
    public static bool IsValid(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return false;
        var n = Normalize(phone);
        return n.Length == 11 && n.StartsWith("84") && n.All(char.IsDigit);
    }
}
