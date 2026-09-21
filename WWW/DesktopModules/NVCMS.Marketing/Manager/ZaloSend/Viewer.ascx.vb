Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.Events
Imports NVCMS.Modules.EventsWebsite
Imports NVCMS.Modules.HeThong
Imports NVCMS.Modules.LibCRM
Imports NVCMS.Modules.Student

Namespace NVCMS.Modules.Marketing

    Public MustInherit Class ZaloSendViewer
        Inherits Entities.Modules.PortalModuleBase

        Private _zaloCampaignCtl As New Marketing_Zalo_Campaign_Controller
        Private _zaloTemplate As New Marketing_ZNS_TemplateController
        Private _zaloListSdtCtl As New Marketing_Zalo_ListSdt_Controller
        Dim _Events_CatController As New EventsWebsite_CatController
        Dim _EventsController As New EventsWebsiteController

        Private Class ZnsTemplateItem
            Public Property TemplateId As Long
            Public Property TemplateName As String
            Public Property PreviewUrl As String
            Public Property IsActive As Boolean
        End Class

        Private Class SendZnsRequest
            Public Property TemplateId As Long
            'Public Property Phone As String
            'Public Property TemplateData As Dictionary(Of String, Object)
            Public Property Type As String
            Public Property CampaignId As Integer?
            Public Property EventCatId As Integer?
            Public Property EventId As Integer?
            Public Property ContextType As String
            Public Property CreatedBy As String
        End Class

        Private Class SendZnsResponse
            Public Property success As Boolean
            Public Property message As String
            Public Property errorCode As Integer
        End Class

        Private ReadOnly Property RequestedCampaignId As Integer
            Get
                Dim value As Integer = 0
                Integer.TryParse(Request.QueryString("campaignId"), value)
                Return value
            End Get
        End Property

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                Try
                    BindCampaign()
                    BindTemplate()
                    BindddlDanhmuc()
                    lbtSendZalo.CausesValidation = True
                    lbtSendZalo.ValidationGroup = "SendZalo"

                    If RequestedCampaignId > 0 Then
                        ddlCampaign.SelectedValue = RequestedCampaignId.ToString()
                        BindPhoneList(RequestedCampaignId)
                    End If
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub

        Private Sub BindCampaign()
            ddlCampaign.DataSource = _zaloCampaignCtl._GetAll(PortalId)
            ddlCampaign.DataTextField = "Title"
            ddlCampaign.DataValueField = "Id"
            ddlCampaign.DataBind()
            ddlCampaign.Items.Insert(0, New ListItem("-- CHỌN CHIẾN DỊCH --", "0"))
        End Sub

        Private Sub BindddlDanhmuc()
            Dim arr As ArrayList = _Events_CatController.Events_Cat_GetAll(50)
            Me.ddlEventCat.DataSource = arr
            Me.ddlEventCat.DataTextField = "CatName"
            Me.ddlEventCat.DataValueField = "id"
            Me.ddlEventCat.DataBind()
            ddlEventCat.Items.Insert(0, New ListItem("-- CHON SU KIEN --", 0))
        End Sub

        Private Sub BindddlEvent(ByVal itemid As Integer)
            Me.ddlEvent.DataSource = _EventsController.Events_GetAllByCat(itemid, 50)
            Me.ddlEvent.DataTextField = "Title"
            Me.ddlEvent.DataValueField = "id"
            Me.ddlEvent.DataBind()
            Me.ddlEvent.Items.Insert(0, New ListItem("-- CHON DIA DIEM --", 0))
        End Sub

        Public Sub ddlEventCat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEventCat.SelectedIndexChanged
            BindddlEvent(CInt(ddlEventCat.SelectedValue))
        End Sub

        Private Function GetZnsTemplateById(ByVal templateId As Long) As ZnsTemplateItem
            Dim connString As String = ConfigurationManager.ConnectionStrings("SiteSqlServerV1").ConnectionString
            If String.IsNullOrWhiteSpace(connString) Then Return Nothing

            Const sql As String = "SELECT TOP 1 TemplateId, TemplateName, PreviewUrl, IsActive FROM dbo.ZNS_Template WHERE TemplateId = @TemplateId"
            Using conn As New SqlConnection(connString)
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@TemplateId", templateId)
                    conn.Open()
                    Using rd As SqlDataReader = cmd.ExecuteReader()
                        If rd.Read() Then
                            Return New ZnsTemplateItem With {
                                .TemplateId = If(rd("TemplateId") Is DBNull.Value, 0, CLng(rd("TemplateId"))),
                                .TemplateName = If(rd("TemplateName") Is DBNull.Value, "", rd("TemplateName").ToString()),
                                .PreviewUrl = If(rd("PreviewUrl") Is DBNull.Value, "", rd("PreviewUrl").ToString()),
                                .IsActive = Not (rd("IsActive") Is DBNull.Value) AndAlso CBool(rd("IsActive"))
                            }
                        End If
                    End Using
                End Using
            End Using

            Return Nothing
        End Function

        Private Sub BindTemplate()
            ddlTemplate.DataSource = _zaloTemplate._GetAll()
            ddlTemplate.DataTextField = "TemplateName"
            ddlTemplate.DataValueField = "TemplateId"
            ddlTemplate.DataBind()
            ddlTemplate.Items.Insert(0, New ListItem("-- CHỌN TEMPLATE ZNS --", "0"))
        End Sub

        Public Sub ddlCampaign_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCampaign.SelectedIndexChanged
            BindPhoneList(CInt(ddlCampaign.SelectedValue))
        End Sub

        Private Sub BindPhoneList(ByVal campaignId As Integer)
            Dim arr As ArrayList = _zaloListSdtCtl._GetAll(campaignId, "", -1, 0, 100000)
            rptListPhone.DataSource = arr
            rptListPhone.DataBind()
            ltrTotalPhone.Text = arr.Count.ToString()
        End Sub

        Public Sub ddlTemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTemplate.SelectedIndexChanged
            Dim templateId As Long = CLng(ddlTemplate.SelectedValue)
            If templateId <= 0 Then
                txtPreviewUrl.Text = ""
                ifrPreview.Attributes("src") = "about:blank"
                Return
            End If

            Dim tpl As Marketing_ZNS_TemplateInfo = _zaloTemplate._GetByTemplateId(templateId)
            If tpl Is Nothing Then
                txtPreviewUrl.Text = ""
                ifrPreview.Attributes("src") = "about:blank"
                Return
            End If

            txtPreviewUrl.Text = tpl.PreviewUrl
            If String.IsNullOrWhiteSpace(tpl.PreviewUrl) Then
                ifrPreview.Attributes("src") = "about:blank"
            Else
                ifrPreview.Attributes("src") = tpl.PreviewUrl
            End If
        End Sub

        Private Shared Function SendZns(request As SendZnsRequest) As SendCampaignResponse
            Dim apiPath As String = ConfigurationManager.AppSettings("cap_api_url_sendzns")
            If String.IsNullOrWhiteSpace(apiPath) Then
                apiPath = "/api/zns/send-job"
            End If
            Dim result = UltiCapApiClient.Post(Of SendCampaignResponse)(apiPath, request)
            Return result
        End Function

        Private Sub lbtSendZalo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtSendZalo.Click
            Try
                Page.Validate("SendZalo")
                If Not Page.IsValid Then Return

                Dim campaignId As Integer = If(IsNumeric(ddlCampaign.SelectedValue), CInt(ddlCampaign.SelectedValue), 0)
                Dim templateId As Long = If(IsNumeric(ddlTemplate.SelectedValue), CLng(ddlTemplate.SelectedValue), 0)
                Dim eventCatId As Integer = If(IsNumeric(ddlEventCat.SelectedValue), CInt(ddlEventCat.SelectedValue), 0)
                Dim eventId As Integer = If(IsNumeric(ddlEvent.SelectedValue), CInt(ddlEvent.SelectedValue), 0)

                If campaignId <= 0 Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Vui lòng chọn chiến dịch Zalo.');</script>")
                    Return
                End If

                If templateId <= 0 Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Vui lòng chọn template ZNS.');</script>")
                    Return
                End If

                If eventCatId <= 0 Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Vui lòng chọn sự kiện.');</script>")
                    Return
                End If

                If eventId <= 0 Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Vui lòng chọn địa điểm.');</script>")
                    Return
                End If

                Dim arr As ArrayList = _zaloListSdtCtl._GetAll(campaignId, "", -1, 0, 100000)
                If arr Is Nothing OrElse arr.Count = 0 Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Không có số điện thoại trong chiến dịch.');</script>")
                    Return
                End If

                Dim validPhones As New List(Of Marketing_Zalo_ListSdtInfo)()
                Dim seen As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

                For Each item As Marketing_Zalo_ListSdtInfo In arr
                    Dim errMsg As String = String.Empty
                    Dim normalized As String = ZaloPhoneHelper.ValidateAndNormalize(If(item.Phone, String.Empty), errMsg)
                    If String.IsNullOrWhiteSpace(normalized) Then Continue For
                    If seen.Contains(normalized) Then Continue For
                    seen.Add(normalized)
                    validPhones.Add(item)
                Next

                If validPhones.Count = 0 Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Không có số điện thoại hợp lệ để gửi.');</script>")
                    Return
                End If

                Dim request As New SendZnsRequest With {
                    .EventCatId = eventCatId,
                    .EventId = eventId,
                    .TemplateId = templateId,
                    .Type = "Marketing",
                    .CampaignId = campaignId,
                    .CreatedBy = UserId
                }

                Dim result As SendCampaignResponse = SendZns(request)
                If result IsNot Nothing AndAlso result.Success Then
                    BindPhoneList(campaignId)
                    'UpdateSuccess("Đã đưa " & result.TotalRecipient.ToString() & " email vào hàng đợi gửi.")
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Đã đưa " & result.TotalRecipient.ToString() & " email vào hàng đợi gửi.');</script>")
                Else
                    'UpdateError(If(result Is Nothing, "Không nhận được phản hồi từ API.", result.Message))
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Không nhận được phản hồi từ API.');</script>")
                End If
            Catch ex As Exception
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex)
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('" & ex.Message.Replace("'", "") & "');</script>")
            End Try
        End Sub

    End Class

End Namespace