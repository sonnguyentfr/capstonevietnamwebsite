using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

/// <summary>
/// Hien thi danh sach su kien dang hoat dong (FromDate &lt;= Now &lt;= EndDate, is_show_website = true).
/// Su dung: @await Component.InvokeAsync("Events")
/// </summary>
public class EventsViewComponent : ViewComponent
{
    private readonly IEventsService _eventsService;
    private readonly int _portalId;

    public EventsViewComponent(IEventsService eventsService, IConfiguration config)
    {
        _eventsService = eventsService;
        _portalId = config.GetValue<int>("SiteSettings:PortalId");
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var cats = await _eventsService.GetActiveCatsWithEventsAsync(50);
        return View(cats);
    }
}
