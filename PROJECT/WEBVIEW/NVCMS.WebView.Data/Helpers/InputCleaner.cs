using System.Text.RegularExpressions;
namespace Capstone.View.Helpers;
public static class InputCleaner
{
    public static string Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "";

        value = Regex.Replace(value, "<.*?>", "");

        value = value.Trim();

        if (!Regex.IsMatch(value,
            @"^[\p{L}\s.'-]{1,100}$"))
            throw new ArgumentException("Invalid name");

        return value;
    }

    public static string Text(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "";

        return Regex.Replace(value, "<.*?>", "").Trim();
    }

    public static string Email(string value)
    {
        return value?.Trim();
    }
}

