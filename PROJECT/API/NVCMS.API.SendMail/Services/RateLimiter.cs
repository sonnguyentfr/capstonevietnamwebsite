using System;
using System.Threading;
using System.Threading.Tasks;
using NVCMS.API.SendMail.Config;
using NVCMS.API.SendMail.Interfaces;
namespace NVCMS.API.SendMail.Services
{
    public class TokenBucketRateLimiter : IRateLimiter
    {
        private readonly int _perMin;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1,1);
        private int _tokens;
        private DateTime _lastRefill = DateTime.UtcNow;
        public TokenBucketRateLimiter() { _perMin = AppConfig.RateLimitPerMinute; _tokens = _perMin; }
        public async Task AcquireAsync(CancellationToken ct)
        {
            while (true)
            {
                await _lock.WaitAsync(ct);
                try { Refill(); if (_tokens > 0) { _tokens--; return; } }
                finally { _lock.Release(); }
                await Task.Delay(200, ct);
            }
        }
        private void Refill()
        {
            var now = DateTime.UtcNow; var e = now - _lastRefill;
            if (e >= TimeSpan.FromMinutes(1)) { _tokens = _perMin; _lastRefill = now; }
            else if (e.TotalMilliseconds > 0)
            {
                var add = (int)(_perMin * e.TotalMinutes);
                _tokens = Math.Min(_perMin, _tokens + add);
                if (add > 0) _lastRefill = now;
            }
        }
    }
}
