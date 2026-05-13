Imports System.Web.UI.WebControls
Imports NVCMS.Modules.Lib.FollowUp
Imports NVCMS.Modules.LibCRM

Public Class BLStudent
    Public Shared _LocationController As New LibLocationController
    Public Shared _FollowUpTrangThaiController As New FollowUpTrangThaiController
#Region "Search Toolbox"
    Public Shared Sub Search_BindStudentStatus(ByVal ddl As DropDownList)
        Dim arrStudentStatus As New ArrayList
        arrStudentStatus = _FollowUpTrangThaiController.Follow_TrangThaI_GetAll(False)
        ddl.DataSource = arrStudentStatus
        ddl.DataValueField = "id"
        ddl.DataTextField = "Title"
        ddl.DataBind()
    End Sub
    Public Shared Sub Search_BindYearFrom(ByVal ddl As DropDownList)
        ddl.Items.Insert(0, New ListItem("Từ", 0))
        For i As Integer = 1900 To DateTime.Now.Year
            ddl.Items.Add(New ListItem(i.ToString, i))
        Next
    End Sub
    Public Shared Sub Search_BindYearTo(ByVal ddl As DropDownList)
        ddl.Items.Insert(0, New ListItem("Đến", DateTime.Now.Year))
        For i As Integer = 1970 To DateTime.Now.Year
            ddl.Items.Add(New ListItem(i.ToString, i))
        Next
    End Sub
    Public Shared Sub Search_Bindsex(ByVal ddlSex As DropDownList)
        ddlSex.Items.Insert(0, New ListItem("- GIỚI TÍNH --", -1))
        ddlSex.Items.Insert(1, New ListItem("NAM", 1))
        ddlSex.Items.Insert(2, New ListItem("NỮ", 0))
        ddlSex.DataBind()
    End Sub
    Public Shared Sub Search_BindDDLLocation(ByVal ParentId As Integer, ByVal rdcLocation As DropDownList)
        Dim arrNewsCategories As New ArrayList
        arrNewsCategories = _LocationController.Location_SelectByParentId(ParentId, 0)
        rdcLocation.DataSource = arrNewsCategories
        rdcLocation.DataValueField = "id"
        rdcLocation.DataTextField = "Name"
        rdcLocation.DataBind()
    End Sub
    Public Shared Sub Search_BindTuvanBacHoc(ByVal ddlBachocmongmuon As DropDownList)
        ddlBachocmongmuon.Items.Insert(0, New ListItem("--CHỌN BẬC HỌC MONG MUỐN --", 0))
        ddlBachocmongmuon.Items.Insert(1, New ListItem("Trung Học", 1))
        ddlBachocmongmuon.Items.Insert(2, New ListItem("Đại Học", 2))
        ddlBachocmongmuon.Items.Insert(3, New ListItem("Sau Đại Học", 3))
        ddlBachocmongmuon.Items.Insert(4, New ListItem("Cao Đẳng Cộng Đồng", 4))
        ddlBachocmongmuon.Items.Insert(5, New ListItem("A-Level", 5))
        ddlBachocmongmuon.Items.Insert(6, New ListItem("Học Tiếng Anh", 6))
        ddlBachocmongmuon.Items.Insert(7, New ListItem("Chương Trình Hè", 7))
        ddlBachocmongmuon.DataBind()
    End Sub
    Public Shared Sub Search_BindTuvanChiTra(ByVal ddlTuvanChiTra As DropDownList)
        ddlTuvanChiTra.Items.Insert(0, New ListItem("--CHỌN MỨC KHẢ NĂNG CHI TRẢ --", 0))
        ddlTuvanChiTra.Items.Insert(1, New ListItem("$10.000-$15.000", 1))
        ddlTuvanChiTra.Items.Insert(2, New ListItem("$15.000-$20.000", 2))
        ddlTuvanChiTra.Items.Insert(3, New ListItem("$20.000-$25.000", 3))
        ddlTuvanChiTra.Items.Insert(4, New ListItem("$25.000-$30.000", 4))
        ddlTuvanChiTra.Items.Insert(5, New ListItem("$30.000-$35.000", 5))
        ddlTuvanChiTra.Items.Insert(6, New ListItem("$35.000-$40.000", 6))
        ddlTuvanChiTra.Items.Insert(7, New ListItem("$40.000-$45.000", 7))
        ddlTuvanChiTra.Items.Insert(8, New ListItem("$45.000-$50.000", 8))
        ddlTuvanChiTra.Items.Insert(9, New ListItem("> $50.000", 9))
        ddlTuvanChiTra.DataBind()
    End Sub
    Public Shared Sub Search_Event_BindCheckin(ddlEventCheckIn As DropDownList)
        ddlEventCheckIn.Items.Insert(0, New ListItem("- TẤT CẢ-", -1))
        ddlEventCheckIn.Items.Insert(1, New ListItem("CHECK-IN", 1))
        ddlEventCheckIn.Items.Insert(2, New ListItem("KHÔNG", 0))
        ddlEventCheckIn.DataBind()
    End Sub
#End Region
End Class