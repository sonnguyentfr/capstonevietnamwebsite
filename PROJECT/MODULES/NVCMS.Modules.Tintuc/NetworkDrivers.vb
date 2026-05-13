'==============================================================================================================
'AejW.com - Network Drives
'-------------------------
'Build:             0015
'Author:            Adam ej Woods
'Modified:          14/05/2004
'Ownership:         Copyright (c)2004 Adam ej Woods
'Source:            http://www.aejw.com/
'EULA:              In no way can this class be disturbed without my permission, this means reposting on a
'                   web site, cdrom, or any other form of media. The code can be used for commercial or
'                   personal purposes, as long as credit is given to the author. The header (this information)
'                   can not be modified or removed. www.CodeProject.com has permission to disturbe this class.
'==============================================================================================================
Imports System.Runtime.InteropServices

Namespace aejw.Network
    ''' <summary>
    ''' AejW.com - Network Drive Interface
    ''' </summary>
    Public Class NetworkDrive

#Region "API"
        <DllImport("mpr.dll")> _
        Private Shared Function WNetAddConnection2A(ByRef pstNetRes As structNetResource, psPassword As String, psUsername As String, piFlags As Integer) As Integer
        End Function
        <DllImport("mpr.dll")> _
        Private Shared Function WNetCancelConnection2A(psName As String, piFlags As Integer, pfForce As Integer) As Integer
        End Function
        <DllImport("mpr.dll")> _
        Private Shared Function WNetConnectionDialog(phWnd As Integer, piType As Integer) As Integer
        End Function
        <DllImport("mpr.dll")> _
        Private Shared Function WNetDisconnectDialog(phWnd As Integer, piType As Integer) As Integer
        End Function
        <DllImport("mpr.dll")> _
        Private Shared Function WNetRestoreConnectionW(phWnd As Integer, psLocalDrive As String) As Integer
        End Function

        <StructLayout(LayoutKind.Sequential)> _
        Private Structure structNetResource
            Public iScope As Integer
            Public iType As Integer
            Public iDisplayType As Integer
            Public iUsage As Integer
            Public sLocalName As String
            Public sRemoteName As String
            Public sComment As String
            Public sProvider As String
        End Structure

        Private Const RESOURCETYPE_DISK As Integer = &H1

        'Standard	
        Private Const CONNECT_INTERACTIVE As Integer = &H8
        Private Const CONNECT_PROMPT As Integer = &H10
        Private Const CONNECT_UPDATE_PROFILE As Integer = &H1
        'IE4+
        Private Const CONNECT_REDIRECT As Integer = &H80
        'NT5 only
        Private Const CONNECT_COMMANDLINE As Integer = &H800
        Private Const CONNECT_CMD_SAVECRED As Integer = &H1000

#End Region

#Region "Propertys and options"
        Private lf_SaveCredentials As Boolean = False
        ''' <summary>
        ''' Option to save credentials are reconnection...
        ''' </summary>
        Public Property SaveCredentials() As Boolean
            Get
                Return (lf_SaveCredentials)
            End Get
            Set(value As Boolean)
                lf_SaveCredentials = value
            End Set
        End Property
        Private lf_Persistent As Boolean = False
        ''' <summary>
        ''' Option to reconnect drive after log off / reboot ...
        ''' </summary>
        Public Property Persistent() As Boolean
            Get
                Return (lf_Persistent)
            End Get
            Set(value As Boolean)
                lf_Persistent = value
            End Set
        End Property
        Private lf_Force As Boolean = False
        ''' <summary>
        ''' Option to force connection if drive is already mapped...
        ''' or force disconnection if network path is not responding...
        ''' </summary>
        Public Property Force() As Boolean
            Get
                Return (lf_Force)
            End Get
            Set(value As Boolean)
                lf_Force = value
            End Set
        End Property
        Private ls_PromptForCredentials As Boolean = False
        ''' <summary>
        ''' Option to prompt for user credintals when mapping a drive
        ''' </summary>
        Public Property PromptForCredentials() As Boolean
            Get
                Return (ls_PromptForCredentials)
            End Get
            Set(value As Boolean)
                ls_PromptForCredentials = value
            End Set
        End Property

        Private ls_Drive As String = "s:"
        ''' <summary>
        ''' Drive to be used in mapping / unmapping...
        ''' </summary>
        Public Property LocalDrive() As String
            Get
                Return (ls_Drive)
            End Get
            Set(value As String)
                If value.Length >= 1 Then
                    ls_Drive = value.Substring(0, 1) & ":"
                Else
                    ls_Drive = ""
                End If
            End Set
        End Property
        Private ls_ShareName As String = "\\Computer\C$"
        ''' <summary>
        ''' Share address to map drive to.
        ''' </summary>
        Public Property ShareName() As String
            Get
                Return (ls_ShareName)
            End Get
            Set(value As String)
                ls_ShareName = value
            End Set
        End Property
#End Region

#Region "Function mapping"
        ''' <summary>
        ''' Map network drive
        ''' </summary>
        Public Sub MapDrive()
            zMapDrive(Nothing, Nothing)
        End Sub
        ''' <summary>
        ''' Map network drive (using supplied Password)
        ''' </summary>
        Public Sub MapDrive(Password As String)
            zMapDrive(Nothing, Password)
        End Sub
        ''' <summary>
        ''' Map network drive (using supplied Username and Password)
        ''' </summary>
        Public Sub MapDrive(Username As String, Password As String)
            zMapDrive(Username, Password)
        End Sub
        ''' <summary>
        ''' Unmap network drive
        ''' </summary>
        Public Sub UnMapDrive()
            zUnMapDrive(Me.lf_Force)
        End Sub
        ''' <summary>
        ''' Check / restore persistent network drive
        ''' </summary>
        Public Sub RestoreDrives()
            zRestoreDrive()
        End Sub
#End Region

#Region "Core functions"

        ' Map network drive
        Private Sub zMapDrive(psUsername As String, psPassword As String)
            'create struct data
            Dim stNetRes As New structNetResource()
            stNetRes.iScope = 2
            stNetRes.iType = RESOURCETYPE_DISK
            stNetRes.iDisplayType = 3
            stNetRes.iUsage = 1
            stNetRes.sRemoteName = ls_ShareName
            stNetRes.sLocalName = ls_Drive
            'prepare params
            Dim iFlags As Integer = 0
            If lf_SaveCredentials Then
                iFlags += CONNECT_CMD_SAVECRED
            End If
            If lf_Persistent Then
                iFlags += CONNECT_UPDATE_PROFILE
            End If
            If ls_PromptForCredentials Then
                iFlags += CONNECT_INTERACTIVE + CONNECT_PROMPT
            End If
            If psUsername = "" Then
                psUsername = Nothing
            End If
            If psPassword = "" Then
                psPassword = Nothing
            End If
            'if force, unmap ready for new connection
            If lf_Force Then
                Try
                    zUnMapDrive(True)
                Catch
                End Try
            End If
            'call and return
            Dim i As Integer = WNetAddConnection2A(stNetRes, psPassword, psUsername, iFlags)
            If i > 0 Then
                Throw New System.ComponentModel.Win32Exception(i)
            End If
        End Sub

        ' Unmap network drive
        Private Sub zUnMapDrive(pfForce As Boolean)
            'call unmap and return
            Dim iFlags As Integer = 0
            If lf_Persistent Then
                iFlags += CONNECT_UPDATE_PROFILE
            End If
            Dim i As Integer = WNetCancelConnection2A(ls_Drive, iFlags, Convert.ToInt32(pfForce))
            If i > 0 Then
                Throw New System.ComponentModel.Win32Exception(i)
            End If
        End Sub
        ' Check / Restore a network drive
        Private Sub zRestoreDrive()
            'call restore and return
            Dim i As Integer = WNetRestoreConnectionW(0, Nothing)
            If i > 0 Then
                Throw New System.ComponentModel.Win32Exception(i)
            End If
        End Sub

#End Region

    End Class
End Namespace