using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Capstone.View.Options;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.Controllers;

/// <summary>
/// Route: /thong-tin-du-hoc/{countrySlug}/danh-sach-truong
/// Reuse TruongSearchResultViewModel + QuocGia view.
/// </summary>
public class ThongTinDuHocController : Controller
{
    private readonly ITruongService _truongService;

    private static readonly Dictionary<string, (int Id, string Ten)> CountryMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "du-hoc-my",          (38,  "Mỹ") },
            { "du-hoc-canada",      (3,   "Canada") },
            { "du-hoc-uc",          (1,   "Úc") },
            { "du-hoc-anh",         (28,  "Anh") },
            { "du-hoc-ireland",     (401, "Ireland") },
            { "du-hoc-thuy-si",     (23,  "Thụy Sĩ") },
            { "du-hoc-new-zealand", (99,  "New Zealand") },
        };

    private static readonly Dictionary<int, string> TruongSlugMap = new()
    {
        { 38,  "my" },
        { 3,   "canada" },
        { 1,   "uc" },
        { 28,  "anh" },
        { 401, "ireland" },
        { 23,  "thuy-si" },
        { 99,  "new-zealand" },
    };

    public ThongTinDuHocController(ITruongService truongService, IOptions<SiteSettings> settings)
    {
        _truongService = truongService;
    }

    // GET /thong-tin-du-hoc/{countrySlug}/danh-sach-truong
    public async Task<IActionResult> DanhSachTruong(string countrySlug, TruongSearchFilterViewModel filter)
    {
        if (!CountryMap.TryGetValue(countrySlug, out var country))
            return NotFound();

        filter.QuocGia   = country.Id;
        filter.IsPartner = true;
        if (filter.PageSize <= 0) filter.PageSize = 12;

        var vm = await _truongService.SearchAsync(filter);

        ViewData["Title"]   = $"Danh sách trường du học {country.Ten}";
        ViewBag.CountryName = country.Ten;
        ViewBag.CountrySlug = TruongSlugMap.TryGetValue(country.Id, out var ts) ? ts : countrySlug;
        ViewBag.CountryId   = country.Id;
        ViewBag.BaseAction  = $"/thong-tin-du-hoc/{countrySlug}/danh-sach-truong";

        return View("~/Views/Truong/QuocGia.cshtml", vm);
    }
}
