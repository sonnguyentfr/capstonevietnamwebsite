Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :SonNguyen 
'Created Date   :23/07/2016
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.TrangGioiThieu

    Public Class GioiThieu_Controller

        Public Sub _Insert(ByVal TrangDanhMuc As String, ByVal Tieudephu As String, ByVal ImagePath As String, ByVal tomtat As String, ByVal Noidung As String, ByVal Link As String, ByVal ParentId As Integer, ByVal Ordernumber As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.PageGioiThieu_Insert(TrangDanhMuc, Tieudephu, ImagePath, tomtat, Noidung, Link, ParentId, Ordernumber, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal id As Integer, ByVal TrangDanhMuc As String, ByVal Tieudephu As String, ByVal ImagePath As String, ByVal tomtat As String, ByVal Noidung As String, ByVal Link As String, ByVal ParentId As Integer, ByVal Ordernumber As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.PageGioiThieu_Update(id, TrangDanhMuc, Tieudephu, ImagePath, tomtat, Noidung, Link, ParentId, Ordernumber, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.PageGioiThieu_Delete(id, PortalId)
        End Sub

        '------------------------------------------'
        Public Function _GetByID(ByVal id As Integer, ByVal PortalId As Integer) As GioiThieu_Info
            Return CType(CBO.FillObject(Of GioiThieu_Info)(DataProvider.Instance.PageGioiThieu_GetByID(id, PortalId), True), GioiThieu_Info)
        End Function

        '------------------------------------------'
        Public Function _GetAll(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.PageGioiThieu_GetAll(PortalId), GetType(GioiThieu_Info))
        End Function
        '------------------------------------------'
        Public Function _GetAllByParentId(ByVal ParentId As Integer, ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.PageGioiThieu_GetAllByParentId(ParentId, PortalId), GetType(GioiThieu_Info))
        End Function
        '------------------------------------------'
    End Class

End Namespace