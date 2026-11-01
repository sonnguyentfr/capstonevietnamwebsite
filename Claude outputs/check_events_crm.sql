/* Chẩn đoán phần Sự kiện không hiển thị trên capstonevietnam.com
   Chạy trên DB mà CRMConnection trỏ tới (CapstoneVietNamCRM @ 161.248.78.217,1439) */
USE CapstoneVietNamCRM;
GO

-- 1) Event category đang nằm ở PortalId nào? (website đang lọc PortalId = 50)
SELECT PortalId,
       COUNT(*)                                             AS TongSo,
       SUM(CASE WHEN Isactive = 1 THEN 1 ELSE 0 END)        AS Active,
       SUM(CASE WHEN is_show_website = 1 THEN 1 ELSE 0 END) AS ShowWebsite,
       SUM(CASE WHEN is_show_website IS NULL THEN 1 ELSE 0 END) AS ShowWebsite_NULL,
       SUM(CASE WHEN EndDate >= GETDATE() THEN 1 ELSE 0 END)    AS ChuaKetThuc,
       MAX(EndDate)                                         AS EndDate_MoiNhat
FROM NV_Events_Cat
GROUP BY PortalId
ORDER BY PortalId;

-- 2) 20 category mới nhất – xem cột nào đang chặn
SELECT TOP 20 id, CatName, PortalId, Isactive, is_show_website, FromDate, EndDate, DateShow
FROM NV_Events_Cat
ORDER BY id DESC;

-- 3) Event con theo PortalId
SELECT PortalId, COUNT(*) AS SoEvent FROM NV_Events GROUP BY PortalId;

-- 4) SP mà trang chủ / trang /su-kien gọi
EXEC NV_Events_Cat_SelectAllOnlineViewWebsite @PortalId = 50;
SELECT OBJECT_DEFINITION(OBJECT_ID('dbo.NV_Events_Cat_SelectAllOnlineViewWebsite')) AS SP_Definition;

/* Cách sửa tuỳ kết quả:
   - Dữ liệu nằm ở PortalId khác 50  -> sửa "SiteSettings:PortalCRMId" trong WWW_VIEW\appsettings.json
     (sau khi deploy bản code mới) rồi recycle app pool.
   - is_show_website = NULL/0 do DB mới chưa set -> bật lại cờ hiển thị cho các sự kiện cần show, ví dụ:
     UPDATE NV_Events_Cat SET is_show_website = 1 WHERE PortalId = 50 AND Isactive = 1 AND is_show_website IS NULL;
   - SP ở bước 4 không tồn tại / khác logic -> cần tạo lại SP từ DB cũ.
*/
