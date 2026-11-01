using Capstone.Content.Models;

namespace Capstone.Content;

/// <summary>
/// Everything the EN site reads from or sends to the backend.
/// Pages and components only depend on this interface, so the data source
/// (real API or MockData) is switched in configuration: ContentApi:UseMock.
/// </summary>
public interface IContentApi
{
    Task<SiteDto> GetSiteAsync(CancellationToken ct = default);
    Task<HomeContentDto> GetHomeAsync(CancellationToken ct = default);
    Task<PageDto?> GetPageAsync(string slug, CancellationToken ct = default);

    Task<IReadOnlyList<CountryDto>> GetCountriesAsync(CancellationToken ct = default);
    Task<PagedResult<SchoolDto>> GetSchoolsAsync(SchoolQuery query, CancellationToken ct = default);
    Task<SchoolDetailDto?> GetSchoolAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<ServiceDto>> GetServicesAsync(CancellationToken ct = default);
    Task<ServiceDto?> GetServiceAsync(string slug, CancellationToken ct = default);

    Task<PagedResult<EventDto>> GetEventsAsync(EventQuery query, CancellationToken ct = default);
    Task<EventDetailDto?> GetEventAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<CategoryDto>> GetNewsCategoriesAsync(CancellationToken ct = default);
    Task<PagedResult<NewsDto>> GetNewsAsync(NewsQuery query, CancellationToken ct = default);
    Task<NewsDetailDto?> GetNewsArticleAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<TestimonialDto>> GetTestimonialsAsync(int take = 4, CancellationToken ct = default);

    Task<IReadOnlyList<DestinationDto>> GetDestinationsAsync(CancellationToken ct = default);
    Task<DestinationDto?> GetDestinationAsync(string slug, CancellationToken ct = default);

    Task<IReadOnlyList<TeamMemberDto>> GetTeamAsync(CancellationToken ct = default);

    /// <summary>Pages of one section (e.g. "about"), ordered, without body.</summary>
    Task<IReadOnlyList<PageDto>> GetPagesAsync(string section, CancellationToken ct = default);

    Task<LeadResult> SubmitLeadAsync(LeadRequest request, CancellationToken ct = default);
}
