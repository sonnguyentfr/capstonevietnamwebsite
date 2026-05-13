Imports System.Text.RegularExpressions
Imports NVCMS.Modules.EventsWebsite
Public Class SuKienZ
    Public Shared StatusOnline As Integer = 1
    Public Shared StatusOffline As Integer = 2
    Public Shared StatusCu As Integer = 0
    Public Shared StatusTelesale As Integer = 3
    Public Shared Function StatusNguon(ByVal id As Integer) As String
        If String.IsNullOrEmpty(id) Or CStr(id) = "" Then
            Return ""
        End If
        Dim childAgeAsInt As Integer
        If Integer.TryParse(id, childAgeAsInt) Then
            Select Case id
                Case 0
                    Return "Online Cũ"
                Case 1
                    Return "Online mới"
                Case 2
                    Return "Tại sự kiện"
                Case 3
                    Return "Telelsale"
                Case Else
                    Return "-"
            End Select
        Else
            Return "-"
        End If
        Return ""
    End Function
    Public Shared Function GetEventCatName(ByVal EventCatId As Integer) As String
        If String.IsNullOrEmpty(EventCatId) Or CStr(EventCatId) = "" Then
            Return ""
        End If
        Dim childAgeAsInt As Integer
        If Integer.TryParse(EventCatId, childAgeAsInt) Then
            If EventCatId = 0 Then
                Return ""
            Else
                Dim ctlEventCat As New EventsWebsite_CatController
                Dim objEventCat As Events_CatInfo
                objEventCat = ctlEventCat.Events_Cat_GetByID(EventCatId, 50)
                With objEventCat
                    Return objEventCat.CatName
                End With
            End If
            Return "-"
        Else
            Return "-"
        End If
        Return ""
    End Function
    Public Shared Function GetEventCatNameEN(ByVal EventCatId As Integer) As String
        If String.IsNullOrEmpty(EventCatId) Or CStr(EventCatId) = "" Then
            Return ""
        End If
        Dim childAgeAsInt As Integer
        If Integer.TryParse(EventCatId, childAgeAsInt) Then
            If EventCatId = 0 Then
                Return ""
            Else
                Dim ctlEventCat As New EventsWebsite_CatController
                Dim objEventCat As Events_CatInfo
                objEventCat = ctlEventCat.Events_Cat_GetByID(EventCatId, 50)
                If Not objEventCat Is Nothing Then
                    With objEventCat
                        Return objEventCat.CatNameEN
                    End With
                Else
                    Return "-"
                End If

            End If
            Return "-"
        Else
            Return "-"
        End If
        Return ""
    End Function
    Public Shared Function IsValidEmail(email As String) As Boolean
        Return Regex.IsMatch(email, "^([0-9a-z]+[-._+&])*[0-9a-z]+@([-0-9a-z]+[.])+[a-z]{2,6}$", RegexOptions.IgnoreCase)
    End Function
    Public Shared Function GetEventName(ByVal EventId As Integer) As String
        If String.IsNullOrEmpty(EventId) Or CStr(EventId) = "" Then
            Return ""
        End If
        Dim childAgeAsInt As Integer
        If Integer.TryParse(EventId, childAgeAsInt) Then
            If EventId = 0 Then
                Return ""
            Else
                Dim ctlEventCat As New EventsWebsiteController
                Dim objEventCat As EventsInfo
                objEventCat = ctlEventCat.Events_GetByID(EventId, 50)
                If Not objEventCat Is Nothing Then
                    With objEventCat
                        Return objEventCat.Title
                    End With
                Else
                    Return "-"
                End If

            End If
            Return "-"
        Else
            Return "-"
        End If
        Return ""
    End Function
    Public Shared Function GetEventNameEN(ByVal EventId As Integer) As String
        If String.IsNullOrEmpty(EventId) Or CStr(EventId) = "" Then
            Return ""
        End If
        Dim childAgeAsInt As Integer
        If Integer.TryParse(EventId, childAgeAsInt) Then
            If EventId = 0 Then
                Return ""
            Else
                Dim ctlEventCat As New EventsWebsiteController
                Dim objEventCat As EventsInfo
                objEventCat = ctlEventCat.Events_GetByID(EventId, 50)
                If Not objEventCat Is Nothing Then
                    With objEventCat
                        Return objEventCat.TitleEN
                    End With
                Else
                    Return "-"
                End If

            End If
            Return "-"
        Else
            Return "-"
        End If
        Return ""
    End Function
    Public Shared Function GetEventCatId(ByVal EventId As Integer) As Integer
        If String.IsNullOrEmpty(EventId) Or CStr(EventId) = "" Then
            Return 0
        End If
        Dim childAgeAsInt As Integer
        If Integer.TryParse(EventId, childAgeAsInt) Then
            If EventId = 0 Then
                Return 0
            Else
                Dim ctlEventCat As New EventsWebsiteController
                Dim objEventCat As EventsInfo
                objEventCat = ctlEventCat.Events_GetByID(EventId, 50)
                With objEventCat
                    Return objEventCat.CatId
                End With
            End If
            Return 0
        Else
            Return 0
        End If
        Return ""
    End Function
    'Public Shared Function ParticipationTitle(ByVal id As Integer) As String
    '    If String.IsNullOrEmpty(id) Or CStr(id) = "" Then
    '        Return ""
    '    End If
    '    Dim childAgeAsInt As Integer
    '    If Integer.TryParse(id, childAgeAsInt) Then
    '        If id = 0 Then
    '            Return ""
    '        Else
    '            Dim ctlEventCat As New ParticipationController
    '            Dim objEventCat As ParticipationInfo
    '            objEventCat = ctlEventCat.RegParticipation_GetByID(id)
    '            With objEventCat
    '                Return objEventCat.Title
    '            End With
    '        End If
    '        Return "-"
    '    Else
    '        Return "-"
    '    End If
    '    Return ""
    'End Function
    'Public Shared Function ParticipationParentTitle(ByVal id As Integer) As String
    '    If String.IsNullOrEmpty(id) Or CStr(id) = "" Then
    '        Return ""
    '    End If
    '    Dim childAgeAsInt As Integer
    '    If Integer.TryParse(id, childAgeAsInt) Then
    '        If id = 0 Then
    '            Return ""
    '        Else
    '            Dim ctlEventCat As New ParticipationController
    '            Dim objEventCat As ParticipationInfo
    '            objEventCat = ctlEventCat.RegParticipation_GetByID(id)
    '            With objEventCat
    '                If .ParentId = 0 Then
    '                    Return ""
    '                Else
    '                    Dim objEventCat2 As ParticipationInfo
    '                    objEventCat2 = ctlEventCat.RegParticipation_GetByID(.ParentId)
    '                    With objEventCat2
    '                        Return objEventCat2.Title
    '                    End With
    '                End If
    '            End With
    '        End If
    '        Return "-"
    '    Else
    '        Return "-"
    '    End If
    '    Return ""
    'End Function
    'Public Shared Function AdvertisingTitle(ByVal id As Integer) As String
    '    If String.IsNullOrEmpty(id) Or CStr(id) = "" Then
    '        Return ""
    '    End If
    '    Dim childAgeAsInt As Integer
    '    If Integer.TryParse(id, childAgeAsInt) Then
    '        If id = 0 Then
    '            Return ""
    '        Else
    '            Dim ctlEventCat As New AdvertisingController
    '            Dim objEventCat As AdvertisingInfo
    '            objEventCat = ctlEventCat.RegAdvertising_GetByID(id)
    '            With objEventCat
    '                Return objEventCat.Title
    '            End With
    '        End If
    '        Return "-"
    '    Else
    '        Return "-"
    '    End If
    '    Return ""
    'End Function
    '-----------------------------
    Public Shared Function TypeofInstitution(ByVal id As Integer) As String
        If String.IsNullOrEmpty(id) Or CStr(id) = "" Then
            Return ""
        End If
        Dim childAgeAsInt As Integer
        If Integer.TryParse(id, childAgeAsInt) Then
            Select Case id
                Case 0
                    Return ""
                Case 1
                    Return "2-year College"
                Case 2
                    Return "4-year College/University"
                Case 3
                    Return "English Language Programs"
                Case 4
                    Return "Boarding/ Day school"
                Case Else
                    Return "-"
            End Select
        Else
            Return "-"
        End If
        Return ""
    End Function
    '-----------------------------
End Class