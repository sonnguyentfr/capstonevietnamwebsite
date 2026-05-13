Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.LibCRM

    Public Class Lib_EventsController

        Public Sub Events_Insert(ByVal Title As String, ByVal TitleEN As String, ByVal CODE As String, ByVal Source As String, ByVal Vanphong As Integer, ByVal CatId As Integer, ByVal Avatar As String, ByVal diadiem As String, diadiemEN As String, ByVal fromdatetime As DateTime, ByVal enddatetime As DateTime, ByVal thanhphan As String, thanhphanEN As String, ByVal School As String, Org As String, ByVal Gia As Integer, ByVal Descreption As String, DescreptionEN As String, ByVal LienheName As String, ByVal LienheEmail As String, ByVal LienheMobile As String, ByVal LienheAdd As String, ByVal UserId As Integer, ByVal Portalid As Integer, ByVal Createddate As DateTime, ByVal Isactive As Boolean, ByVal anhbando As String, ByVal linkbando As String, Ordernumber As Integer)
            DataProvider.Instance.Events_Insert(Title, TitleEN, CODE, Source, Vanphong, CatId, Avatar, diadiem, diadiemEN, fromdatetime, enddatetime, thanhphan, thanhphanEN, School, Org, Gia, Descreption, DescreptionEN, LienheName, LienheEmail, LienheMobile, LienheAdd, UserId, Portalid, Createddate, Isactive, anhbando, linkbando, Ordernumber)
        End Sub
        Public Sub Events_Update(ByVal id As Integer, ByVal Title As String, ByVal TitleEN As String, ByVal CODE As String, ByVal Source As String, ByVal Vanphong As Integer, ByVal CatId As Integer, ByVal Avatar As String, ByVal diadiem As String, diadiemEN As String, ByVal fromdatetime As DateTime, ByVal enddatetime As DateTime, ByVal thanhphan As String, thanhphanEN As String, ByVal School As String, Org As String, ByVal Gia As Integer, ByVal Descreption As String, DescreptionEN As String, ByVal LienheName As String, ByVal LienheEmail As String, ByVal LienheMobile As String, ByVal LienheAdd As String, ByVal UserId As Integer, ByVal Portalid As Integer, ByVal Createddate As DateTime, ByVal Isactive As Boolean, ByVal anhbando As String, ByVal linkbando As String, Ordernumber As Integer)
            DataProvider.Instance.Events_Update(id, Title, TitleEN, CODE, Source, Vanphong, CatId, Avatar, diadiem, diadiemEN, fromdatetime, enddatetime, thanhphan, thanhphanEN, School, Org, Gia, Descreption, DescreptionEN, LienheName, LienheEmail, LienheMobile, LienheAdd, UserId, Portalid, Createddate, Isactive, anhbando, linkbando, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Sub Events_Delete(ByVal id As Integer, Portalid As Integer)
            DataProvider.Instance.Events_Delete(id, Portalid)
        End Sub

        '------------------------------------------'
        Public Function Events_GetByID(ByVal id As Integer, Portalid As Integer) As Lib_EventsInfo
            Return CType(CBO.FillObject(Of Lib_EventsInfo)(DataProvider.Instance.Events_GetByID(id, Portalid), True), Lib_EventsInfo)
        End Function

        '------------------------------------------'
        Public Function Events_GetAll(Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_GetAll(Portalid), GetType(Lib_EventsInfo))
        End Function
        '------------------------------------------'
        Public Function Events_GetAllByCat(CatId As Integer, Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_GetAllByCat(CatId, Portalid), GetType(Lib_EventsInfo))
        End Function
        '------------------------------------------'
        Public Function Events_GetAllShowByCat(CatId As Integer, Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_GetAllShowByCat(CatId, Portalid), GetType(Lib_EventsInfo))
        End Function
        Public Function Events_Find_Count(ByVal subtractIds As String, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, CatId As Integer, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal UserId As Integer) As Integer
            Return DataProvider.Instance.Events_Find_Count(subtractIds, datefrom, dateto, title, CatId, PortalId, Isactive, UserId)
        End Function
        '------------------------------------------'
        Public Function Events_Find_Index(ByVal subtractIds As String, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, CatId As Integer, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Find_Index(subtractIds, datefrom, dateto, title, CatId, PortalId, Isactive, UserId, PageIndex, PageSize), GetType(Lib_EventsInfo))
        End Function
        '------------------------------------------'
        Public Function Events_FindShow_Count(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer) As Integer
            Return DataProvider.Instance.Events_FindShow_Count(subtractIds, PortalId, Isactive)
        End Function
        '------------------------------------------'
        Public Function Events_FindShow_Index(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_FindShow_Index(subtractIds, PortalId, Isactive, PageIndex, PageSize), GetType(Lib_EventsInfo))
        End Function
        '------------------------------------------'
        Public Function Events_FindShowPast_Count(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer) As Integer
            Return DataProvider.Instance.Events_FindShowPast_Count(subtractIds, PortalId, Isactive)
        End Function
        '------------------------------------------'
        Public Function Events_FindShowPast_Index(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_FindShowPast_Index(subtractIds, PortalId, Isactive, PageIndex, PageSize), GetType(Lib_EventsInfo))
        End Function
        '------------------------------------------'
        Public Function Events_GetAllShow(Portalid As Integer, Count As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_GetAllShow(Portalid, Count), GetType(Lib_EventsInfo))
        End Function
        '------------------------------------------'
        '------------------------------------------'
        Public Function Events_GetAllOnline(Portalid As Integer, Count As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_GetAllOnline(Portalid, Count), GetType(Lib_EventsInfo))
        End Function
        '------------------------------------------'
        Public Function Events_GetAllShowEnd(Portalid As Integer, Count As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_GetAllShowEnd(Portalid, Count), GetType(Lib_EventsInfo))
        End Function
    End Class

End Namespace