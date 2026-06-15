namespace NVCMS.API.ReadGoogleSheet.Models
{
    /// <summary>
    /// SNS notification envelope từ AWS SES
    /// </summary>
    public class SnsNotification
    {
        public string? Type { get; set; }
        public string? MessageId { get; set; }
        public string? TopicArn { get; set; }
        public string? Message { get; set; }        // JSON string chứa SesEventPayload
        public string? Timestamp { get; set; }
        public string? SubscribeURL { get; set; }   // dùng khi Type = SubscriptionConfirmation
    }

    /// <summary>
    /// SES event notification payload (deserialized từ SnsNotification.Message)
    /// </summary>
    public class SesEventPayload
    {
        public string? eventType { get; set; }      // Delivery | Bounce | Complaint | Open | Click
        public SesMail? mail { get; set; }
        public SesDelivery? delivery { get; set; }
        public SesBounce? bounce { get; set; }
        public SesComplaint? complaint { get; set; }
        public SesOpen? open { get; set; }
        public SesClick? click { get; set; }
    }

    public class SesMail
    {
        public string? messageId { get; set; }
        public string? source { get; set; }
        public List<string> destination { get; set; } = [];
    }

    public class SesDelivery
    {
        public string? timestamp { get; set; }
        public List<string> recipients { get; set; } = [];
    }

    public class SesBounce
    {
        public string? bounceType { get; set; }         // Permanent | Transient
        public string? bounceSubType { get; set; }
        public List<SesBounceRecipient> bouncedRecipients { get; set; } = [];
    }

    public class SesBounceRecipient
    {
        public string? emailAddress { get; set; }
        public string? diagnosticCode { get; set; }
    }

    public class SesComplaint
    {
        public List<SesComplaintRecipient> complainedRecipients { get; set; } = [];
        public string? complaintFeedbackType { get; set; }
    }

    public class SesComplaintRecipient
    {
        public string? emailAddress { get; set; }
    }

    public class SesOpen
    {
        public string? timestamp { get; set; }
        public string? ipAddress { get; set; }
        public string? userAgent { get; set; }
    }

    public class SesClick
    {
        public string? timestamp { get; set; }
        public string? ipAddress { get; set; }
        public string? userAgent { get; set; }
        public string? link { get; set; }
    }
}
