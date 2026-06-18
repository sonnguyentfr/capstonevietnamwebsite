Imports System.ComponentModel.DataAnnotations
Imports DotNetNuke.UI.WebControls

Namespace NVCMS.API.ReadGoogleSheet.Models

    Public Class GoogleSheetRequest

        Public Property SpreadsheetId As String = String.Empty

        Public Property Range As String = String.Empty

        Public Property eventCat_id As Integer

    End Class

End Namespace
