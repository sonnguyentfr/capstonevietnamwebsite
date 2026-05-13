Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.School

    Public Class TruongMajorController

        Public Function TruongMajor_GetCountAllByTruong(TruongId) As Integer
            Return DataProvider.Instance.TruongMajor_GetCountAllByTruong(TruongId)
        End Function


        ''------------------------------------------'
        'Public Sub TruongMajor_Insert(ByVal TruongId As Integer, ByVal Major As Integer, ByVal Associate As Boolean, ByVal Bachelor As Boolean, ByVal Master As Boolean, ByVal Doctor As Boolean, ByVal ProfessionalCertificate As Boolean, ByVal Other As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
        '    DataProvider.Instance.TruongMajor_Insert(TruongId, Major, Associate, Bachelor, Master, Doctor, ProfessionalCertificate, Other, CreatedDate, UserId, PortalId)
        'End Sub

        ''------------------------------------------'
        'Public Sub TruongMajor_Update(ByVal TruongId As Integer, ByVal Major As Integer, ByVal Associate As Boolean, ByVal Bachelor As Boolean, ByVal Master As Boolean, ByVal Doctor As Boolean, ByVal ProfessionalCertificate As Boolean, ByVal Other As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
        '    DataProvider.Instance.TruongMajor_Update(TruongId, Major, Associate, Bachelor, Master, Doctor, ProfessionalCertificate, Other, CreatedDate, UserId, PortalId)
        'End Sub

        ''------------------------------------------'
        'Public Sub TruongMajor_Delete(ByVal id As Integer)
        '    DataProvider.Instance.TruongMajor_Delete(id)
        'End Sub

        ''------------------------------------------'
        'Public Function TruongMajor_GetByID(ByVal id As Integer) As TruongMajorInfo
        '    Return CType(CBO.FillObject(DataProvider.Instance.TruongMajor_GetByID(id), GetType(TruongMajorInfo)), TruongMajorInfo)
        'End Function
        ''------------------------------------------'
        'Public Function TruongMajor_GetByTruongIdMajorId(ByVal Truongid As Integer, Major As Integer) As TruongMajorInfo
        '    Return CType(CBO.FillObject(DataProvider.Instance.TruongMajor_GetByTruongIdMajorId(Truongid, Major), GetType(TruongMajorInfo)), TruongMajorInfo)
        'End Function
        ''------------------------------------------'
        'Public Function TruongMajor_GetAll() As ArrayList
        '    Return CBO.FillCollection(DataProvider.Instance.TruongMajor_GetAll(), GetType(TruongMajorInfo))
        'End Function
        ''------------------------------------------'
        'Public Function TruongMajor_GetAllByTruong(TruongId) As ArrayList
        '    Return CBO.FillCollection(DataProvider.Instance.TruongMajor_GetAllByTruong(TruongId), GetType(TruongMajorInfo))
        'End Function
        '------------------------------------------'
    End Class

End Namespace