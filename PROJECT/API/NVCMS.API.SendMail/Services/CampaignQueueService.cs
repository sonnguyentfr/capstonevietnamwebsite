using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NVCMS.API.SendMail.Config;
using NVCMS.API.SendMail.Domain;
using NVCMS.API.SendMail.Interfaces;
namespace NVCMS.API.SendMail.Services
{
    public class CampaignQueueService : ICampaignQueueService
    {
        private readonly ICampaignRepository    _camp;
        private readonly IMailQueueRepository   _queue;
        private readonly IUnsubscribeRepository _unsub;
        private static readonly Regex EmailRx = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public CampaignQueueService(ICampaignRepository camp,
            IMailQueueRepository queue, IUnsubscribeRepository unsub)
        { _camp = camp; _queue = queue; _unsub = unsub; }
        public async Task<long> EnqueueCampaignAsync(CreateCampaignRequest req, CancellationToken ct)
        {
            var campaign = new Campaign
            {
                Name=req.Name, Subject=req.Subject, HtmlContent=req.HtmlContent,
                FromEmail=req.FromEmail, FromName=req.FromName,
                ScheduledDate=req.ScheduledDate, Status=(int)CampaignStatus.Queued,
                TotalRecipients=req.Recipients.Count, CreatedDate=DateTime.UtcNow
            };
            var id = await _camp.CreateAsync(campaign, ct);
            var unsub = await _unsub.GetAllEmailsAsync(ct);
            var items = new List<MailQueueItem>();
            foreach (var r in req.Recipients)
            {
                if (!EmailRx.IsMatch((r.Email ?? "").Trim())) continue;
                if (unsub.Contains(r.Email)) continue;
                items.Add(new MailQueueItem
                {
                    CampaignId=id, RecipientId=r.Id, Email=r.Email,
                    Subject=req.Subject,
                    Body=req.HtmlContent
                        .Replace("{{FullName}}", r.FullName ?? r.Email)
                        .Replace("{{Email}}", r.Email)
                        + string.Format("<img src=\"{0}/api/sendmail/tracking/open/{1}_{2}\" width=\"1\" height=\"1\" style=\"display:none\" alt=\"\"/>",AppConfig.AppDomain,id,r.Id)
                        + string.Format("<p style=\"text-align:center;font-size:12px;color:#999\"><a href=\"{0}/api/sendmail/unsubscribe/{1}\">Unsubscribe</a></p>",AppConfig.AppDomain,Uri.EscapeDataString(r.Email)),
                    Status=MailQueueStatus.Pending, CreatedDate=DateTime.UtcNow
                });
            }
            await _queue.BulkInsertAsync(items, ct);
            return id;
        }
    }
}
