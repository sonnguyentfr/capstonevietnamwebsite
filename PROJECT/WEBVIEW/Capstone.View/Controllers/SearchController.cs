using Microsoft.AspNetCore.Mvc;

namespace Capstone.View.Controllers;

public class SearchController : Controller
{
    private readonly string _googleCx;

    public SearchController(IConfiguration config)
    {
        _googleCx = config["Google:cx"] ?? string.Empty;
    }

    // GET /tim-kiem?q=...
    public IActionResult Index(string? q)
    {
        var query = q?.Trim() ?? string.Empty;

        ViewData["Title"]        = string.IsNullOrEmpty(query)
            ? "Tìm kiếm"
            : $"Kết quả tìm kiếm: {query}";
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/tim-kiem";
        ViewData["GoogleCx"]     = _googleCx;
        ViewData["Query"]        = query;

        return View();
    }
}
