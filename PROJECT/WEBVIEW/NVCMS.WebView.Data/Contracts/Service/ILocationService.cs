using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface ILocationService
{
    /// <summary>Returns all provinces/states that are direct children of the given parentId.</summary>
    Task<IReadOnlyList<CapLocationModel>> GetProvincesAsync(int parentId);
}
