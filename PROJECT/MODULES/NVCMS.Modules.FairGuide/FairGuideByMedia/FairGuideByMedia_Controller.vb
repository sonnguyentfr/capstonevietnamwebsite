'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/27/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.FairGuide

    Public Class FairGuideByMediaController

        Public Sub _Insert(ByVal FairGuideId As Integer, ByVal mediaid As Integer, ByVal ordernumber As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            DataProvider.Instance.FairGuideByMedia_Insert(FairGuideId, mediaid, ordernumber, createdted, userid, portalid)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal id As Integer, ByVal FairGuideId As Integer, ByVal mediaid As Integer, ByVal ordernumber As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            DataProvider.Instance.FairGuideByMedia_Update(id, FairGuideId, mediaid, ordernumber, createdted, userid, portalid)
        End Sub
        '------------------------------------------'
        Public Sub _UpdateFairGuideId(ByVal FairGuideId As Integer, FairGuideIdnew As Integer)
            DataProvider.Instance.FairGuideByMedia_UpdateFairGuideId(FairGuideId, FairGuideIdnew)
        End Sub
        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer, portalid As Integer)
            DataProvider.Instance.FairGuideByMedia_Delete(id, portalid)
        End Sub
        '------------------------------------------'
        Public Sub _DeleteByFairGuideId(ByVal FairGuideId As Integer, portalid As Integer)
            DataProvider.Instance.FairGuideByMedia_DeleteByFairGuideId(FairGuideId, portalid)
        End Sub
        '------------------------------------------'
        Public Sub _DeleteByMediaId(ByVal Mediaid As Integer, portalid As Integer)
            DataProvider.Instance.FairGuideByMedia_DeleteByMediaId(Mediaid, portalid)
        End Sub
        '------------------------------------------'
        Public Function _GetByID(ByVal id As Integer, portalid As Integer) As FairGuideByMediaInfo
            Return CType(CBO.FillObject(Of FairGuideByMediaInfo)(DataProvider.Instance.FairGuideByMedia_GetByID(id, portalid), True), FairGuideByMediaInfo)
        End Function

        '------------------------------------------'
        Public Function _GetAllByFairGuideId(ByVal FairGuideId As Integer, ByVal portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FairGuideByMedia_GetAllByFairGuideId(FairGuideId, portalid), GetType(FairGuideByMediaInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace
