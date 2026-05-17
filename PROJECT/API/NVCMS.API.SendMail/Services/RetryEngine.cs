using System;
using NVCMS.API.SendMail.Interfaces;
namespace NVCMS.API.SendMail.Services
{
    public class ExponentialBackoffRetryEngine : IRetryEngine
    {
        private static readonly TimeSpan[] Schedule =
        {
            TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5),
            TimeSpan.FromMinutes(15), TimeSpan.FromHours(1), TimeSpan.FromHours(6)
        };
        public DateTime GetNextRetryTime(int retryCount)
            => DateTime.UtcNow.Add(Schedule[Math.Min(retryCount, Schedule.Length - 1)]);
        public bool ShouldRetry(int retryCount, int max) => retryCount < max;
    }
}
