using Capstone.View.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;
using System.Net;

namespace Capstone.View.Controllers;

public class LandingPageController : Controller
{
    private readonly ILadingPageService _service;
    private readonly IOptions<SiteSettings> _siteSettings;

    public LandingPageController(
        ILadingPageService service,
        IOptions<SiteSettings> siteSettings)
    {
        _service = service;
        _siteSettings = siteSettings;
    }

    [HttpGet("/landing-page")]
    public async Task<IActionResult> Index()
    {
        var portalId = _siteSettings.Value.PortalId;
        var all = await _service.GetAllAsync(portalId);

        // Nhóm theo Parent (ParentId = 0) và lấy children tương ứng
        var groups = all
            .Where(x => x.ParentId == 0)
            .Select(parent => new LandingPageGroupViewModel
            {
                Parent = parent,
                Children = all.Where(c => c.ParentId == parent.Id).ToList()
            })
            .ToList();

        return View(groups);
    }

    [HttpGet("/landing-page/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        var portalId = _siteSettings.Value.PortalId;

        var id = SlugHelper.ExtractIdFromSlug(slug);
        if (id == null)
            return NotFound();

        var record = await _service.GetByIdAsync(id.Value, portalId);
        if (record == null)
            return NotFound();

        // Generate correct slug from title
        var correctSlug = SlugHelper.ToSlug(record.TrangDanhMuc);
        var currentSlugPart = SlugHelper.RemoveIdSuffix(slug, id.Value);
        record.Noidung =  WebUtility.HtmlDecode(record.Noidung);
        // Redirect if slug is incorrect (SEO: avoid duplicate content)
        if (!string.Equals(currentSlugPart, correctSlug, StringComparison.OrdinalIgnoreCase))
        {
            return RedirectPermanent($"/landing-page/{correctSlug}-{record.Id}");
        }

        // Branch logic based on ParentId
        if (record.ParentId == 0)
        {
            // Parent record: show children list
            var children = await _service.GetAllByParentIdAsync(record.Id, portalId);
            return View("Index", children);
        }
        else
        {
            // Child record: show detail HTML
            return View("Detail", record);
        }
    }
}
