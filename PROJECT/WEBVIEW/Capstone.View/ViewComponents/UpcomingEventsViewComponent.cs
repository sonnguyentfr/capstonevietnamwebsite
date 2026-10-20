using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

/// <summary>Box "Sự kiện sắp diễn ra" ở sidebar – hiển thị website, sắp xếp theo thời gian bắt đầu gần nhất.</summary>
public class UpcomingEventsViewComponent : ViewComponent
{
    private readonly IEventsService _eventsService;

    public UpcomingEventsViewComponent(IEventsService eventsService)
    {
        _eventsService = eventsService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int take = 5)
    {
        var activeCats = await _eventsService.GetActiveCatsWithEventsAsync(50);
        var upcomingEvents = activeCats
            .Where(c => c.Is_show_website)
            .OrderBy(c => c.Events.Any() ? c.Events.Min(e => e.Fromdatetime ?? DateTime.MaxValue)
                                         : (c.FromDate ?? DateTime.MaxValue))
            .Take(take)
            .ToList();

        return View(upcomingEvents);
    }
}
