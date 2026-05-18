Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.Banner

    Public Class BannerAdv_VitriController
        Public Sub _Vitri_Insert(ByVal Title As String, ByVal width As Integer, ByVal height As Integer, ByVal Images As String, ByVal CreatedByUserId As Integer, ByVal CreatedOnDate As DateTime, ByVal LastModifiedByUserId As Integer, ByVal LastModifiedOnDate As DateTime, ByVal ModuleId As Integer, ByVal portalid As Integer)
            DataProvider.Instance._Vitri_Insert(Title, width, height, Images, CreatedByUserId, CreatedOnDate, LastModifiedByUserId, LastModifiedOnDate, ModuleId, portalid)
        End Sub

        '------------------------------------------'
        Public Sub _Vitri_Update(id As Integer, ByVal Title As String, ByVal width As Integer, ByVal height As Integer, Images As String, ByVal LastModifiedByUserId As Integer, ByVal LastModifiedOnDate As DateTime)
            DataProvider.Instance._Vitri_Update(id, Title, width, height, Images, LastModifiedByUserId, LastModifiedOnDate)
        End Sub

        '------------------------------------------'
        Public Sub _Vitri_Delete(ByVal id As Integer)
            DataProvider.Instance._Vitri_Delete(id)
        End Sub

        '------------------------------------------'
        Public Function _Vitri_GetByID(ByVal id As Integer) As BannerAdv_VitriInfo
            Return CType(CBO.FillObject(Of BannerAdv_VitriInfo)(DataProvider.Instance._Vitri_GetByID(id), True), BannerAdv_VitriInfo)
        End Function

        '------------------------------------------'
        Public Function _Vitri_GetAll(portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance._Vitri_GetAll(portalid), GetType(BannerAdv_VitriInfo))
        End Function

        '------------------------------------------'
    End Class

End Namespace