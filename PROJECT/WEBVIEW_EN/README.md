# Capstone Vietnam – website tiếng Anh (WEBVIEW_EN)

Ba project .NET 10, mở bằng `CapstoneEN.slnx`:

| Project | Vai trò |
|---|---|
| `Capstone.Content` | Thư viện dùng chung: **DTO (hợp đồng JSON)**, interface `IContentApi`, `JsonContentStore` đọc `MockData/` |
| `Capstone.ApiEN` | **Web API** `/api/v1/en/*` — trả MockData (`Json`) hoặc đọc SQL Server (`Sql`) |
| `Capstone.ViewEN` | **Website** Blazor Static SSR — gọi API qua HTTP |
| `MockData/` | Dữ liệu mẫu dùng chung cho cả API và website |

## 1. Chạy

**Visual Studio:** chọn profile khởi động **"Website + API"** (file `CapstoneEN.slnLaunch`) → F5.

**Dòng lệnh** (2 cửa sổ):
```
dotnet run --project Capstone.ApiEN     # http://localhost:5320  (OpenAPI: /openapi/v1.json)
dotnet run --project Capstone.ViewEN    # https://localhost:7310
```
- Chỉ làm giao diện, không muốn bật API: đặt `ContentApi:UseMock = true` trong `Capstone.ViewEN/appsettings.Development.json` → website đọc thẳng `MockData/`.
- Thử API nhanh: `Capstone.ApiEN/Capstone.ApiEN.http` (nút *Send request* trong VS).
- Sửa file trong `MockData/` khi đang chạy → API tự đọc lại, không cần restart.

## 2. Luồng dữ liệu

```
Trình duyệt ──HTML──> Capstone.ViewEN ──HTTP JSON (X-Api-Key)──> Capstone.ApiEN ──┬── MockData/*.json      (DataSource = Json)
                       (Blazor SSR)        ContentApiClient          /api/v1/en       └── SQL Server (Dapper)  (DataSource = Sql)
                                                                                         CrmDb: Cap_Truong, NV_Events*, CRM leads
                                                                                         WebDb: NVCMS news
```

### Capstone.ApiEN
- **Minimal API**, gom route tại `Endpoints/ContentEndpoints.cs`.
- **Output cache** 60 giây cho mọi GET (cấu hình `OutputCache:Seconds`, Development = 0). Sau khi sửa nội dung: `POST /api/v1/en/admin/cache/clear`.
- **API key**: `ApiSecurity:ApiKeys` có giá trị → bắt buộc header `X-Api-Key` (website gửi từ `ContentApi:ApiKey`). Để trống = mở (dev).
- **Leads** `POST /leads`: validate từng trường (trả `ValidationProblem` 400), rate limit, rồi lưu:
  - `Json` → nối vào `App_Data/leads.jsonl`;
  - `Sql` → SP `WebView_TuVan_Upsert` (giống form tiếng Việt), `CodePrefix = WEBEN`, vai trò/quan tâm/UTM ghi vào `TuVanKhac`.
- Lỗi chuẩn ProblemDetails; `/health` để giám sát.

### Chế độ `Sql` (`Content:DataSource = "Sql"`)
Điền `ConnectionStrings:CrmDb` (CapstoneVietnam_old) và `WebDb` (CapstoneVietNamWeb), **không commit mật khẩu** — dùng *User Secrets* hoặc biến môi trường trên server.

| Endpoint | Nguồn trong DB | Ghi chú |
|---|---|---|
| `countries` | SP `WebView_Truong_GetCountriesWithCount @IsPartner=1` | Map `Country` → US, CA, UK, AU, NZ, CH, IE; còn lại gộp "Other destinations" |
| `schools` | `Cap_Truong` (Status=1, isPartner=1), lọc tên/thành phố, nước, bậc học (`Loai` → nhóm như `TruongService.NormalizeLoai`) | `featured=true` dùng SP `WebView_Truong_RandomPartners` |
| `schools/{id}` | SP `WebView_Truong_GetById` + 3 trường cùng nước + sự kiện sắp tới có trường này (`FairSchool`, `NV_Events.School`) | Chỉ dùng cột **EN** (`TomTatEN`, `DescreptionWebsiteEN/DescreptionEN`, `KiemdinhEN`, `ThanhPholongannhatEN`) |
| `events` | `NV_Events_Cat` (PortalId = `Content:EventsPortalId`, mặc định 50) + `NV_Events` làm *sessions* | `type` suy ra: ≥2 trường hoặc ≥2 buổi = `fair`, còn lại `info-session`. Thành phố lấy từ `Vanphong` qua `Content:OfficeCities` |
| `news*` | SP `WebView_NVCMS_News_*`, `WebView_NVCMS_NewsCategory_SelectAll` | Bảng tin không có cột EN → tin tiếng Anh nằm ở portal riêng: **`Content:NewsPortalId` (cần kiểm tra giá trị thật)** |
| `site`, `home`, `pages`, `services`, `testimonials` | `MockData/*.json` | Chưa có bảng → biên tập trực tiếp file JSON |

Ảnh/HTML trong DB (`/DATA/...`, `/Portals/...`) được đổi sang `Content:FileServerBaseUrl` giống site VN.

## 3. Hợp đồng API

Base `…/api/v1/en/`. JSON **camelCase**, ngày giờ **ISO 8601 có offset** (+07:00). DTO định nghĩa duy nhất tại `Capstone.Content/Models/ContentModels.cs`.

| Method | Endpoint | Trả về |
|---|---|---|
| GET | `site` | `SiteDto` |
| GET | `home` | `HomeContentDto` |
| GET | `pages/{slug}` | `PageDto` / 404 |
| GET | `pages?section=about` | `PageDto[]` (không có body) — menu bên của nhóm About |
| GET | `team` | `TeamMemberDto[]` |
| GET | `destinations`, `destinations/{slug}` | `DestinationDto[]`, `DestinationDto` / 404 — trang /study-in/{slug} |
| GET | `countries` | `CountryDto[]` |
| GET | `schools?q=&country=&level=&featured=&page=&pageSize=` | `PagedResult<SchoolDto>` |
| GET | `schools/{id}` | `SchoolDetailDto` / 404 |
| GET | `services`, `services/{slug}` | `ServiceDto[]`, `ServiceDto` / 404 |
| GET | `events?type=fair\|info-session&when=upcoming\|past&page=&pageSize=` | `PagedResult<EventDto>` |
| GET | `events/{id}` | `EventDetailDto` / 404 |
| GET | `news/categories`, `news?category=&country=&page=&pageSize=`, `news/{id}` | `CategoryDto[]` (có `group`: topic/destination, `count`), `PagedResult<NewsDto>`, `NewsDetailDto` / 404 |
| GET | `testimonials?take=` | `TestimonialDto[]` |
| POST | `leads` | `LeadResult` / 400 `ValidationProblem` / 429 |
| POST | `admin/cache/clear` | 204 |

`PagedResult<T>` = `{ items, total, page, pageSize }`. Giá trị `level`: High school, Community college, College, University, Graduate school, ESL, Summer programme.

## 4. Website (Capstone.ViewEN)

| URL | Trang |
|---|---|
| `/` | Home (9 call API song song): hero, hội chợ sắp tới, tìm trường, điểm đến, trường đối tác, tin tức + học bổng, dịch vụ, sự kiện, khối cho trường, nhà sáng lập + học viên, form |
| `/study-in`, `/study-in/{usa, canada, uk, australia, other-countries}` | Điểm đến: thông tin nhanh, trường đối tác, bài viết theo nước |
| `/about`, `/about/{vision-mission-values, why-capstone, our-team, careers}` | Nhóm About có menu bên (như site cũ) |
| `/partners`, `/partners/{slug}-{id}` | Danh bạ & chi tiết trường |
| `/services`, `/services/{slug}` | Dịch vụ |
| `/events`, `/events/{slug}-{id}` | Triển lãm & info session (JSON-LD Event) |
| `/news?category=&country=`, `/news/{slug}-{id}` | Tin tức: lọc theo chủ đề và theo nước (JSON-LD NewsArticle) |
| `/institutions`, `/contact`, `/privacy`, `/terms` | Trang nội dung |

- API lỗi ở một khối → khối đó hiện trạng thái rỗng, trang vẫn render. Chi tiết 404 → trang 404 thật.
- Form tư vấn: Static SSR + antiforgery + honeypot + rate limit 5 POST/phút/IP, chuyển UTM sang API.
- SEO: canonical, OG, JSON-LD, `sitemap.xml`, `robots.txt`.
- **301 từ site EN cũ**: các trang menu cũ (`about-capstone/*.html`, `our-partners/study-in-*/*.html`, `events/*.html`, `news/*.html`, `overseas-study/*.html`, terms, privacy) và **mọi bài viết `…-{id}.html` → `/news/{slug}-{id}`** (giữ nguyên id bài).

### MockData lấy từ site cũ
`news.json` là 28 bài thật của en.capstonevietnam.com (tiêu đề, ngày, chuyên mục, ảnh trên file server, id giữ nguyên; thân bài là bản tóm tắt). `schools.json` gồm 64 trường trong danh sách đối tác cũ. `team.json`, các trang About, `destinations.json` dựng từ nội dung cũ. Sự kiện **sắp tới** là dữ liệu mẫu; ngày của vài bài/sự kiện cũ không hiện trên site cũ nên là **ngày ước lượng**.

Thiết kế: navy + đỏ logo, đồng Đông Sơn làm điểm nhấn; hoạ tiết mặt trống đồng (`DongSonDrum.razor`) hiển thị các điểm đến và số trường; font Be Vietnam Pro. Token màu ở đầu `wwwroot/css/app.css`.

## 5. Production checklist

- [ ] `Capstone.ApiEN`: `DataSource = Sql`, connection string qua secrets, `ApiKeys` có ít nhất 1 key, kiểm tra `NewsPortalId`, `OfficeCities`.
- [ ] `Capstone.ViewEN`: `ContentApi:BaseUrl` = URL API nội bộ, `ContentApi:ApiKey` khớp, `Site:BaseUrl` = domain thật.
- [ ] Backend **sanitize** HTML từ CMS (`descriptionHtml`, `bodyHtml`, `headerCode`).
- [ ] Xoá ảnh mẫu `Capstone.ViewEN/wwwroot/img/sample/` khi có ảnh thật.
- [ ] Gửi email thông báo lead cho tư vấn viên (như `TuVanDangKyMailService` bên VN) và cân nhắc tách enquiry của **institution** khỏi `Student_Info`.
- [ ] `hreflang` từng trang ↔ URL tiếng Việt; self-host font; reCAPTCHA v3 cho form.
