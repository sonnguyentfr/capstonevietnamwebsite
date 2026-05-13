Namespace NVCMS.Modules.TinTuc
    Public Enum LoaiWF
        TatCa = 0
        DanhChoPhongVien
        DanhChoLanhDaoPhong
    End Enum
    Public Enum NewsStatus
        DangBienSoan = 0
        ChoPheDuyet
        DaXuatBan
        BiTraLai
        HuyXuatBan
        ChoXuatBan
        UyNhiemXB
    End Enum
    Public Enum KieuNhuanBut
        Tatca = 0
        TinBai = 1
        Videoclips = 2
    End Enum
    Public Enum CommentStatus
        Created = 1
        Pulished
        Deleted
    End Enum
    Public Enum TheLoaiTin
        Text = 1
        Image
        Audio
        Video
        TextImage
        TextAudio
        TextVideo
        TextImageAudio
        TextImageVideo
        TextAudioVideo
        TextImageAudioVideo
    End Enum
    Public Enum SourcesType
        FTP = 1
        UPLOAD
    End Enum
    Public Enum FilesType
        AUDIO = 1
        VIDEO
        IMAGE
    End Enum
End Namespace