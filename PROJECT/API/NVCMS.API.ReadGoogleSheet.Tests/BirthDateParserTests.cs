using NVCMS.API.ReadGoogleSheet.Common;
using Xunit;

namespace NVCMS.API.ReadGoogleSheet.Tests;

public class BirthDateParserTests
{
    private static readonly DateTime Today = new(2026, 11, 11);

    [Theory]
    [InlineData("05/03/2008", 2008, 3, 5)]   // dd/MM/yyyy: ngày 5 tháng 3
    [InlineData("5/3/2008", 2008, 3, 5)]
    [InlineData("25/12/2007", 2007, 12, 25)]
    [InlineData(" 25/12/2007 ", 2007, 12, 25)]
    [InlineData("25-12-2007", 2007, 12, 25)]
    [InlineData("25.12.2007", 2007, 12, 25)]
    [InlineData("25/12/2007 00:00:00", 2007, 12, 25)]
    [InlineData("2007-12-25", 2007, 12, 25)]
    [InlineData("2007-12-25T00:00:00", 2007, 12, 25)]
    [InlineData("39441", 2007, 12, 25)]      // serial ngày của Google Sheet
    public void Parse_ValidDayFirst(string input, int y, int m, int d)
        => Assert.Equal(new DateTime(y, m, d), BirthDateParser.Parse(input, Today));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12/25/2007")]   // kiểu Mỹ MM/dd: tháng 25 không tồn tại
    [InlineData("31/02/2008")]   // ngày không tồn tại
    [InlineData("abc")]
    [InlineData("05/03/08")]     // năm 2 chữ số: không đoán
    [InlineData("2008")]         // chỉ có năm: không đoán ngày
    [InlineData("01/01/1890")]   // trước 1900
    [InlineData("12/11/2026")]   // sau hôm nay
    public void Parse_Invalid_ReturnsNull(string? input)
        => Assert.Null(BirthDateParser.Parse(input, Today));
}
