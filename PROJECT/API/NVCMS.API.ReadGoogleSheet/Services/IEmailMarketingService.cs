using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public interface IEmailMarketingService
    {
        /// <summary>
        /// Tạo CampaignSend, sinh SendLog cho mỗi email, enqueue Hangfire.
        /// Trả ngay campaignSendId và totalRecipient.
        /// </summary>
        Task<SendCampaignResult> SendCampaignAsync(SendCampaignBodyRequest request);

        /// <summary>Tạo campaign mới với trạng thái Draft (luồng cũ).</summary>
        Task<CampaignStatusResponse> CreateCampaignAsync(CreateCampaignRequest request);

        /// <summary>Thêm danh sách email vào campaign (luồng cũ).</summary>
        Task<int> ImportRecipientsAsync(AddRecipientsRequest request);

        /// <summary>Đặt lịch gửi (luồng cũ).</summary>
        Task<CampaignStatusResponse> ScheduleCampaignAsync(int campaignId, DateTime scheduledAt);

        /// <summary>Tạm dừng campaign (luồng cũ).</summary>
        Task<CampaignStatusResponse> PauseCampaignAsync(int campaignId);

        /// <summary>Tiếp tục campaign (luồng cũ).</summary>
        Task<CampaignStatusResponse> ResumeCampaignAsync(int campaignId);

        /// <summary>Dừng hẳn campaign (luồng cũ).</summary>
        Task<CampaignStatusResponse> StopCampaignAsync(int campaignId);

        /// <summary>Thống kê (luồng cũ).</summary>
        Task<CampaignStatisticsResponse> GetStatisticsAsync(int campaignId);
    }
}
