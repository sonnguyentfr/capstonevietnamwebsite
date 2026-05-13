Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.LibCRM

    Public Class Lib_EventsStudentController
        Private Sub ClearCache()
            DataCache.ClearCache("Events_Static")
            DataCache.ClearCache("Events_Student")
        End Sub
        Public Sub Events_Student_Insert(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal StudentCode As String, ByVal Source As Integer, ByVal Nguon As String, ByVal CreatedDate As DateTime, ByVal PortalId As Integer, NguonTutao As String)
            DataProvider.Instance.Events_Student_Insert(EventId, EventCatId, StudentId, StudentCode, Source, Nguon, CreatedDate, PortalId, NguonTutao)
            ClearCache()
        End Sub

        '------------------------------------------'
        Public Sub Events_Student_UpdateCheckIn(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, Nguoidikem As Integer, ByVal Checkin As Boolean, ByVal CheckInDate As DateTime, ByVal UserId As Integer)
            DataProvider.Instance.Events_Student_UpdateCheckIn(EventId, EventCatId, StudentId, Nguoidikem, Checkin, CheckInDate, UserId)
            ClearCache()
        End Sub
        '------------------------------------------'
        Public Sub Events_Student_UpdateThamdu(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal Thamdu As Boolean, ByVal ThamduDateUpdate As DateTime, ByVal ThamduUserUpdate As Integer)
            DataProvider.Instance.Events_Student_UpdateThamdu(EventId, EventCatId, StudentId, Thamdu, ThamduDateUpdate, ThamduUserUpdate)
            ClearCache()
        End Sub
        '------------------------------------------'
        Public Sub Events_Student_UpdateCheckInAfterFair(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal Checkin As Boolean, ByVal CheckInDate As DateTime, ByVal UserId As Integer)
            DataProvider.Instance.Events_Student_UpdateCheckInAfterFair(EventId, EventCatId, StudentId, Checkin, CheckInDate, UserId)
            ClearCache()
        End Sub
        '------------------------------------------'
        Public Sub Events_Student_UpdateStudentNguon(ByVal EventId As Integer, ByVal StudentId As Integer, ByVal Nguon As String)
            DataProvider.Instance.Events_Student_UpdateStudentNguon(EventId, StudentId, Nguon)
            ClearCache()
        End Sub
        '------------------------------------------'
        Public Sub Events_Student_UpdateStudentNguonTutao(ByVal EventId As Integer, ByVal StudentId As Integer, ByVal NguonTutao As String)
            DataProvider.Instance.Events_Student_UpdateStudentNguonTutao(EventId, StudentId, NguonTutao)
            ClearCache()
        End Sub
        '------------------------------------------'
        Public Sub Events_Student_UpdateCheckInInsert(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal StudentCode As String, Source As Integer, Nguoidikem As Integer, ByVal Checkin As Boolean, ByVal CreatedDate As DateTime, Portalid As Integer, ByVal CheckInDate As DateTime, ByVal UserId As Integer)
            DataProvider.Instance.Events_Student_UpdateCheckInInsert(EventId, StudentId, EventCatId, StudentCode, Source, Nguoidikem, Checkin, CreatedDate, Portalid, CheckInDate, UserId)
            ClearCache()
        End Sub
        '------------------------------------------'
        Public Function Events_Student_GetAllByEvent(EventId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Student_GetAllByEvent(EventId), GetType(Lib_EventsStudentInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Student_GetAllByStudent(StudentId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Student_GetAllByStudent(StudentId), GetType(Lib_EventsStudentInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Student_GetAllByEventCheckIn(EventId As Integer, Source As String, checkin As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Student_GetAllByEventCheckIn(EventId, Source, checkin), GetType(Lib_EventsStudentInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Student_SelectAllByEventCheckInbySource(EventId As Integer, CheckIn As Boolean, Source As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Student_SelectAllByEventCheckInbySource(EventId, CheckIn, Source), GetType(Lib_EventsStudentInfo))
        End Function
        Public Function Events_Student_GetById(ByVal Id As Integer) As Lib_EventsStudentInfo
            Return CType(CBO.FillObject(Of Lib_EventsStudentInfo)(DataProvider.Instance.Events_Student_GetById(Id), True), Lib_EventsStudentInfo)
        End Function
        '------------------------------------------'
        Public Function Events_Student_GetAllByEventCat(EventCatId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Student_GetAllByEventCat(EventCatId), GetType(Lib_EventsStudentInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Student_GetCountByEventCat(datetime As DateTime, EventCatId As Integer) As Integer
            Return DataProvider.Instance.Events_Student_GetCountByEventCat(datetime, EventCatId)
        End Function
        '------------------------------------------'
        Public Function Events_Student_GetCountByEvent(datetime As DateTime, EventId As Integer) As Integer
            Return DataProvider.Instance.Events_Student_GetCountByEvent(datetime, EventId)
        End Function
        '------------------------------------------'
        Public Function Events_Student_SelectAllByEventCatCheckInbySource(EventCatId As Integer, CheckIn As Boolean, Source As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Student_SelectAllByEventCatCheckInbySource(EventCatId, CheckIn, Source), GetType(Lib_EventsStudentInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Student_SelectAllByEventCatbySource(EventCatId As Integer, Source As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Student_SelectAllByEventCatbySource(EventCatId, Source), GetType(Lib_EventsStudentInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Student_SelectAllByEventCatbyNguon(EventCatId As Integer, CheckIn As Integer, Nguon As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Student_SelectAllByEventCatbyNguon(EventCatId, CheckIn, Nguon), GetType(Lib_EventsStudentInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Student_SelectAllByEventCatbyNguonTutao(EventCatId As Integer, CheckIn As Integer, NguonTutao As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Student_SelectAllByEventCatbyNguonTutao(EventCatId, CheckIn, NguonTutao), GetType(Lib_EventsStudentInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Student_SelectAllByEventbyNguon(EventId As Integer, CheckIn As Integer, Nguon As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Student_SelectAllByEventbyNguon(EventId, CheckIn, Nguon), GetType(Lib_EventsStudentInfo))
        End Function
        Public Function Events_Student_FindCountByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer) As Integer
            Return DataProvider.Instance.Events_Student_FindCountByEvent(EventId, EventCatId, Checkin, Source)
        End Function
        '------------------------------------------'
        Public Function Events_Student_FindIndexByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Dim stringcache = "Events_Student" & EventId & EventCatId & Checkin & Source & PageIndex & PageSize
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Events_Student_FindIndexByEvent(EventId, EventCatId, Checkin, Source, PageIndex, PageSize), GetType(Lib_EventsStudentInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)
            'Return CBO.FillCollection(DataProvider.Instance.Events_Student_FindIndexByEvent(EventId, EventCatId, Checkin, Source, PageIndex, PageSize), GetType(Lib_EventsStudentInfo))
        End Function
        '------------------------------------------'
        Public Sub Events_Student_DeleteStudentEventId(ByVal EventId As Integer, StudentId As Integer)
            DataProvider.Instance.Events_Student_DeleteStudentEventId(EventId, StudentId)
            ClearCache()
        End Sub
        '------------------------------------------'
        Public Function Events_Student_SelectByEventstudentid(EventCatId As Integer, studentid As Integer) As Lib_EventsStudentInfo
            Return CType(CBO.FillObject(Of Lib_EventsStudentInfo)(DataProvider.Instance.Events_Student_SelectByEventstudentid(EventCatId, studentid), True), Lib_EventsStudentInfo)
        End Function
    End Class

End Namespace