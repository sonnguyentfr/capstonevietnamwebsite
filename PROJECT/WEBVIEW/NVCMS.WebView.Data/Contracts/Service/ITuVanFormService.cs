using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface ITuVanFormService
{
    Task SubmitAsync(TuVanFormInputViewModel input, int portalId, CancellationToken ct = default);
}