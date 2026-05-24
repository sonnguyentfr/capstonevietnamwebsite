Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.Banner


    Public Class BannerAdvWebController

        Private Function MapBanner(reader As IDataReader) As BannerAdvInfo

            Return New BannerAdvInfo With {
        .id = If(IsDBNull(reader("id")), 0, Convert.ToInt32(reader("id"))),
        .Title = If(IsDBNull(reader("Title")), "", reader("Title").ToString()),
        .KieuBanner = If(IsDBNull(reader("KieuBanner")), 0, Convert.ToInt32(reader("KieuBanner"))),
        .IMGLink = If(IsDBNull(reader("IMGLink")), "", reader("IMGLink").ToString()),
        .Vitri = If(IsDBNull(reader("Vitri")), 0, Convert.ToInt32(reader("Vitri"))),
        .Height = If(IsDBNull(reader("Height")), 0, Convert.ToInt32(reader("Height"))),
        .Width = If(IsDBNull(reader("Width")), 0, Convert.ToInt32(reader("Width"))),
        .Link = If(IsDBNull(reader("Link")), "", reader("Link").ToString()),
        .Visible = If(IsDBNull(reader("Visible")), False, Convert.ToBoolean(reader("Visible"))),
        .Ordernumber = If(IsDBNull(reader("Ordernumber")), 0, Convert.ToInt32(reader("Ordernumber"))),
        .Contact = If(IsDBNull(reader("Contact")), "", reader("Contact").ToString())
    }

        End Function


        Public Function GetByID(ByVal id As Integer) As BannerAdvInfo

            Using reader As IDataReader = DataProvider.Instance.NVCMS_Banner_GetByID(id)

                If reader.Read() Then
                    Return MapBanner(reader)
                End If

            End Using

            Return Nothing

        End Function


        Public Function GetAll(ByVal Portalid As Integer) As ArrayList

            Dim result As New ArrayList()

            Using reader As IDataReader = DataProvider.Instance.NVCMS_Banner_GetAll(Portalid)

                While reader.Read()
                    result.Add(MapBanner(reader))
                End While

            End Using

            Return result

        End Function


        Public Function GetAllVitri(ByVal Portalid As Integer, ByVal vitri As Integer) As ArrayList

            Dim result As New ArrayList()

            Using reader As IDataReader = DataProvider.Instance.NVCMS_Banner_GetAllVitri(Portalid, vitri)

                While reader.Read()
                    result.Add(MapBanner(reader))
                End While

            End Using

            Return result

        End Function


        Public Function GetAllShow(ByVal Portalid As Integer, ByVal vitri As Integer) As ArrayList

            Dim result As New ArrayList()

            Using reader As IDataReader = DataProvider.Instance.NVCMS_Banner_GetAllShow(Portalid, vitri)

                While reader.Read()
                    result.Add(MapBanner(reader))
                End While

            End Using

            Return result

        End Function

        '------------------------------------------'
    End Class
End Namespace