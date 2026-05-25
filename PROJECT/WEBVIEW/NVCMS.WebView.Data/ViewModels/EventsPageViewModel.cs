namespace NVCMS.WebView.Data.ViewModels;

/// <summary>ViewModel cho trang /su-kien</summary>
public class EventsPageViewModel
{
    public IEnumerable<EventsCatViewModel> Upcoming { get; set; } = [];
    public IEnumerable<EventsCatViewModel> Past      { get; set; } = [];
}
