using NVCMS.Modules.Marketing;
namespace NVCMS.API.Marketing.Services.Marketing
{
    public class MarketingReportServices
    {
        public static DashboardResult GetDashboard(int campaignSendId)
        {
            var controller = new MarketingReportController();

            return controller.GetDashboard(campaignSendId);
        }
    //    public static List<DashboardStatus> GetStatus(int campaignId)
    //    {
    //        var controller = new MarketingReportController();

    //        return controller.GetStatus(campaignId);
    //    }
    //    public static List<DashboardTimeline> GetTimeline(int campaignId)
    //    {
    //        var controller = new MarketingReportController();

    //        return controller.GetTimeline(campaignId);
    //    }
    //    public static PagedResult<DashboardDetail> GetDetails(
    //int campaignId,
    //int page,
    //int pageSize,
    //string keyword,
    //string status)
    //    {
    //        var controller = new MarketingReportController();

    //        return controller.GetDetails(
    //            campaignId,
    //            page,
    //            pageSize,
    //            keyword,
    //            status);
    //    }
    //    public static void Refresh(int campaignId)
    //    {
    //        var controller = new MarketingReportController();

    //        controller.Refresh(campaignId);
    //    }
    }
}
