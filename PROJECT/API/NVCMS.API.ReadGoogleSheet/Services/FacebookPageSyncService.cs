using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Repositories;

namespace NVCMS.API.ReadGoogleSheet.Services;

/// <summary>
/// FB-SYNC 19/11/2026: lấy bài mới của Facebook Page và tạo thành bài Tin tức trên website.
///
/// Mỗi lần chạy: đọc các bài Page đã đăng kể từ bài mới nhất đã ghi nhận (lùi 1 ngày cho chắc),
/// xử lý từ cũ tới mới, mỗi bài Facebook chỉ tạo đúng 1 bài Tin tức (bảng News_FacebookSync).
/// Ảnh được tải về máy chủ vì link ảnh của Facebook hết hạn sau một thời gian.
/// </summary>
public class FacebookPageSyncService
{
    // Trùng với Enum NewsStatus của NVCMS.Modules.TinTuc
    private const int StatusDangBienSoan = 0;
    private const int StatusDaXuatBan = 2;
    private const int StatusChoXuatBan = 5;

    // Trùng với Enum TheLoaiTin + BL.FormatTheLoai
    private const int TypeText = 1;
    private const int TypeTextImage = 5;
    private const string TypeUrlText = "/Images/icons/vov/TEXT_ONLY.png";
    private const string TypeUrlTextImage = "/Images/icons/vov/TEXT_I.png";

    private const string SettingImagePhysical = "settingAnhLuuTruPhysical";
    private const string SettingImageVirtual = "settingAnhLuuTruVirtual";

    private const int TitleMaxLength = 160;
    private const int MaxVideosPerPost = 2;

    // Từ viết tắt thường gặp - giữ nguyên chữ hoa khi đổi tiêu đề viết HOA
    private static readonly HashSet<string> DefaultKeepUpper = new(StringComparer.OrdinalIgnoreCase)
    {
        "USA", "US", "UK", "EU", "ASEAN", "VN", "HN", "HCM", "TPHCM", "TP",
        "IELTS", "TOEFL", "SAT", "ACT", "GPA", "GMAT", "GRE", "PTE", "AP", "IB", "ESL",
        "MBA", "STEM", "IT", "AI", "CEO", "PR", "FAQ", "OPT", "CPT",
        "UC", "UCLA", "MIT", "NYU", "CSU", "K12", "F1", "J1"
    };
    private const int SummaryMaxLength = 500;

    private static readonly Regex HashtagRegex = new(@"(?<![\p{L}\p{N}_&])#([\p{L}\p{N}_]+)", RegexOptions.Compiled);
    private static readonly Regex UrlRegex = new(@"https?://[^\s<>""]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex SpaceRegex = new(@"\s+", RegexOptions.Compiled);

    private readonly FacebookGraphClient _graph;
    private readonly FacebookNewsRepository _repo;
    private readonly FacebookSyncSettings _settings;
    private readonly ILogger<FacebookPageSyncService> _logger;

    public FacebookPageSyncService(
        FacebookGraphClient graph,
        FacebookNewsRepository repo,
        IOptions<FacebookSyncSettings> settings,
        ILogger<FacebookPageSyncService> logger)
    {
        _graph = graph;
        _repo = repo;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<FacebookSyncResult> RunAsync(List<JobFailure> failures, CancellationToken ct)
    {
        ValidateSettings();

        var result = new FacebookSyncResult();
        var pageId = _settings.PageId.Trim();

        await _repo.EnsureTableAsync(ct);

        var last = await _repo.GetLastCreatedTimeUtcAsync(pageId, ct);
        var sinceUtc = last.HasValue
            ? last.Value.AddDays(-1)
            : DateTime.UtcNow.AddDays(-Math.Max(1, _settings.FirstRunLookbackDays));

        var posts = await _graph.GetPublishedPostsAsync(sinceUtc, ct);
        result.Fetched = posts.Count;

        var maxPerRun = Math.Max(1, _settings.MaxPostsPerRun);
        var handled = 0;
        ImageFolder? imageFolder = null;

        // Cũ trước, mới sau: mốc "bài mới nhất đã ghi nhận" luôn tiến dần, không bỏ sót bài
        foreach (var post in posts
                     .Where(p => !string.IsNullOrEmpty(p.Id))
                     .OrderBy(p => p.CreatedTimeUtc ?? DateTime.MinValue))
        {
            ct.ThrowIfCancellationRequested();

            if (handled >= maxPerRun)
                break;

            if (!await _repo.TryClaimAsync(pageId, post, ct))
                continue;

            handled++;

            try
            {
                var skipReason = GetSkipReason(post);
                if (skipReason != null)
                {
                    await _repo.MarkAsync(post.Id, FacebookSyncStatus.Skipped, skipReason, ct);
                    result.Skipped++;
                    continue;
                }

                imageFolder ??= await ResolveImageFolderAsync(ct);

                var newId = await ImportPostAsync(post, imageFolder, ct);
                await _repo.MarkAsync(post.Id, FacebookSyncStatus.Imported, null, ct);

                result.Imported++;
                result.Messages.Add($"NewId {newId} <= {post.PermalinkUrl ?? post.Id}");
                _logger.LogInformation("FB-SYNC: đã tạo bài NewId={NewId} từ bài Facebook {PostId}", newId, post.Id);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                result.Failed++;
                _logger.LogError(ex, "FB-SYNC: lỗi xử lý bài Facebook {PostId}", post.Id);

                try
                {
                    await _repo.MarkAsync(post.Id, FacebookSyncStatus.Failed, ex.Message, CancellationToken.None);
                }
                catch (Exception markEx)
                {
                    _logger.LogError(markEx, "FB-SYNC: không ghi được trạng thái lỗi cho bài {PostId}", post.Id);
                }

                failures.Add(new JobFailure
                {
                    Step = "Tạo bài Tin tức từ bài Facebook",
                    FallbackSource = $"{nameof(FacebookPageSyncService)}.{nameof(ImportPostAsync)}",
                    Context = new Dictionary<string, string?>
                    {
                        ["FbPostId"] = post.Id,
                        ["Permalink"] = post.PermalinkUrl
                    },
                    Exception = ex
                });
            }
        }

        return result;
    }

    private void ValidateSettings()
    {
        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(_settings.PageId)) missing.Add("FacebookSync:PageId");
        if (string.IsNullOrWhiteSpace(_settings.PageAccessToken)) missing.Add("FacebookSync:PageAccessToken");
        if (_settings.CategoryId <= 0) missing.Add("FacebookSync:CategoryId");
        if (_settings.UserId <= 0) missing.Add("FacebookSync:UserId");

        if (missing.Count > 0)
            throw new InvalidOperationException("Chưa cấu hình: " + string.Join(", ", missing));
    }

    /// <summary>Lý do bỏ qua bài (null = lấy).</summary>
    private string? GetSkipReason(FbPost post)
    {
        var message = (post.Message ?? string.Empty).Trim();

        if (_settings.SkipSharedPosts && string.Equals(post.StatusType, "shared_story", StringComparison.OrdinalIgnoreCase))
            return "Bài chia sẻ lại";

        if (message.Length == 0)
            return "Bài không có nội dung chữ";

        if (message.Length < _settings.MinMessageLength)
            return $"Nội dung ngắn hơn {_settings.MinMessageLength} ký tự";

        if (_settings.RequiredHashtags is { Length: > 0 })
        {
            var wanted = _settings.RequiredHashtags
                .Select(h => (h ?? string.Empty).Trim().TrimStart('#'))
                .Where(h => h.Length > 0)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (wanted.Count > 0 && !ExtractHashtags(message).Any(wanted.Contains))
                return "Không có hashtag yêu cầu";
        }

        if (_settings.SkipPostsWithoutImage && CollectImageUrls(post).Count == 0)
            return "Bài không có ảnh";

        return null;
    }

    private async Task<int> ImportPostAsync(FbPost post, ImageFolder folder, CancellationToken ct)
    {
        var now = DateTime.Now;
        var parsed = ParseMessage(post.Message ?? string.Empty);

        // 1. Ảnh: tải về máy chủ. Ảnh đầu làm ảnh đại diện, các ảnh sau chèn vào nội dung.
        var images = await SaveImagesAsync(post, folder, now, ct);
        var avatar = images.Count > 0 ? images[0] : string.Empty;

        // 2. Nội dung
        var content = BuildContentHtml(parsed, images.Skip(1).ToList(), post);
        var hasImage = images.Count > 0;

        // Giờ xuất bản = giờ đăng trên Facebook (giờ máy chủ), không vượt quá hiện tại
        var publishedDate = post.CreatedTimeUtc?.ToLocalTime() ?? now;
        if (publishedDate > now) publishedDate = now;

        var keyword = Truncate(string.Join(", ", parsed.Hashtags), 100);

        // 3. News_add - THỨ TỰ theo SqlDataProvider.NV_News_add (48 tham số)
        var values = new object?[]
        {
            Truncate(parsed.Title, 500),            // meta_title
            Truncate(parsed.SummaryText, 300),      // meta_description
            avatar,                                 // meta_image
            Truncate(ToSlug(parsed.Title), 200),    // meta_url
            _settings.CategoryId,                   // CategoryId
            Truncate(parsed.Title, 1000),           // Title
            avatar,                                 // ImagePath
            WebUtility.HtmlEncode(parsed.SummaryText), // Summary
            keyword,                                // keyword
            content,                                // Content
            true,                                   // isActive
            false,                                  // Hotcat
            false,                                  // Hotsite
            StatusDangBienSoan,                     // Status (đổi sang Đã xuất bản ở bước 5)
            0,                                      // Unit
            _settings.NewsKind,                     // NewsKind
            hasImage ? TypeTextImage : TypeText,    // Type
            hasImage ? TypeUrlTextImage : TypeUrlText, // TypeUrl
            string.Empty,                           // Links (tin liên quan)
            string.Empty,                           // Tags
            false,                                  // IsImage
            false,                                  // IsVideo
            false,                                  // IsPhoto
            false,                                  // IsPR
            false,                                  // IsShowBaiMoi
            false,                                  // IsAMP
            false,                                  // IsHienQuangCao
            false,                                  // IsAnNoiDung
            Truncate(_settings.ButDanh, 50),        // ButDanh
            string.Empty,                           // Note (đang dùng làm mật khẩu bài - để trống)
            0,                                      // SourceInfo
            string.Empty,                           // SourceText (đang dùng làm link đăng ký - để trống)
            now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture), // StorageFolder
            string.Empty,                           // AttachedFiles
            false,                                  // IsEdited
            0,                                      // EditedUser
            null,                                   // EditedTime
            0,                                      // Credit
            now,                                    // CreateDate
            null,                                   // ApprovalRequestDate
            null,                                   // ApprovalDate
            null,                                   // ReturnedDate
            null,                                   // CancelPublishDate
            _settings.AutoPublish ? (object?)publishedDate : null, // PublishedDate = ngày đăng trên Facebook
            _settings.UserId,                       // UserId
            string.Empty,                           // Tacgia
            _settings.LanguageId,                   // LanguageId
            _settings.PortalId                      // PortalId
        };

        var newId = await _repo.InsertNewsAsync(values, ct);

        // Ghi NewId ngay: nếu các bước sau lỗi thì lần chạy sau KHÔNG tạo lại bài thứ hai
        await _repo.SetNewIdAsync(post.Id, newId, CancellationToken.None);

        // 4. Chuyên mục + lịch sử (giống AddNews của màn soạn tin)
        await _repo.AddCategoryAsync(newId, _settings.CategoryId, true, ct);
        await _repo.UpdateStatusAsync(newId, StatusDangBienSoan, _settings.UserId, ct);
        await TryLogProcessAsync(newId, StatusDangBienSoan, "Tạo tự động từ Facebook Page", ct);

        // 5. Xuất bản - cùng thứ tự gọi như nút "Xuất bản ngay" (lbtSaveXB_Click)
        if (_settings.AutoPublish)
        {
            await TryLogProcessAsync(newId, StatusDaXuatBan, "Tự động xuất bản (Facebook Page) => ", ct);
            await _repo.UpdateStatusAsync(newId, StatusChoXuatBan, _settings.UserId, ct);
            await _repo.UpdatePublishedDateAsync(newId, publishedDate, _settings.UserId, ct);
            await _repo.UpdateStatusAsync(newId, StatusDaXuatBan, _settings.UserId, ct);
            // News_updateStatus có thể tự đặt ngày xuất bản = hiện tại -> đặt lại theo ngày đăng trên Facebook
            await _repo.UpdatePublishedDateAsync(newId, publishedDate, _settings.UserId, ct);
        }

        return newId;
    }

    private async Task TryLogProcessAsync(int newId, int statusId, string processName, CancellationToken ct)
    {
        try
        {
            await _repo.InsertProcessAsync(newId, statusId, processName, _settings.UserId, ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Lịch sử xử lý chỉ để tra cứu - không để nó làm hỏng việc tạo bài
            _logger.LogWarning(ex, "FB-SYNC: không ghi được lịch sử xử lý cho NewId={NewId}", newId);
        }
    }

    // ── Ảnh ──────────────────────────────────────────────────────────────────

    private sealed record ImageFolder(string Physical, string Virtual);

    private async Task<ImageFolder> ResolveImageFolderAsync(CancellationToken ct)
    {
        var physical = _settings.ImagePhysicalPath;
        var virt = _settings.ImageVirtualPath;

        if (string.IsNullOrWhiteSpace(physical))
            physical = await _repo.GetPortalSettingAsync(_settings.PortalId, SettingImagePhysical, ct) ?? string.Empty;
        if (string.IsNullOrWhiteSpace(virt))
            virt = await _repo.GetPortalSettingAsync(_settings.PortalId, SettingImageVirtual, ct) ?? string.Empty;

        if (string.IsNullOrWhiteSpace(physical) || string.IsNullOrWhiteSpace(virt))
            throw new InvalidOperationException(
                "Không xác định được thư mục lưu ảnh. Điền FacebookSync:ImagePhysicalPath và FacebookSync:ImageVirtualPath " +
                $"(hoặc kiểm tra PortalSettings {SettingImagePhysical}/{SettingImageVirtual} của portal {_settings.PortalId}).");

        return new ImageFolder(physical.Trim(), virt.Trim().TrimEnd('/'));
    }

    /// <summary>Tải ảnh của bài về thư mục ảnh của website. Trả danh sách đường dẫn ảo đã lưu.</summary>
    private async Task<List<string>> SaveImagesAsync(FbPost post, ImageFolder folder, DateTime now, CancellationToken ct)
    {
        var saved = new List<string>();
        var urls = CollectImageUrls(post).Take(Math.Max(0, _settings.MaxImagesPerPost)).ToList();
        if (urls.Count == 0)
            return saved;

        var datePart = now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
        var physicalDir = Path.Combine(folder.Physical, now.ToString("yyyy"), now.ToString("MM"), now.ToString("dd"));
        Directory.CreateDirectory(physicalDir);

        var safeId = Regex.Replace(post.Id, @"[^0-9A-Za-z_]", "-");

        for (var i = 0; i < urls.Count; i++)
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                var image = await _graph.DownloadImageAsync(urls[i], ct);
                if (image == null)
                    continue;

                var fileName = $"fb-{safeId}-{i + 1}.{image.Value.Ext}";
                await File.WriteAllBytesAsync(Path.Combine(physicalDir, fileName), image.Value.Bytes, ct);
                saved.Add($"{folder.Virtual}/{datePart}/{fileName}");
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                // Thiếu 1 ảnh không chặn cả bài
                _logger.LogWarning(ex, "FB-SYNC: không tải được ảnh {Index} của bài {PostId}", i + 1, post.Id);
            }
        }

        return saved;
    }

    /// <summary>
    /// Ảnh của bài: ảnh trong album / ảnh đính kèm. Bài chỉ có video thì dùng ảnh thumbnail của video
    /// (làm ảnh đại diện), không có nữa thì dùng full_picture.
    /// </summary>
    private static List<string> CollectImageUrls(FbPost post)
    {
        var photos = new List<string>();
        var thumbs = new List<string>();

        foreach (var att in FlattenAttachments(post))
        {
            var src = att.Media?.Image?.Src;
            if (string.IsNullOrWhiteSpace(src))
                continue;

            (IsVideo(att) ? thumbs : photos).Add(src);
        }

        var urls = photos.Count > 0 ? photos : thumbs;
        if (urls.Count == 0 && !string.IsNullOrWhiteSpace(post.FullPicture))
            urls.Add(post.FullPicture);

        return urls.Distinct(StringComparer.Ordinal).ToList();
    }

    /// <summary>Mọi tệp đính kèm của bài, album thì lấy từng mục con.</summary>
    private static IEnumerable<FbAttachment> FlattenAttachments(FbPost post)
    {
        foreach (var att in post.Attachments?.Data ?? new List<FbAttachment>())
        {
            var subs = att.Subattachments?.Data;
            if (subs is { Count: > 0 })
            {
                foreach (var sub in subs)
                    yield return sub;
            }
            else
            {
                yield return att;
            }
        }
    }

    // ── Video ────────────────────────────────────────────────────────────────

    private sealed record VideoEmbed(string Href, int Width, int Height);

    private List<VideoEmbed> CollectVideos(FbPost post)
    {
        var videos = new List<VideoEmbed>();

        foreach (var att in FlattenAttachments(post).Where(IsVideo))
        {
            var href = ResolveVideoHref(att, post);
            if (href == null || videos.Any(v => v.Href == href))
                continue;

            videos.Add(new VideoEmbed(href, att.Media?.Image?.Width ?? 0, att.Media?.Image?.Height ?? 0));
            if (videos.Count >= MaxVideosPerPost)
                break;
        }

        return videos;
    }

    /// <summary>Địa chỉ video trên Facebook để đưa vào trình phát nhúng.</summary>
    private string? ResolveVideoHref(FbAttachment att, FbPost post)
    {
        if (IsFacebookPageUrl(att.Url))
            return att.Url;

        var videoId = att.Target?.Id;
        if (!string.IsNullOrEmpty(videoId) && videoId.All(char.IsDigit))
            return $"https://www.facebook.com/{_settings.PageId.Trim()}/videos/{videoId}";

        return IsFacebookPageUrl(post.PermalinkUrl) ? post.PermalinkUrl : null;
    }

    private static bool IsFacebookPageUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out var u) || u.Scheme != Uri.UriSchemeHttps)
            return false;

        var host = u.Host.ToLowerInvariant();
        if (host is "l.facebook.com" or "lm.facebook.com")
            return false;   // link chuyển hướng ra ngoài, không phải video

        return host == "facebook.com" || host.EndsWith(".facebook.com") || host == "fb.watch";
    }

    /// <summary>Khung phát video Facebook, co giãn theo bề ngang bài. Video dọc (reel) được thu hẹp và căn giữa.</summary>
    private static string BuildVideoEmbedHtml(VideoEmbed video)
    {
        var ratio = 56.25;
        var maxWidth = 800;

        if (video.Width > 0 && video.Height > 0)
        {
            ratio = Math.Clamp(video.Height * 100.0 / video.Width, 40, 178);
            if (video.Height > video.Width)
                maxWidth = 360;
        }

        var src = "https://www.facebook.com/plugins/video.php?href=" + Uri.EscapeDataString(video.Href) + "&show_text=false";

        return "<div class=\"fb-video\" style=\"max-width:" + maxWidth + "px;margin:0 auto 16px\">" +
               "<div style=\"position:relative;height:0;overflow:hidden;padding-bottom:" +
               ratio.ToString("0.##", CultureInfo.InvariantCulture) + "%\">" +
               "<iframe src=\"" + WebUtility.HtmlEncode(src) + "\" " +
               "style=\"position:absolute;top:0;left:0;width:100%;height:100%;border:0;overflow:hidden\" " +
               "scrolling=\"no\" frameborder=\"0\" allowfullscreen=\"true\" loading=\"lazy\" " +
               "allow=\"autoplay; clipboard-write; encrypted-media; picture-in-picture; web-share\"></iframe>" +
               "</div></div>\n";
    }

    private static bool HasVideo(FbPost post) =>
        (post.Attachments?.Data ?? new List<FbAttachment>()).Any(a =>
            IsVideo(a) || (a.Subattachments?.Data?.Any(IsVideo) ?? false));

    private static bool IsVideo(FbAttachment a) =>
        string.Equals(a.MediaType, "video", StringComparison.OrdinalIgnoreCase) ||
        (a.Type ?? string.Empty).StartsWith("video", StringComparison.OrdinalIgnoreCase);

    // ── Nội dung ─────────────────────────────────────────────────────────────

    private sealed record ParsedMessage(string Title, string SummaryText, List<List<string>> BodyBlocks, List<string> Hashtags);

    /// <summary>
    /// Tách bài Facebook thành: tiêu đề (dòng đầu), tóm tắt (đoạn kế tiếp), các đoạn nội dung, hashtag.
    /// Đoạn = các dòng liền nhau; dòng trống (hoặc dòng chỉ có ký hiệu trang trí) mới ngắt đoạn.
    /// Nhờ vậy danh sách gạch đầu dòng / dòng ngắn không bị tách thành từng đoạn cách xa nhau.
    /// </summary>
    private ParsedMessage ParseMessage(string message)
    {
        message = message.Normalize(NormalizationForm.FormC);
        var hashtags = ExtractHashtags(message).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        var blocks = new List<List<string>>();
        List<string>? current = null;

        foreach (var raw in message.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n'))
        {
            var line = SpaceRegex.Replace(raw, " ").Trim();

            if (line.Length > 0 && IsHashtagOnlyLine(line))
                continue;

            if (line.Length == 0 || !line.Any(char.IsLetterOrDigit))
            {
                current = null;   // dòng trống / dòng trang trí (-----, ***, toàn emoji) => ngắt đoạn
                continue;
            }

            if (current == null)
            {
                current = new List<string>();
                blocks.Add(current);
            }

            current.Add(line);
        }

        if (blocks.Count == 0)
            blocks.Add(new List<string> { message.Trim() });

        // Tiêu đề: dòng đầu. Dài quá thì cắt, và giữ nguyên dòng đó trong nội dung để không mất chữ.
        var firstLine = CleanTitle(blocks[0][0]);
        string title;
        if (firstLine.Length <= TitleMaxLength)
        {
            title = firstLine;
            blocks[0].RemoveAt(0);
            if (blocks[0].Count == 0)
                blocks.RemoveAt(0);
        }
        else
        {
            title = CutAtWord(firstLine, TitleMaxLength);
        }

        if (_settings.NormalizeUppercaseTitle)
            title = NormalizeUppercaseTitle(title);

        if (title.Length == 0)
            title = "Tin từ Facebook";

        // Tóm tắt: đoạn đầu của nội dung. Vừa đủ ngắn thì lấy làm tóm tắt và bỏ khỏi nội dung (trang chi tiết
        // hiện tóm tắt ngay trên nội dung, để nguyên sẽ bị lặp).
        string summary;
        if (blocks.Count == 0)
        {
            summary = title;
        }
        else
        {
            var firstBlock = string.Join(" ", blocks[0]);
            if (firstBlock.Length <= SummaryMaxLength && blocks.Count > 1)
            {
                summary = firstBlock;
                blocks.RemoveAt(0);
            }
            else
            {
                summary = CutAtWord(firstBlock, Math.Min(SummaryMaxLength, 300));
            }
        }

        return new ParsedMessage(title, summary, blocks, hashtags);
    }

    /// <summary>Bỏ dấu # và emoji / ký hiệu trang trí ở đầu, cuối tiêu đề.</summary>
    private static string CleanTitle(string line)
    {
        var text = SpaceRegex.Replace(HashtagRegex.Replace(line, "$1"), " ").Trim();

        var start = 0;
        while (start < text.Length && !char.IsLetterOrDigit(text[start]) && "\"“‘'([".IndexOf(text[start]) < 0)
            start++;

        var end = text.Length;
        while (end > start && !char.IsLetterOrDigit(text[end - 1]) && "?!%)]\"”’'".IndexOf(text[end - 1]) < 0)
            end--;

        return text[start..end].Trim();
    }

    /// <summary>
    /// Tiêu đề viết HOA toàn bộ (từ 70% chữ cái trở lên) => Viết Hoa Chữ Đầu Mỗi Từ.
    /// Từ viết tắt quen thuộc và từ có chữ số (IELTS, USA, K12...) được giữ nguyên.
    /// </summary>
    private string NormalizeUppercaseTitle(string title)
    {
        var letters = title.Count(char.IsLetter);
        if (letters < 4 || title.Count(char.IsUpper) * 100 < letters * 70)
            return title;

        var culture = CultureInfo.GetCultureInfo("vi-VN");
        var sb = new StringBuilder(title.Length);
        var i = 0;

        // Duyệt từng cụm chữ/số liền nhau; dấu cách, dấu chấm, gạch nối... giữ nguyên
        // (nên "TP.HCM" và "IELTS/SAT" vẫn được nhận ra là từ viết tắt)
        while (i < title.Length)
        {
            if (!char.IsLetterOrDigit(title[i]))
            {
                sb.Append(title[i++]);
                continue;
            }

            var start = i;
            while (i < title.Length && char.IsLetterOrDigit(title[i]))
                i++;

            var run = title[start..i];
            var afterApostrophe = start > 0 && (title[start - 1] == '\'' || title[start - 1] == '’');

            if (run.Any(char.IsDigit) || IsKeepUpperWord(run))
            {
                sb.Append(run);
            }
            else
            {
                var lower = run.ToLower(culture);
                sb.Append(afterApostrophe ? lower : char.ToUpper(lower[0], culture) + lower[1..]);
            }
        }

        return sb.ToString();
    }

    private bool IsKeepUpperWord(string word) =>
        DefaultKeepUpper.Contains(word) ||
        (_settings.KeepUppercaseWords?.Any(w => string.Equals((w ?? string.Empty).Trim(), word, StringComparison.OrdinalIgnoreCase)) ?? false);

    private string BuildContentHtml(ParsedMessage parsed, List<string> contentImages, FbPost post)
    {
        var sb = new StringBuilder();
        var altText = WebUtility.HtmlEncode(parsed.Title);

        // Video đặt đầu bài
        if (_settings.EmbedVideo)
        {
            foreach (var video in CollectVideos(post))
                sb.Append(BuildVideoEmbedHtml(video));
        }

        // Mỗi đoạn một thẻ p, các dòng trong đoạn xuống hàng bằng br (không tạo khoảng trống giữa các dòng)
        foreach (var block in parsed.BodyBlocks)
            sb.Append("<p>").Append(string.Join("<br />\n", block.Select(Linkify))).Append("</p>\n");

        foreach (var img in contentImages)
        {
            sb.Append("<p style=\"text-align:center\"><img src=\"")
              .Append(WebUtility.HtmlEncode(img))
              .Append("\" alt=\"").Append(altText).Append("\" style=\"max-width:100%;height:auto\" /></p>\n");
        }

        var permalink = post.PermalinkUrl;
        if (_settings.AppendSourceLink && !string.IsNullOrWhiteSpace(permalink) && IsHttpUrl(permalink))
        {
            sb.Append("<p><em>Nguồn: <a href=\"").Append(WebUtility.HtmlEncode(permalink))
              .Append("\" target=\"_blank\" rel=\"noopener\">Xem bài gốc trên Facebook</a></em></p>\n");
        }

        return sb.ToString();
    }

    /// <summary>Mã hoá HTML cho một đoạn chữ và biến link thành thẻ a.</summary>
    private static string Linkify(string text)
    {
        var sb = new StringBuilder();
        var pos = 0;

        foreach (Match m in UrlRegex.Matches(text))
        {
            var url = m.Value.TrimEnd('.', ',', ';', ':', '!', '?', ')', ']', '"', '\'');
            if (url.Length < 10)
                continue;

            sb.Append(WebUtility.HtmlEncode(text[pos..m.Index]));
            var encoded = WebUtility.HtmlEncode(url);
            sb.Append("<a href=\"").Append(encoded).Append("\" target=\"_blank\" rel=\"noopener nofollow\">")
              .Append(encoded).Append("</a>");
            pos = m.Index + url.Length;
        }

        sb.Append(WebUtility.HtmlEncode(text[pos..]));
        return sb.ToString();
    }

    private static IEnumerable<string> ExtractHashtags(string text) =>
        HashtagRegex.Matches(text).Select(m => m.Groups[1].Value);

    private static bool IsHashtagOnlyLine(string line) =>
        line.Contains('#') && HashtagRegex.Replace(line, string.Empty).Trim().Length == 0;

    private static bool IsHttpUrl(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var u) && (u.Scheme == Uri.UriSchemeHttps || u.Scheme == Uri.UriSchemeHttp);

    /// <summary>Cắt ở ranh giới từ, thêm dấu "…".</summary>
    private static string CutAtWord(string text, int max)
    {
        if (text.Length <= max)
            return text;

        var cut = text[..max];
        var lastSpace = cut.LastIndexOf(' ');
        if (lastSpace > max / 2)
            cut = cut[..lastSpace];

        return cut.TrimEnd('.', ',', ';', ':', '-', ' ') + "…";
    }

    private static string Truncate(string? s, int max)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        return s.Length <= max ? s : s[..max];
    }

    /// <summary>Tạo slug không dấu từ tiêu đề tiếng Việt.</summary>
    private static string ToSlug(string text)
    {
        var normalized = text.ToLowerInvariant().Replace('đ', 'd').Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            var cat = CharUnicodeInfo.GetUnicodeCategory(c);
            if (cat == UnicodeCategory.NonSpacingMark)
                continue;

            if (c is >= 'a' and <= 'z' or >= '0' and <= '9')
                sb.Append(c);
            else
                sb.Append('-');
        }

        var slug = Regex.Replace(sb.ToString(), "-{2,}", "-").Trim('-');
        if (slug.Length > 120)
        {
            slug = slug[..120];
            var lastDash = slug.LastIndexOf('-');
            if (lastDash > 60) slug = slug[..lastDash];
        }

        return slug.Length == 0 ? "tin-facebook" : slug;
    }
}
