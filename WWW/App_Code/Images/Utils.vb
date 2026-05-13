Imports Microsoft.VisualBasic
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Imports System
Imports System.Web
Imports System.Drawing
Imports System.Net
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports Microsoft.VisualBasic.CompilerServices
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports System.Web.UI.WebControls

Namespace NVCMS.Modules.TinTuc
    Public Class Utils

        Public Shared Function MakeImageThumbs(ByVal path As String, ByVal w As String, ByVal h As String, ByVal _woption As Integer, ByVal _hoption As Integer) As String
            If String.IsNullOrEmpty(path) Then
                Return String.Empty
            End If
            Dim sec As New Encryption64
            path = sec.Encrypt(path, "~!@#$%^&*()VOV")
            Dim params As String = "file=" & path.Trim
            If Not String.IsNullOrEmpty(w) Then
                params += "&w=" & w
            End If
            If Not String.IsNullOrEmpty(h) Then
                params += "&h=" & h
            End If

            If _woption <> 0 Then
                params += "&_w=" + _woption.ToString
            End If
            If _hoption <> 0 Then
                params += "&_h=" + _hoption.ToString
            End If
            Return "/DesktopModules/TinTuc/Control/MakeThumbnail.aspx?" & params
        End Function
    End Class

    Public Class Encryption64
        Private key() As Byte = {}
        Private IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

        Public Function Decrypt(ByVal stringToDecrypt As String,
            ByVal sEncryptionKey As String) As String
            Dim inputByteArray(stringToDecrypt.Length) As Byte
            Try
                key = System.Text.Encoding.UTF8.GetBytes(Left(sEncryptionKey, 8))
                Dim des As New DESCryptoServiceProvider()
                inputByteArray = Convert.FromBase64String(stringToDecrypt.Replace(" ", "+"))
                Dim ms As New MemoryStream()
                Dim cs As New CryptoStream(ms, des.CreateDecryptor(key, IV),
                    CryptoStreamMode.Write)
                cs.Write(inputByteArray, 0, inputByteArray.Length)
                cs.FlushFinalBlock()
                Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
                Return encoding.GetString(ms.ToArray())
            Catch e As Exception
                Return e.Message
            End Try
        End Function

        Public Function Encrypt(ByVal stringToEncrypt As String,
            ByVal SEncryptionKey As String) As String
            Try
                key = System.Text.Encoding.UTF8.GetBytes(Left(SEncryptionKey, 8))
                Dim des As New DESCryptoServiceProvider()
                Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(
                    stringToEncrypt)
                Dim ms As New MemoryStream()
                Dim cs As New CryptoStream(ms, des.CreateEncryptor(key, IV),
                    CryptoStreamMode.Write)
                cs.Write(inputByteArray, 0, inputByteArray.Length)
                cs.FlushFinalBlock()
                Return Convert.ToBase64String(ms.ToArray())
            Catch e As Exception
                Return e.Message
            End Try
        End Function

    End Class

    Public Class ImageUltilities

#Region "Fields"
        Private _align As HorizontalAlign
        Private _fillColor As String
        Private _height As Integer
        Private _imagePath As String
        Private _isFixSize As Boolean
        Private _notFoundImage As String
        Private _response As HttpResponse
        Private _server As HttpServerUtility
        Private _type As System.Drawing.Imaging.ImageFormat
        Private _vAlign As VerticalAlign
        Private _width As Integer
#End Region

#Region "Properties"
        Public Property Align() As HorizontalAlign
            Get
                Return Me._align
            End Get
            Set(ByVal value As HorizontalAlign)
                Me._align = value
            End Set
        End Property
        Public Property FillColor() As String
            Get
                Return Me._fillColor
            End Get
            Set(ByVal value As String)
                Me._fillColor = value
            End Set
        End Property
        Public Property Height() As Integer
            Get
                Return Me._height
            End Get
            Set(ByVal value As Integer)
                Me._height = value
            End Set
        End Property
        Public Property ImagePath() As String
            Get
                Return Me._imagePath
            End Get
            Set(ByVal value As String)
                Me._imagePath = value
            End Set
        End Property
        Public Property IsFixSize() As Boolean
            Get
                Return Me._isFixSize
            End Get
            Set(ByVal value As Boolean)
                Me._isFixSize = value
            End Set
        End Property
        Public Property NotFoundImage() As String
            Get
                Return Me._notFoundImage
            End Get
            Set(ByVal value As String)
                Me._notFoundImage = value
            End Set
        End Property
        Public Property Response() As HttpResponse
            Get
                Return Me._response
            End Get
            Set(ByVal value As HttpResponse)
                Me._response = value
            End Set
        End Property
        Public Property Server() As HttpServerUtility
            Get
                Return Me._server
            End Get
            Set(ByVal value As HttpServerUtility)
                Me._server = value
            End Set
        End Property
        Public Property Type() As System.Drawing.Imaging.ImageFormat
            Get
                Return Me._type
            End Get
            Set(ByVal value As System.Drawing.Imaging.ImageFormat)
                Me._type = value
            End Set
        End Property
        Public Property vAlign() As VerticalAlign
            Get
                Return Me._vAlign
            End Get
            Set(ByVal value As VerticalAlign)
                Me._vAlign = value
            End Set
        End Property
        Public Property Width() As Integer
            Get
                Return Me._width
            End Get
            Set(ByVal value As Integer)
                Me._width = value
            End Set
        End Property
#End Region

#Region "Methods"
        Public Sub New()
            Me._server = Nothing
            Me._response = Nothing
            Me._type = System.Drawing.Imaging.ImageFormat.Jpeg
            Me._fillColor = "white"
            Me._vAlign = VerticalAlign.Middle
            Me._align = HorizontalAlign.Center
            Me._isFixSize = True
            Me._notFoundImage = "~/images/thumbnail.jpg"
        End Sub
        Public Sub New(ByVal server As HttpServerUtility, ByVal response As HttpResponse, ByVal imagePath As String, ByVal type As System.Drawing.Imaging.ImageFormat, ByVal fillColor As String, ByVal width As Integer, ByVal height As Integer, ByVal vAlign As VerticalAlign, ByVal align As HorizontalAlign, ByVal isFixSize As Boolean, ByVal notFoundImage As String)
            Me._server = Nothing
            Me._response = Nothing
            Me._type = System.Drawing.Imaging.ImageFormat.Jpeg
            Me._fillColor = "white"
            Me._vAlign = VerticalAlign.Middle
            Me._align = HorizontalAlign.Center
            Me._isFixSize = True
            Me._notFoundImage = "~/images/thumbnail.jpg"
            Me.Server = server
            Me.Response = response
            Me.ImagePath = imagePath
            Me.Type = type
            Me.FillColor = fillColor
            Me.Width = width
            Me.Height = height
            Me.vAlign = vAlign
            Me.Align = align
            Me.IsFixSize = isFixSize
            Me.NotFoundImage = notFoundImage
        End Sub
        Public Function ThumbSizeWidth(ByVal currentWidth As Integer, ByVal currentHeight As Integer, ByVal newWidth As Integer) As Size
            Dim num As Double
            If (currentWidth > newWidth) Then
                num = (Convert.ToDouble(newWidth) / CDbl(currentWidth))
            Else
                num = 1
            End If
            Return New Size(Convert.ToInt16(CDbl((currentWidth * num))), Convert.ToInt16(CDbl((currentHeight * num))))
        End Function
        Public Function ThumbSizeHeight(ByVal currentWidth As Integer, ByVal currentHeight As Integer, ByVal newHeight As Integer) As Size
            Dim num As Double
            If (currentHeight > newHeight) Then
                num = (Convert.ToDouble(newHeight) / CDbl(currentHeight))
            Else
                num = 1
            End If
            Return New Size(Convert.ToInt16(CDbl((currentWidth * num))), Convert.ToInt16(CDbl((currentHeight * num))))
        End Function
        Private Function getColor(ByVal strBrushName As String) As Color
            Dim white As Color = Color.White
            Select Case strBrushName.ToLower
                Case "black"
                    Return Color.Black
                Case "yellow"
                    Return Color.Yellow
                Case "silver"
                    Return Color.Silver
                Case "aqua"
                    Return Color.Aqua
                Case "blue"
                    Return Color.Blue
                Case "brown"
                    Return Color.Brown
                Case "cyan"
                    Return Color.Cyan
                Case "orange"
                    Return Color.Orange
                Case "red"
                    Return Color.Red
                Case "skyblue"
                    Return Color.SkyBlue
                Case "gold"
                    Return Color.Gold
                Case "gray"
                    Return Color.Gray
                Case "green"
                    Return Color.Green
                Case "magenta"
                    Return Color.Magenta
                Case "transparent"
                    white = Color.Transparent
                    Exit Select
            End Select
            Return white
        End Function
        Public Function GetImage(ByVal sURL As String) As System.Drawing.Image
            If (sURL.IndexOf("http://") > -1) Then
                Dim image As System.Drawing.Image
                Try
                    Dim request As HttpWebRequest = DirectCast(WebRequest.Create(sURL), HttpWebRequest)
                    request.Credentials = CredentialCache.DefaultCredentials
                    Dim response As HttpWebResponse = DirectCast(request.GetResponse, HttpWebResponse)
                    Return Drawing.Image.FromStream(response.GetResponseStream)
                Catch exception1 As Exception
                    ProjectData.SetProjectError(exception1)
                    Dim exception As Exception = exception1
                    image = Drawing.Image.FromFile(Me.Server.MapPath("~/images/thumbnail.jpg"))
                    ProjectData.ClearProjectError()
                    Return image
                    ProjectData.ClearProjectError()
                End Try
                Return image
            End If
            Return System.Drawing.Image.FromFile(sURL)
        End Function
        Public Sub OutputStreamImage()
            Dim graphics As Graphics = Nothing
            Dim image As System.Drawing.Image = Nothing
            Dim imgTemp As System.Drawing.Image = Nothing
            Dim bitmap As Bitmap = Nothing
            Try
                Dim size As Size
                If Not Me.ImagePath.StartsWith(DotNetNuke.Common.Globals.ApplicationPath) Then
                    Me.ImagePath = (DotNetNuke.Common.Globals.ApplicationPath & Me.ImagePath)
                End If
                If (Not Null.IsNull(Me.ImagePath) And File.Exists(Me.Server.MapPath(Me.ImagePath))) Then
                    imgTemp = Me.GetImage(Me.ImagePath)
                Else
                    imgTemp = Me.GetImage(Me.Server.MapPath(Me.NotFoundImage))
                End If
                image = imgTemp.Clone
                If (Not Null.IsNull(Me.Width) And Not Null.IsNull(Me.Height)) Then
                    size = Me.ThumbSizeWidth(image.Width, image.Height, Me.Width)
                    size = Me.ThumbSizeHeight(size.Width, size.Height, Me.Height)
                ElseIf Not Null.IsNull(Me.Width) Then
                    size = Me.ThumbSizeWidth(image.Width, image.Height, Me.Width)
                    If (Convert.ToInt16(Me.Width) > image.Width) Then
                        Me.Width = image.Width
                    End If
                    Me.Height = size.Height
                ElseIf Not Null.IsNull(Me.Height) Then
                    size = Me.ThumbSizeHeight(image.Width, image.Height, Me.Height)
                    If (Me.Height > image.Height) Then
                        Me.Height = image.Height
                    End If
                    Me.Width = size.Width
                Else
                    Me.Width = image.Width
                    Me.Height = image.Height
                    size = New Size(image.Width, image.Height)
                End If
                If Not Me.IsFixSize Then
                    If (Me.Width > size.Width) Then
                        Me.Width = size.Width
                    End If
                    If (Me.Height > size.Height) Then
                        Me.Height = size.Height
                    End If
                End If
                bitmap = New Bitmap(Integer.Parse(Conversions.ToString(Me.Width)), Integer.Parse(Conversions.ToString(Me.Height)))
                bitmap.MakeTransparent()
                graphics = Drawing.Graphics.FromImage(bitmap)
                graphics.SmoothingMode = SmoothingMode.HighQuality
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
                graphics.DrawImage(image, 0, 0, size.Width, size.Height)
                If Not Null.IsNull(Me.FillColor) Then
                    graphics.Clear(Me.getColor(Me.FillColor))
                Else
                    graphics.Clear(bitmap.GetPixel(0, 0))
                End If
                Dim x As Integer = 0
                If Not Null.IsNull(Me.Align) Then
                    If (Me.Align = HorizontalAlign.Center) Then
                        x = CInt(Math.Round(CDbl((CDbl((Me.Width - size.Width)) / 2))))
                    ElseIf (Me.Align = HorizontalAlign.Right) Then
                        x = (Me.Width - size.Width)
                    End If
                End If
                Dim y As Integer = 0
                If Not Null.IsNull(Me.vAlign) Then
                    If (Me.vAlign = VerticalAlign.Middle) Then
                        y = CInt(Math.Round(CDbl((CDbl((Me.Height - size.Height)) / 2))))
                    ElseIf (Me.vAlign = VerticalAlign.Bottom) Then
                        y = (Me.Height - size.Height)
                    End If
                End If
                graphics.DrawImage(image, x, y, size.Width, size.Height)
                graphics.Dispose()
                Dim rawFormat As ImageFormat = image.RawFormat
                If Null.IsNull(Me.Type) Then
                    If rawFormat.Equals(ImageFormat.Bmp) Then
                        Me.Response.ContentType = "image/bmp"
                        rawFormat = ImageFormat.Bmp
                    ElseIf rawFormat.Equals(ImageFormat.Emf) Then
                        Me.Response.ContentType = "image/emf"
                        rawFormat = ImageFormat.Emf
                    ElseIf rawFormat.Equals(ImageFormat.Exif) Then
                        Me.Response.ContentType = "image/exif"
                        rawFormat = ImageFormat.Exif
                    ElseIf rawFormat.Equals(ImageFormat.Gif) Then
                        Me.Response.ContentType = "image/gif"
                        rawFormat = ImageFormat.Gif
                    ElseIf rawFormat.Equals(ImageFormat.Png) Then
                        Me.Response.ContentType = "image/png"
                        rawFormat = ImageFormat.Png
                    ElseIf rawFormat.Equals(ImageFormat.Tiff) Then
                        Me.Response.ContentType = "image/tiff"
                        rawFormat = ImageFormat.Tiff
                    ElseIf rawFormat.Equals(ImageFormat.Wmf) Then
                        Me.Response.ContentType = "image/wmf"
                        rawFormat = ImageFormat.Wmf
                    Else
                        Me.Response.ContentType = "image/jpeg"
                        rawFormat = ImageFormat.Jpeg
                    End If
                Else
                    If Me.Type.Equals(ImageFormat.Bmp) Then
                        Me.Response.ContentType = "image/bmp"
                    ElseIf Me.Type.Equals(ImageFormat.Emf) Then
                        Me.Response.ContentType = "image/emf"
                    ElseIf Me.Type.Equals(ImageFormat.Exif) Then
                        Me.Response.ContentType = "image/exif"
                    ElseIf Me.Type.Equals(ImageFormat.Gif) Then
                        Me.Response.ContentType = "image/gif"
                    ElseIf Me.Type.Equals(ImageFormat.Png) Then
                        Me.Response.ContentType = "image/png"
                    ElseIf Me.Type.Equals(ImageFormat.Tiff) Then
                        Me.Response.ContentType = "image/tiff"
                    ElseIf Me.Type.Equals(ImageFormat.Wmf) Then
                        Me.Response.ContentType = "image/wmf"
                    Else
                        Me.Response.ContentType = "image/jpeg"
                    End If
                    rawFormat = Me.Type
                End If
                Try
                    bitmap.Save(Me.Response.OutputStream, rawFormat)
                Catch exception1 As Exception
                    ProjectData.SetProjectError(exception1)
                    Dim exception As Exception = exception1
                    bitmap.Save(Me.Response.OutputStream, ImageFormat.Jpeg)
                    ProjectData.ClearProjectError()
                End Try
            Catch exception3 As Exception
                ProjectData.SetProjectError(exception3)
                Dim exception2 As Exception = exception3
                ProjectData.ClearProjectError()
            End Try
            image.Dispose()
            bitmap.Dispose()
        End Sub
        Public Function FillImage() As System.Drawing.Image
            Dim graphics As Graphics = Nothing
            Dim image As System.Drawing.Image = Nothing
            Dim imgTemp As System.Drawing.Image = Nothing
            Dim bitmap As Bitmap = Nothing
            Dim stream As New IO.MemoryStream
            Try
                Dim size As Size
                If Not Me.ImagePath.StartsWith(DotNetNuke.Common.Globals.ApplicationPath) Then
                    Me.ImagePath = (DotNetNuke.Common.Globals.ApplicationPath & Me.ImagePath)
                End If
                If (Not Null.IsNull(Me.ImagePath) And File.Exists(Me.ImagePath)) Then
                    imgTemp = Me.GetImage(Me.ImagePath)
                Else
                    imgTemp = Me.GetImage(Me.Server.MapPath(Me.NotFoundImage))
                End If
                image = imgTemp.Clone
                If (Not Null.IsNull(Me.Width) And Not Null.IsNull(Me.Height)) Then
                    size = Me.ThumbSizeWidth(image.Width, image.Height, Me.Width)
                    size = Me.ThumbSizeHeight(size.Width, size.Height, Me.Height)
                ElseIf Not Null.IsNull(Me.Width) Then
                    size = Me.ThumbSizeWidth(image.Width, image.Height, Me.Width)
                    If (Convert.ToInt16(Me.Width) > image.Width) Then
                        Me.Width = image.Width
                    End If
                    Me.Height = size.Height
                ElseIf Not Null.IsNull(Me.Height) Then
                    size = Me.ThumbSizeHeight(image.Width, image.Height, Me.Height)
                    If (Me.Height > image.Height) Then
                        Me.Height = image.Height
                    End If
                    Me.Width = size.Width
                Else
                    Me.Width = image.Width
                    Me.Height = image.Height
                    size = New Size(image.Width, image.Height)
                End If
                If Not Me.IsFixSize Then
                    If (Me.Width > size.Width) Then
                        Me.Width = size.Width
                    End If
                    If (Me.Height > size.Height) Then
                        Me.Height = size.Height
                    End If
                End If
                bitmap = New Bitmap(Integer.Parse(Conversions.ToString(Me.Width)), Integer.Parse(Conversions.ToString(Me.Height)))
                bitmap.MakeTransparent()
                graphics = Drawing.Graphics.FromImage(bitmap)
                graphics.SmoothingMode = SmoothingMode.HighQuality
                graphics.CompositingQuality = CompositingQuality.HighQuality
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
                graphics.DrawImage(image, 0, 0, size.Width, size.Height)
                If Not Null.IsNull(Me.FillColor) Then
                    graphics.Clear(Me.getColor(Me.FillColor))
                Else
                    graphics.Clear(bitmap.GetPixel(0, 0))
                End If
                Dim x As Integer = 0
                If Not Null.IsNull(Me.Align) Then
                    If (Me.Align = HorizontalAlign.Center) Then
                        x = CInt(Math.Round(CDbl((CDbl((Me.Width - size.Width)) / 2))))
                    ElseIf (Me.Align = HorizontalAlign.Right) Then
                        x = (Me.Width - size.Width)
                    End If
                End If
                Dim y As Integer = 0
                If Not Null.IsNull(Me.vAlign) Then
                    If (Me.vAlign = VerticalAlign.Middle) Then
                        y = CInt(Math.Round(CDbl((CDbl((Me.Height - size.Height)) / 2))))
                    ElseIf (Me.vAlign = VerticalAlign.Bottom) Then
                        y = (Me.Height - size.Height)
                    End If
                End If
                graphics.DrawImage(image, x, y, size.Width, size.Height)
                graphics.Dispose()
                Dim rawFormat As ImageFormat = image.RawFormat
                If Null.IsNull(Me.Type) Then
                    If rawFormat.Equals(ImageFormat.Bmp) Then
                        rawFormat = ImageFormat.Bmp
                    ElseIf rawFormat.Equals(ImageFormat.Emf) Then
                        rawFormat = ImageFormat.Emf
                    ElseIf rawFormat.Equals(ImageFormat.Exif) Then
                        rawFormat = ImageFormat.Exif
                    ElseIf rawFormat.Equals(ImageFormat.Gif) Then
                        rawFormat = ImageFormat.Gif
                    ElseIf rawFormat.Equals(ImageFormat.Png) Then
                        rawFormat = ImageFormat.Png
                    ElseIf rawFormat.Equals(ImageFormat.Tiff) Then
                        rawFormat = ImageFormat.Tiff
                    ElseIf rawFormat.Equals(ImageFormat.Jpeg) Then
                        rawFormat = ImageFormat.Wmf
                    Else
                        rawFormat = ImageFormat.Jpeg
                    End If
                Else
                    rawFormat = Me.Type
                End If
                Try
                    bitmap.Save(stream, rawFormat)
                Catch exception1 As Exception
                    ProjectData.SetProjectError(exception1)
                    Dim exception As Exception = exception1
                    bitmap.Save(stream, ImageFormat.Jpeg)
                    ProjectData.ClearProjectError()
                End Try
            Catch exception3 As Exception
                ProjectData.SetProjectError(exception3)
                Dim exception2 As Exception = exception3
                ProjectData.ClearProjectError()
            End Try
            image.Dispose()
            bitmap.Dispose()
            Return Drawing.Image.FromStream(stream)
        End Function
#End Region

#Region "Other Methods"
        ''' <summary>
        ''' Tao thumbnail cho file ảnh
        ''' </summary>
        ''' <param name="sFolder"></param>
        ''' <param name="sFileImage"></param>
        ''' <param name="width"></param>
        ''' <param name="height"></param>
        ''' <remarks></remarks>
        <Obsolete("This method is obsolete.  It has been replaced by GenerateThumbnail(ByVal sFolder As String, ByVal ContainerFolder As String, ByVal sFileImage As String, ByVal widthThumb As Short, ByVal heightThumb As Short) ")>
        Public Shared Sub GenerateThumbnail(ByVal sFolder As String, ByVal sFileImage As String, ByVal width As Short, ByVal height As Short)
            'Upload thumbnails images
            Dim sFileName As String = sFileImage
            'Khai bao cac doi tuong
            Dim objImage, objThumbnail As System.Drawing.Image
            Dim strServerPath As String
            Dim shtWidth, shtHeight As Short

            If Not (Directory.Exists(sFolder & "Thumbnails/")) Then
                Directory.CreateDirectory(sFolder & "Thumbnails/")
            End If
            strServerPath = sFolder & "Thumbnails/"
            objImage = Drawing.Image.FromFile(sFolder & sFileName)
            shtWidth = width
            'Tinh chieu cao t/ung voi chieu rong
            shtHeight = objImage.Height / (objImage.Width / shtWidth)
            'Tao thumbnail
            objThumbnail = objImage.GetThumbnailImage(shtWidth, shtHeight, Nothing, System.IntPtr.Zero)
            objThumbnail.Save(strServerPath + sFileName, System.Drawing.Imaging.ImageFormat.Jpeg)
            'Tidy up
            objImage.Dispose()
            objThumbnail.Dispose()
        End Sub

        ''' <summary>
        ''' Hàm sinh thumbnail
        ''' </summary>
        ''' <param name="sFolder">Thư mục chứa ảnh</param>
        ''' <param name="ContainerFolder">Nếu ContainerFolder="" --> Generate thumbnail ngay tại thư mục chứa ảnh gốc và xóa ảnh gốc đi :D</param>
        ''' <param name="sFileImage">Tên file</param>
        ''' <param name="widthThumb">chiều rộng thumbnail</param>
        ''' <param name="heightThumb">chiều cao thumbnail</param>
        ''' <remarks></remarks>
        Public Shared Sub GenerateThumbnail(ByVal sFolder As String, ByVal ContainerFolder As String, ByVal sFileImage As String, ByVal widthThumb As Short, ByVal heightThumb As Short, ByVal server As System.Web.HttpServerUtility, ByVal response As System.Web.HttpResponse)
            Dim objResultImg As System.Drawing.Image = Nothing
            Try
                'Upload thumbnails images
                Dim sFileName As String = sFileImage
                'Khai bao cac doi tuong
                Dim strServerPath As String
                If ContainerFolder.Equals("") = False Then
                    If Not (Directory.Exists(sFolder & ContainerFolder)) Then
                        Directory.CreateDirectory(sFolder & ContainerFolder)
                    End If
                    strServerPath = sFolder & ContainerFolder
                Else
                    strServerPath = sFolder
                End If

                'Tao thumbnail
                Dim icf As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders() 'get available codecs
                Dim encps As EncoderParameters = New EncoderParameters(1)
                Dim encp As EncoderParameter = New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 75) 'Set quality to 75
                encps.Param(0) = encp

                'Nếu có file trùng tên --> XÓA
                If File.Exists(strServerPath.Replace("/", "\") + sFileName) Then
                    FileSystemUtils.DeleteFile(strServerPath.Replace("/", "\") + sFileName)
                End If

                'Lưu file MỚI
                Dim objImg As New ImageUltilities(server, response, sFolder & sFileName, System.Drawing.Imaging.ImageFormat.Jpeg, "", widthThumb, heightThumb, System.Web.UI.WebControls.VerticalAlign.NotSet, System.Web.UI.WebControls.HorizontalAlign.NotSet, False, "")
                objResultImg = objImg.FillImage()


                objResultImg.Save(strServerPath.Replace("/", "\") + sFileName, icf(1), encps) ' icf[1] = JPEG Codec
            Catch ex As Exception
                ProjectData.ClearProjectError()
            End Try

            'Tidy up
            objResultImg.Dispose()
        End Sub

        Public Shared Function getDimesion(ByVal width As Integer, ByVal height As Integer, ByVal cropBox As Integer) As Integer()
            'the first detect width
            'detect dimension of image
            Dim width_new As Integer = cropBox
            Dim height_new As Integer = 0
            If width_new <= width Then
                If width <> 0 Then
                    height_new = CInt((height * cropBox / width))
                End If
            Else
                height_new = height
                width_new = width
            End If

            Dim width1 As Integer = width_new
            Dim height1 As Integer = height_new

            'the second will detect height of image
            Dim width2 As Integer = 0
            Dim height2 As Integer = height1
            If height2 <= cropBox Then
                width2 = width1
            Else
                height2 = cropBox
                'height2 = (int)(height1 * cropBox / width1);
                width2 = CInt((width1 * cropBox / height1))
            End If
            Return New Integer() {width2, height2}
        End Function

        Public Shared Function getDimesionWH(ByVal width As Integer, ByVal height As Integer, ByVal cropBox As Integer, ByVal cropBoxH As Integer) As Integer()
            'the first detect width
            'detect dimension of image
            Dim width_new As Integer = cropBox
            Dim height_new As Integer = 0
            If width_new <= width Then
                If width <> 0 Then
                    height_new = CInt((height * cropBox / width))
                End If
            Else
                height_new = height
                width_new = width
            End If

            Dim width1 As Integer = width_new
            Dim height1 As Integer = height_new

            'the second will detect height of image
            Dim width2 As Integer = 0
            Dim height2 As Integer = height1
            If height2 <= cropBoxH Then
                width2 = width1
            Else
                height2 = cropBoxH
                'height2 = (int)(height1 * cropBox / width1);
                width2 = CInt((width1 * cropBoxH / height1))
            End If
            Return New Integer() {width2, height2}
        End Function
#End Region

    End Class
End Namespace