using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface IEventsRepository
{
    /// <summary>Lay danh sach category chua het han (FromDate &lt;= now &lt;= EndDate).</summary>
    Task<IEnumerable<EventsCatModel>> GetActiveCatsAsync(int portalId);

    /// <summary>Lay tat ca category.</summary>
    Task<IEnumerable<EventsCatModel>> GetAllCatsAsync();

    /// <summary>Lay category theo id.</summary>
    Task<EventsCatModel?> GetCatByIdAsync(int id);

    /// <summary>Lay danh sach su kien theo catId.</summary>
    Task<IEnumerable<EventsModel>> GetEventsByCatAsync(int catId, int portalId);

    /// <summary>Lay su kien theo id.</summary>
    Task<EventsModel?> GetEventByIdAsync(int id);

    /// <summary>Lay category da ket thuc (EndDate &lt; now).</summary>
    Task<IEnumerable<EventsCatModel>> GetPastCatsAsync(int portalid);

    /// <summary>Lay category da ket thuc phan trang.</summary>
    Task<(IEnumerable<EventsCatModel> Items, int Total)> GetPastCatsPagedAsync(int portalid, int page, int pageSize);
}
