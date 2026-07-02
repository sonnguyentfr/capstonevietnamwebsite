using System.Text.RegularExpressions;

namespace NVCMS.WebView.Data.Common;

public static class EmailHelper
{
    private static readonly Regex _regex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(1));

    public static bool IsValid(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return _regex.IsMatch(email.Trim());
    }
}
