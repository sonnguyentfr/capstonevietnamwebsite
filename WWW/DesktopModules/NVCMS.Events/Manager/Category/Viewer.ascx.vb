Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.EventsWebsite

Namespace DesktopModules.NV_Events.Manager.Event_Cat

    Public MustInherit Class categoriesviewer
        Inherits Entities.Modules.PortalModuleBase

#Region "Controls"
        Dim _EventsWebsite_CatController As New EventsWebsite_CatController
        Dim _EventsWebsiteController As New EventsWebsiteController
        Dim _EventsStudentController As New EventsStudentWebsiteController
        Public Property ItemID() As Integer
            Get
                If Not ViewState("ItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("ItemID"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("ItemID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("ItemID") = Value.ToString
            End Set
        End Property
#End Region

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '
            If Not IsPostBack Then
                Try
                    Dim strlinkcssjs As String = ""
                    strlinkcssjs = "" _
                    & vbCrLf & "<link href='https://crm.capstone.edu.vn/static/admin/assets/css/jquery.countdown.css' rel='stylesheet' />" _
                    & vbCrLf & ""
                    Dim htmlHeaderTags2 = ""
                    Dim htmlHeaderCtrl2 As New LiteralControl()
                    htmlHeaderTags2 = strlinkcssjs
                    htmlHeaderCtrl2.Text = htmlHeaderTags2.ToString()
                    Page.Header.Controls.Add(htmlHeaderCtrl2)
                    BindGridData()
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
#End Region

        Private Sub BindGridData()
            Dim arrNewsCategories As New ArrayList
            arrNewsCategories = _EventsWebsite_CatController.Events_Cat_GetAll(50)
            Me.drgViewData.DataSource = arrNewsCategories
            Me.drgViewData.DataBind()
            Me.ltrcount.Text = arrNewsCategories.Count
        End Sub
        ''' <summary>
        ''' Bind Su kien theo Cat
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub OnItemDataBound(sender As Object, e As RepeaterItemEventArgs)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim catid As String = TryCast(e.Item.FindControl("hdfcatid"), HiddenField).Value
                Dim rptEventinCat As Repeater = TryCast(e.Item.FindControl("rptEventinCat"), Repeater)
                Dim arrevent As New ArrayList
                arrevent = _EventsWebsiteController.Events_GetAllByCat(catid, 50)
                rptEventinCat.DataSource = arrevent
                rptEventinCat.DataBind()
            End If
        End Sub
#Region "edit insert"
        Protected Sub GetInfo(sender As Object, e As EventArgs)
            ItemID = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
            Dim objtag As Events_CatInfo
            objtag = _EventsWebsite_CatController.Events_Cat_GetByID(ItemID, 50)
            If Not objtag Is Nothing Then
                With objtag
                    'lbtDelete.Visible = True
                    'Me.txtCatName.Text = .CatName
                    'txtDesception.Text = .Desception
                    'Me.chkIsActive.Checked = .Isactive
                    'drlTabID.SelectedValue = .TabId
                End With

            End If
            ClientAPI.RegisterStartUpScript(Me.Page, "Modalhoatdong", "<script>Modalhoatdong();</script>")
        End Sub


        'Private Sub lbtUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtUpdate.Click
        '    Try
        '        If ItemID > 0 Then
        '            'Edit
        '            'ctlNewsCategory.Events_Cat_Update(ItemID, Me.txtCatName.Text, Me.txtDesception.Text, DateTime.Now, UserId, PortalId, Me.chkIsActive.Checked, CType(drlTabID.SelectedValue, Integer))
        '        Else
        '            'ctlNewsCategory.Events_Cat_Insert(Me.txtCatName.Text, Me.txtDesception.Text, DateTime.Now, UserId, PortalId, Me.chkIsActive.Checked, CType(drlTabID.SelectedValue, Integer))
        '        End If
        '        'Me.txtCatName.Text = ""
        '        'Me.txtDesception.Text = ""
        '        'drlTabID.SelectedValue = 0
        '        'Me.chkIsActive.Checked = False
        '        ItemID = 0
        '        BindGridData()
        '        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "ModalFollowUpClose();", True)
        '    Catch exc As Exception    'Module failed to load
        '        ProcessModuleLoadException(Me, exc)
        '    End Try
        'End Sub
        'Protected Sub lbtDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtDelete.Click
        '    If ItemID > 0 Then
        '        ctlNewsCategory.Events_Cat_Delete(ItemID, PortalId)
        '        ItemID = 0
        '        BindGridData()
        '        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "Script", "ModalFollowUpClose();", True)
        '    End If
        'End Sub
        'Private Sub lbtAdd_Click(sender As Object, e As EventArgs) Handles lbtAdd.Click, lbtAddTop.Click
        '    BindTabs()
        '    Me.txtCatName.Text = ""
        '    Me.txtDesception.Text = ""

        '    Me.chkIsActive.Checked = False
        '    ItemID = 0
        '    ClientAPI.RegisterStartUpScript(Me.Page, "Modalhoatdong", "<script>Modalhoatdong();</script>")
        'End Sub
#End Region
#Region "function"
#Region "CountSchool"
        Public Function TyLeCheckIn(ByVal Id As Integer) As String
            Dim result As Double = "0"

            Dim total = _EventsStudentController.Events_Student_FindCountByEvent(0, Id, -1, -1)
            Dim icheckin = _EventsStudentController.Events_Student_FindCountByEvent(0, Id, 1, -1)
            If total > 0 Then
                result = icheckin * 100 / total
            End If
            Return result.ToString("F2")
        End Function
        Public Function CheckIn(ByVal Id As Integer) As String
            Dim result As String = "0/0"
            Dim total = _EventsStudentController.Events_Student_FindCountByEvent(0, Id, -1, -1)
            Dim icheckin = _EventsStudentController.Events_Student_FindCountByEvent(0, Id, 1, -1)
            result = icheckin & "/" & total
            Return result
        End Function
        Public Function TyLeCheckIn2(ByVal Id As Integer) As String
            Dim result As Double = "0"

            Dim total = _EventsStudentController.Events_Student_FindCountByEvent(0, Id, -1, -1)
            Dim icheckin = _EventsStudentController.Events_Student_FindCountByEvent(0, Id, 1, -1)
            If total > 0 Then
                result = icheckin * 100 / total
            End If
            Return result.ToString("F0")
        End Function
        Public Function TyLeCheckIn_Event(ByVal eventid As Integer, ByVal EventCatId As Integer) As String
            Dim result As Double = "0"

            Dim total = _EventsStudentController.Events_Student_FindCountByEvent(eventid, EventCatId, -1, -1)
            Dim icheckin = _EventsStudentController.Events_Student_FindCountByEvent(eventid, EventCatId, 1, -1)
            If total > 0 Then
                result = icheckin * 100 / total
            End If
            Return result.ToString("F2")
        End Function
        Public Function CheckIn_Event(ByVal eventid As Integer, ByVal EventCatId As Integer) As String
            Dim result As String = "0/0"
            Dim total = _EventsStudentController.Events_Student_FindCountByEvent(eventid, EventCatId, -1, -1)
            Dim icheckin = _EventsStudentController.Events_Student_FindCountByEvent(eventid, EventCatId, 1, -1)
            result = icheckin & "/" & total
            Return result
        End Function
        Public Function TyLeCheckIn2_Event(ByVal eventid As Integer, ByVal EventCatId As Integer) As String
            Dim result As Double = "0"

            Dim total = _EventsStudentController.Events_Student_FindCountByEvent(eventid, EventCatId, -1, -1)
            Dim icheckin = _EventsStudentController.Events_Student_FindCountByEvent(eventid, EventCatId, 1, -1)
            If total > 0 Then
                result = icheckin * 100 / total
            End If
            Return result.ToString("F0")
        End Function
        Public Function CountSchoolCat(id As Integer) As String
            Dim objCat As Events_CatInfo
            If IsNumeric(id) Then
                objCat = _EventsWebsite_CatController.Events_Cat_GetByID(id, 50)
                If Not objCat Is Nothing Then
                    With objCat
                        If Not String.IsNullOrEmpty(objCat.FairSchool) Then
                            Dim strArr As String() = objCat.FairSchool.Split(CType(",", Char))
                            'Return "[" & strArr.Length & " - trường]"
                            Return ""
                        Else
                            Return ""
                        End If
                    End With
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        End Function
        Public Function CountSchoolEvent(id As Integer) As String
            Dim objCat As EventsInfo
            If IsNumeric(id) Then
                objCat = _EventsWebsiteController.Events_GetByID(id, 50)
                If Not objCat Is Nothing Then
                    With objCat
                        If Not String.IsNullOrEmpty(objCat.School) Then
                            Dim strArr As String() = objCat.School.Split(CType(",", Char))
                            Return "[" & strArr.Length - 1 & " - trường]"
                        Else
                            Return ""
                        End If
                    End With
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        End Function
        Public Function CoutDowntime(id As Integer) As String
            Dim obj As EventsInfo
            Dim sresult As String = ""
            obj = _EventsWebsiteController.Events_GetByID(id, 50)
            If Not obj Is Nothing Then
                With obj
                    If .fromdatetime >= DateTime.Now Then
                        Dim strFlv As String = "<script type='text/javascript'> " _
            & "$(function () {" _
            & "var austDay = new Date();" _
            & "austDay = new Date(__videoLink__);" _
            & "$('#defaultCountdown____id___').countdown({until: austDay});" _
            & "$('#year').text(austDay.getFullYear());" _
            & "});" _
            & "</script>"
                        strFlv = strFlv.Replace("__videoLink__", obj.fromdatetime.ToString("yyyy") & "," & CInt(obj.fromdatetime.ToString("MM")) - 1 & "," & obj.fromdatetime.ToString("dd") & "," & obj.fromdatetime.ToString("HH") & "," & obj.fromdatetime.ToString("mm"))
                        strFlv = strFlv.Replace("____id___", obj.id)
                        sresult = strFlv
                    End If

                End With
            End If

            Return sresult
        End Function
#End Region
#End Region
    End Class

End Namespace
