using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface IEventsService
{
    /// <summary>Lay danh sach category dang hoat dong (FromDate &lt;= now &lt;= EndDate), kem su kien.</summary>
    Task<IEnumerable<EventsCatViewModel>> GetActiveCatsWithEventsAsync(int portalid);

    /// <summary>Lay tat ca category kem su kien.</summary>
    Task<IEnumerable<EventsCatViewModel>> GetAllCatsWithEventsAsync();

    /// <summary>Lay 1 category kem su kien theo id.</summary>
    Task<EventsCatViewModel?> GetCatWithEventsAsync(int catId, int portalid);

    /// <summary>Lay danh sach category da ket thuc (EndDate &lt; now).</summary>
    Task<IEnumerable<EventsCatViewModel>> GetPastCatsWithEventsAsync(int catId);

    /// <summary>Lay danh sach category da ket thuc phan trang (cho infinite scroll).</summary>
    Task<(IEnumerable<EventsCatViewModel> Items, int Total)> GetPastCatsPagedAsync(int portalid, int page, int pageSize);
}
