Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Security
Imports DotNetNuke.Web.Api
Imports NVCMS.Modules.HeThong.Menus

Namespace NVCMS.Modules.HeThong.Api

    Public Class SaveMenuRequest
        Public Property Items As List(Of MenuNode)
        Public Property Version As String
        Public Property RootName As String
        Public Property ItemName As String
        ''' <summary>True = lưu đè dù file đã bị người khác sửa.</summary>
        Public Property Force As Boolean
    End Class

    ''' <summary>
    ''' Quản trị menu admin của portal hiện tại ({Portal home}\MenuAdmin\menu.xml, vd Portals\1001\MenuAdmin\menu.xml):
    ''' /DesktopModules/NVCMSHeThong/API/Menu/{action}.
    ''' Menu ảnh hưởng toàn bộ trang quản trị nên mọi thao tác (cả đọc) cần quyền Edit trên module
    ''' VÀ là Administrators của portal (hoặc SuperUser). Client gửi header ModuleId, TabId, RequestVerificationToken.
    ''' </summary>
    <DnnModuleAuthorize(AccessLevel:=SecurityAccessLevel.Edit)>
    Public Class MenuController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(MenuController))

        ''' <summary>Menu được đọc/ghi theo PortalId của request - cùng portal mà skin _admin sẽ hiển thị.</summary>
        Private Function GetStore() As MenuStore
            Return New MenuStore(PortalSettings.PortalId, PortalSettings.HomeDirectoryMapPath)
        End Function

        Private Function IsMenuAdmin() As Boolean
            Return UserInfo IsNot Nothing AndAlso (UserInfo.IsSuperUser OrElse UserInfo.IsInRole(PortalSettings.AdministratorRoleName))
        End Function

        <HttpGet>
        Public Function Tree() As HttpResponseMessage
            If Not IsMenuAdmin() Then Return Deny()
            Try
                Dim doc As MenuDocument = GetStore().Load()
                Return JsonOk(New With {
                    .success = True,
                    .items = doc.Items,
                    .version = doc.Version,
                    .rootName = doc.RootName,
                    .itemName = doc.ItemName,
                    .exists = (doc.Version.Length > 0),
                    .portalId = PortalSettings.PortalId,
                    .file = PortalSettings.HomeDirectory & "MenuAdmin/menu.xml"
                })
            Catch ex As Exception
                Return Fail(ex)
            End Try
        End Function

        <HttpPost, ValidateAntiForgeryToken>
        Public Function Save(req As SaveMenuRequest) As HttpResponseMessage
            If Not IsMenuAdmin() Then Return Deny()
            Try
                If req Is Nothing Then Throw New MenuStoreException("Thiếu dữ liệu menu.")
                Dim input As New MenuDocument()
                input.Items = req.Items
                input.RootName = req.RootName
                input.ItemName = req.ItemName
                Dim saved As MenuDocument = GetStore().Save(input, req.Version, req.Force)
                Logger.Info(String.Format("Menu admin portal {0} được lưu bởi user {1} ({2}).", PortalSettings.PortalId, UserInfo.UserID, UserInfo.Username))
                Return JsonOk(New With {
                    .success = True,
                    .message = "Đã lưu menu.",
                    .items = saved.Items,
                    .version = saved.Version
                })
            Catch ex As Exception
                Return Fail(ex)
            End Try
        End Function

        ''' <summary>Danh sách trang của portal để gắn TabId / tự điền link.</summary>
        <HttpGet>
        Public Function Tabs() As HttpResponseMessage
            If Not IsMenuAdmin() Then Return Deny()
            Try
                Dim portalTabs As List(Of TabInfo) = TabController.GetPortalTabs(PortalSettings.PortalId, Null.NullInteger, False, True, False, True)
                Dim rows = portalTabs.Select(Function(t) New With {
                    .id = t.TabID,
                    .name = t.IndentedTabName,
                    .title = t.TabName,
                    .url = RelativeUrl(t),
                    .level = t.Level,
                    .visible = t.IsVisible
                }).ToList()
                Return JsonOk(New With {
                    .success = True,
                    .tabs = rows
                })
            Catch ex As Exception
                Return Fail(ex)
            End Try
        End Function

        <HttpGet>
        Public Function Backups() As HttpResponseMessage
            If Not IsMenuAdmin() Then Return Deny()
            Try
                Dim rows = GetStore().Backups().Select(Function(f) New With {
                    .name = f.Name,
                    .time = f.LastWriteTime,
                    .size = f.Length
                }).ToList()
                Return JsonOk(New With {
                    .success = True,
                    .backups = rows
                })
            Catch ex As Exception
                Return Fail(ex)
            End Try
        End Function

        ''' <summary>Đọc nội dung một bản sao lưu để xem/khôi phục (khôi phục = client gọi Save với nội dung này).</summary>
        <HttpGet>
        Public Function Backup(name As String) As HttpResponseMessage
            If Not IsMenuAdmin() Then Return Deny()
            Try
                Dim doc As MenuDocument = GetStore().LoadBackup(name)
                Return JsonOk(New With {
                    .success = True,
                    .items = doc.Items,
                    .rootName = doc.RootName,
                    .itemName = doc.ItemName
                })
            Catch ex As Exception
                Return Fail(ex)
            End Try
        End Function

        ' ------------------------------------------------------------------ hỗ trợ

        Private Shared Function RelativeUrl(tab As TabInfo) As String
            Try
                Dim url As String = tab.FullUrl
                If String.IsNullOrEmpty(url) Then Return String.Empty
                Dim absolute As Uri = Nothing
                If Uri.TryCreate(url, UriKind.Absolute, absolute) Then Return absolute.PathAndQuery
                Return url
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        Private Function JsonOk(value As Object) As HttpResponseMessage
            Return Request.CreateResponse(HttpStatusCode.OK, value)
        End Function

        Private Function Deny() As HttpResponseMessage
            Return Request.CreateResponse(HttpStatusCode.Forbidden, New With {
                .success = False,
                .message = "Chỉ quản trị viên (Administrators) mới được quản trị menu."
            })
        End Function

        Private Function Fail(ex As Exception) As HttpResponseMessage
            Dim businessError As MenuStoreException = TryCast(ex, MenuStoreException)
            If businessError IsNot Nothing Then
                Dim status As HttpStatusCode = HttpStatusCode.BadRequest
                If businessError.Conflict Then status = HttpStatusCode.Conflict
                Return Request.CreateResponse(status, New With {
                    .success = False,
                    .conflict = businessError.Conflict,
                    .message = businessError.Message
                })
            End If

            Logger.Error(ex)
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, New With {
                .success = False,
                .message = "Lỗi hệ thống: " & ex.Message
            })
        End Function

    End Class

End Namespace
