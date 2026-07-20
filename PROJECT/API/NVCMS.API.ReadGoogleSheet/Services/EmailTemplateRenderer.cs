namespace NVCMS.API.ReadGoogleSheet.Services
{
    /// <summary>
    /// Đọc file HTML template từ thư mục EmailTemplates, thay thế {{PLACEHOLDER}} bằng giá trị thực.
    /// File được cache trong memory sau lần đọc đầu tiên.
    /// </summary>
    public class EmailTemplateRenderer
    {
        private readonly string _templateRoot;
        private readonly ILogger<EmailTemplateRenderer> _logger;
        // Simple in-memory cache: templateName → raw HTML
        private readonly Dictionary<string, string> _cache = new(StringComparer.OrdinalIgnoreCase);
        private readonly object _lock = new();

        public EmailTemplateRenderer(IWebHostEnvironment env,
            ILogger<EmailTemplateRenderer> logger)
        {
            _templateRoot = Path.Combine(env.ContentRootPath, "EmailTemplates");
            _logger = logger;
        }

        /// <summary>
        /// Đọc template, thay tất cả {{KEY}} bằng values[key].
        /// Nếu file không tồn tại → trả về chuỗi lỗi thay vì throw.
        /// </summary>
        public string Render(string templateName, Dictionary<string, string> values)
        {
            var raw = LoadTemplate(templateName);
            foreach (var (key, value) in values)
                raw = raw.Replace($"{{{{{key}}}}}", value, StringComparison.Ordinal);
            return raw;
        }

        private string LoadTemplate(string name)
        {
            lock (_lock)
            {
                if (_cache.TryGetValue(name, out var cached))
                    return cached;
            }

            var path = Path.Combine(_templateRoot, name);
            if (!File.Exists(path))
            {
                _logger.LogError("Email template not found: {Path}", path);
                return $"<!-- template not found: {name} -->";
            }

            var content = File.ReadAllText(path, System.Text.Encoding.UTF8);
            lock (_lock)
                _cache[name] = content;

            return content;
        }

        /// <summary>Xóa cache — gọi khi template file bị chỉnh sửa lúc runtime.</summary>
        public void InvalidateCache(string? templateName = null)
        {
            lock (_lock)
            {
                if (templateName is null) _cache.Clear();
                else _cache.Remove(templateName);
            }
        }
    }
}
