Imports DotNetNuke.Entities.Portals
Namespace NVCMS.Modules.HeThong
    Public Class PortalContextHelper
        Public Shared ReadOnly Property CurrentPortal As CurrentPortalContextModel
            Get

                If HttpContext.Current.Session("CurrentPortal") IsNot Nothing Then
                    Return CType(
                                    HttpContext.Current.Session("CurrentPortal"),
                                    CurrentPortalContextModel
                                )

                End If
                'Nếu chưa có session thì lấy portal hiện tại
                Return New CurrentPortalContextModel With {
                .PortalId = PortalSettings.Current.PortalId,
                .PortalName = PortalSettings.Current.PortalName
            }

            End Get
        End Property

    End Class
End Namespace