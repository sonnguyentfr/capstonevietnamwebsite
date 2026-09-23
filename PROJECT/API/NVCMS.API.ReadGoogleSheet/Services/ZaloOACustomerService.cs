using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using NVCMS.API.ReadGoogleSheet.Repositories;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    /// <summary>Tạo/cập nhật khách Zalo + đồng bộ hồ sơ từ API user/detail (dùng chung cho webhook, lịch sử, API).</summary>
    public interface IZaloOACustomerService
    {
        /// <summary>Upsert khách; đồng bộ hồ sơ nếu khách mới hoặc hồ sơ cũ hơn ProfileRefreshHours.</summary>
        Task<ZaloOACustomer> EnsureCustomerAsync(string oaId, string zaloUserId, string? zaloUserIdByApp, DateTime? interactionAt,
            string? displayName = null, string? avatarUrl = null, bool syncProfile = true, CancellationToken cancellationToken = default);

        /// <summary>Gọi Zalo user/detail và lưu. Không ném lỗi Zalo - trả kết quả để caller quyết định.</summary>
        Task<ZaloOAApiResult<ZaloOAUserDetail>> SyncProfileAsync(ZaloOACustomer customer, CancellationToken cancellationToken = default);
    }

    public class ZaloOACustomerService : IZaloOACustomerService
    {
        private readonly IZaloOAChatRepository _repo;
        private readonly IZaloOAClient _client;
        private readonly ZaloOAChatSettings _settings;
        private readonly ILogger<ZaloOACustomerService> _logger;

        public ZaloOACustomerService(IZaloOAChatRepository repo, IZaloOAClient client,
            IOptions<ZaloOAChatSettings> settings, ILogger<ZaloOACustomerService> logger)
        {
            _repo = repo;
            _client = client;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<ZaloOACustomer> EnsureCustomerAsync(string oaId, string zaloUserId, string? zaloUserIdByApp, DateTime? interactionAt,
            string? displayName = null, string? avatarUrl = null, bool syncProfile = true, CancellationToken cancellationToken = default)
        {
            var customer = await _repo.UpsertCustomerAsync(oaId, zaloUserId, zaloUserIdByApp, interactionAt, displayName, avatarUrl);

            if (syncProfile && NeedsProfileSync(customer))
                await SyncProfileAsync(customer, cancellationToken);

            return customer;
        }

        public async Task<ZaloOAApiResult<ZaloOAUserDetail>> SyncProfileAsync(ZaloOACustomer customer, CancellationToken cancellationToken = default)
        {
            var result = await _client.GetUserDetailAsync(customer.ZaloUserId, cancellationToken);
            if (result.Success && result.Data != null)
            {
                await _repo.UpdateCustomerProfileAsync(customer.Id, result.Data);
                customer.DisplayName = result.Data.DisplayName ?? customer.DisplayName;
                customer.AvatarUrl = result.Data.Avatar ?? customer.AvatarUrl;
            }
            else
            {
                _logger.LogWarning("Zalo OA: không lấy được hồ sơ khách CustomerId={CustomerId} ErrorCode={ErrorCode} Message={Message}",
                    customer.Id, result.ErrorCode, result.ErrorMessage);
            }
            return result;
        }

        private bool NeedsProfileSync(ZaloOACustomer c)
            => c.IsNew
               || c.LastProfileSyncAt == null
               || c.LastProfileSyncAt.Value < DateTime.UtcNow.AddHours(-Math.Max(1, _settings.ProfileRefreshHours));
    }
}
