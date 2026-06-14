using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Capstone.View.Options;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

public class EventsViewComponent : ViewComponent
{
    private readonly IEventsService _eventsService;

    public EventsViewComponent(IEventsService eventsService)
    {
        _eventsService = eventsService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var cats = await _eventsService.GetActiveCatsWithEventsAsync(50);
        return View(cats);
    }
}
