using NVCMS.API.ReadGoogleSheet.Common;
using Xunit;

namespace NVCMS.API.ReadGoogleSheet.Tests;

public class LadiConsultRequestTests
{
    [Theory]
    [InlineData("{\"yeu_cau_tu_van\":\"Muốn du học Mỹ ngành CNTT\"}", "Muốn du học Mỹ ngành CNTT")]
    [InlineData("{\"a\":\"1\",\"yeu_cau_tu_van\":\"  học bổng  \"}", "học bổng")]
    [InlineData("{\"YEU_CAU_TU_VAN\":\"x\"}", "x")]
    [InlineData("{\"yeu cau tu van\":\"x\"}", "x")]
    public void Extract_Found(string json, string expected)
        => Assert.Equal(expected, LadiConsultRequest.Extract(json));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("{}")]
    [InlineData("{\"yeu_cau_tu_van\":\"   \"}")]
    [InlineData("{\"yeu_cau_tu_van\":null}")]
    [InlineData("{\"khac\":\"x\"}")]
    [InlineData("không phải json")]
    public void Extract_NotFound_ReturnsNull(string? json)
        => Assert.Null(LadiConsultRequest.Extract(json));

    [Fact]
    public void NewStudent_WithRequest_AppendsEncodedRequest()
    {
        var s = LadiConsultRequest.BuildNewStudentTuVanKhac("TRIỂN LÃM", "Mỹ <b>& Úc</b>");
        Assert.Equal("<mark>Khách đăng ký Online tại: TRIỂN LÃM</mark>.<br/>Yêu cầu tư vấn: Mỹ &lt;b&gt;&amp; Úc&lt;/b&gt;", s);
    }

    [Fact]
    public void NewStudent_WithoutRequest_KeepsOldText()
        => Assert.Equal("<mark>Khách đăng ký Online tại: X</mark>.",
                        LadiConsultRequest.BuildNewStudentTuVanKhac("X", null));

    [Fact]
    public void ExistingStudentLine_HasDateAndSource()
        => Assert.Equal("[13/11/2026 - Ladi: X] Yêu cầu tư vấn: abc",
                        LadiConsultRequest.BuildExistingStudentLine("X", " abc ", new DateTime(2026, 11, 13)));

    [Fact]
    public void LongText_IsTruncatedTo1000()
        => Assert.Equal(LadiConsultRequest.TuVanKhacMaxLength,
                        LadiConsultRequest.BuildNewStudentTuVanKhac("X", new string('a', 2000)).Length);
}
