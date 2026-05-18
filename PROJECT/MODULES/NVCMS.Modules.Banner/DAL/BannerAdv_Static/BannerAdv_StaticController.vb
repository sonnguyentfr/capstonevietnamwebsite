Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.Banner

    Public Class BannerAdv_StaticController
        Public Sub _Insert(ByVal BannerId As Integer, ByVal IP As String, ByVal Createdate As DateTime, ByVal isclick As Boolean)
            DataProvider.Instance.NVCMS_Banner_Static_Insert(BannerId, IP, Createdate, isclick)
        End Sub
        '------------------------------------------'
        Public Function _GetAllByBanner(BannerId As Integer, isclick As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NVCMS_Banner_Static_GetAllByBanner(BannerId, isclick), GetType(BannerAdv_StaticInfo))
        End Function

        '------------------------------------------'
        Public Function NVCMS_Banner_Static_SeletCount(ByVal datefrom As Date, ByVal dateto As Date, ByVal BannerId As Integer, ByVal Ip As String) As Integer
            Return DataProvider.Instance.NVCMS_Banner_Static_SeletCount(datefrom, dateto, BannerId, Ip)
        End Function
        '------------------------------------------'
        Public Function NVCMS_Banner_Static_SeletIndex(ByVal datefrom As Date, ByVal dateto As Date, ByVal BannerId As Integer, ByVal Ip As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NVCMS_Banner_Static_SeletIndex(datefrom, dateto, BannerId, Ip, PageIndex, PageSize), GetType(BannerAdv_StaticInfo))
        End Function
        '------------------------------------------'
        Public Function NVCMS_Banner_Static_SeletCountDate(ByVal createdate As Date, ByVal BannerId As Integer, ByVal Ip As String) As Integer
            Return DataProvider.Instance.NVCMS_Banner_Static_SeletCountDate(createdate, BannerId, Ip)
        End Function
        '------------------------------------------'
    End Class

End Namespace