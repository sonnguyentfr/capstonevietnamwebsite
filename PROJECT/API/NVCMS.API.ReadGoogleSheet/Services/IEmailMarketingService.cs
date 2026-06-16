using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public interface IEmailMarketingService
    {
        /// <summary>
        /// Nhận request từ DNN, load danh sách email, lọc unsub, insert Queued send-logs,
        /// enqueue Hangfire trực tiếp. Trả ngay campaignId và totalRecipient.
        /// </summary>
        Task<SendCampaignResult> SendCampaignAsync(SendCampaignBodyRequest request);

        /// <summary>Tạo campaign mới trong Marketing_Mail_Campaing.</summary>
        Task<CampaignStatusResponse> CreateCampaignAsync(CreateCampaignRequest request);

        /// <summary>Thống kê gửi mail theo campaignId dựa trên Marketing_Mail_Send_Log.</summary>
        Task<CampaignStatisticsResponse> GetStatisticsAsync(int campaignId);
    }
}
