namespace NVCMS.API.ReadGoogleSheet.Jobs
{
    public class ZnsRefreshTokenJob
    {
        private readonly IZaloService _zaloTokenService;
        private readonly ILogger<ZnsRefreshTokenJob> _logger;

        public ZnsRefreshTokenJob(
            IZaloService zaloTokenService,
            ILogger<ZnsRefreshTokenJob> logger)
        {
            _zaloTokenService = zaloTokenService;
            _logger = logger;
        }

        public async Task Execute()
        {
            try
            {
                _logger.LogInformation(
                    "ZNS Refresh Token Job started at {Time}",
                    DateTime.Now);
                var token = await _zaloTokenService.GetLastTokenAsync();
                if (token == null)
                {
                    _logger.LogWarning("No Zalo token found in database.");
                    return;
                }
                await _zaloTokenService.RefreshAndSaveTokenAsync(token.RefreshToken);
                _logger.LogInformation("ZNS Refresh Token Job completed at {Time}", DateTime.Now);
            }   
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while refreshing ZNS token");
                throw; // để Hangfire tự retry
            }
        }
    }
}