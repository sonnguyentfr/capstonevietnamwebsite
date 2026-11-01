using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Capstone.View.Options;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

public class EventsViewComponent : ViewComponent
{
    private readonly IEventsService _eventsService;
    private readonly int _portalCrmId;

    public EventsViewComponent(IEventsService eventsService, IOptions<SiteSettings> siteOptions)
    {
        _eventsService = eventsService;
        _portalCrmId = siteOptions.Value.PortalCRMId;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var cats = await _eventsService.GetActiveCatsWithEventsAsync(_portalCrmId);
        return View(cats);
    }
}
