using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.ViewModels;

public class EventRegistrationPageViewModel
{
    public EventsCatViewModel Cat   { get; set; } = new();
    public EventsViewModel    Event { get; set; } = new();

    /// <summary>Pre-selected eventId from query-string (entry point 1).</summary>
    public int PreselectedEventId { get; set; }

    public EventRegistrationInputViewModel Input { get; set; } = new();

    /// <summary>Provinces loaded from Cap_Location (parentId = 82 = Vietnam).</summary>
    public IReadOnlyList<CapLocationModel> Provinces { get; set; } = [];
}
