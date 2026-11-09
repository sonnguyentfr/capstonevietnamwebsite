Imports System.Globalization
Imports System.IO
Imports System.Xml
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.HeThong.Menus

    ''' <summary>Lỗi nghiệp vụ khi lưu menu (dữ liệu sai hoặc file đã bị người khác sửa).</summary>
    Public Class MenuStoreException
        Inherits Exception

        Private ReadOnly _conflict As Boolean

        Public Sub New(message As String)
            MyBase.New(message)
            _conflict = False
        End Sub

        Public Sub New(message As String, conflict As Boolean)
            MyBase.New(message)
            _conflict = conflict
        End Sub

        ''' <summary>True = file đã đổi kể từ lúc tải (người khác vừa lưu).</summary>
        Public ReadOnly Property Conflict As Boolean
            Get
                Return _conflict
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Đọc/ghi menu quản trị của MỘT portal tại {Portal home}\MenuAdmin\menu.xml - đúng file skin _admin
    ''' (Portals\_default\Skins\_admin\controls\menu.ascx.vb) hiển thị cho portal đó.
    ''' Mỗi lần lưu tạo bản sao lưu trong MenuAdmin\backup (giữ <see cref="BackupKeep"/> bản gần nhất).
    ''' </summary>
    Public Class MenuStore

        Public Const BackupKeep As Integer = 30
        Public Const MaxDepth As Integer = 3
        Public Const MaxItems As Integer = 1000

        Private Shared ReadOnly SaveLock As New Object()
        Private Shared ReadOnly KnownAttributes As String() = {"Text", "Background", "NavUrl", "Value", "TabId", "Target", "Params"}

        Private ReadOnly _portalId As Integer
        Private ReadOnly _folder As String

        ''' <param name="portalId">PortalSettings.PortalId của portal đang quản trị.</param>
        ''' <param name="homeDirectoryMapPath">PortalSettings.HomeDirectoryMapPath của chính portal đó.</param>
        Public Sub New(portalId As Integer, homeDirectoryMapPath As String)
            If String.IsNullOrWhiteSpace(homeDirectoryMapPath) Then
                Throw New ArgumentException("Thiếu thư mục home của portal.", "homeDirectoryMapPath")
            End If
            _portalId = portalId
            _folder = Path.Combine(homeDirectoryMapPath, "MenuAdmin")
        End Sub

        Public ReadOnly Property PortalId As Integer
            Get
                Return _portalId
            End Get
        End Property

        Public ReadOnly Property FilePath As String
            Get
                Return Path.Combine(_folder, "menu.xml")
            End Get
        End Property

        Public ReadOnly Property BackupFolder As String
            Get
                Return Path.Combine(_folder, "backup")
            End Get
        End Property

        ' ------------------------------------------------------------------ đọc

        Public Function Load() As MenuDocument
            Dim doc As New MenuDocument()
            If Not File.Exists(FilePath) Then Return doc

            Dim xml As XmlDocument = LoadXml(FilePath)
            doc.Version = CurrentVersion()
            FillDocument(doc, xml)
            Return doc
        End Function

        Private Shared Function LoadXml(xmlFile As String) As XmlDocument
            Dim xml As New XmlDocument()
            xml.XmlResolver = Nothing
            xml.Load(xmlFile)
            Return xml
        End Function

        Private Shared Sub FillDocument(doc As MenuDocument, xml As XmlDocument)
            Dim root As XmlElement = xml.DocumentElement
            If root Is Nothing Then Return

            doc.RootName = root.Name
            Dim first As XmlElement = root.ChildNodes.OfType(Of XmlElement)().FirstOrDefault()
            If first IsNot Nothing Then doc.ItemName = first.Name
            doc.Items = ReadChildren(root)
        End Sub

        Private Shared Function ReadChildren(parent As XmlNode) As List(Of MenuNode)
            Dim list As New List(Of MenuNode)()
            For Each el As XmlElement In parent.ChildNodes.OfType(Of XmlElement)()
                Dim tabId As Integer
                If Not Integer.TryParse(Attr(el, "TabId"), NumberStyles.Integer, CultureInfo.InvariantCulture, tabId) Then
                    tabId = -1
                End If

                Dim node As New MenuNode()
                node.Text = Attr(el, "Text")
                node.Description = Attr(el, "Background")
                node.NavUrl = Attr(el, "NavUrl")
                node.Value = Attr(el, "Value")
                node.TabId = tabId
                node.Target = Attr(el, "Target")
                node.Params = Attr(el, "Params")
                node.Children = ReadChildren(el)
                For Each a As XmlAttribute In el.Attributes
                    If Not KnownAttributes.Contains(a.Name) Then node.Extra(a.Name) = a.Value
                Next
                list.Add(node)
            Next
            Return list
        End Function

        Private Shared Function Attr(el As XmlElement, name As String) As String
            If el.HasAttribute(name) Then Return el.GetAttribute(name)
            Return String.Empty
        End Function

        Private Function CurrentVersion() As String
            If File.Exists(FilePath) Then
                Return File.GetLastWriteTimeUtc(FilePath).Ticks.ToString(CultureInfo.InvariantCulture)
            End If
            Return String.Empty
        End Function

        ''' <summary>Thuộc tính của phần tử gốc (vd Skin, EnableDragAndDrop do RadTreeView ghi) - giữ nguyên khi lưu.</summary>
        Private Function ReadRootAttributes() As Dictionary(Of String, String)
            Dim result As New Dictionary(Of String, String)()
            If Not File.Exists(FilePath) Then Return result
            Try
                Dim xml As XmlDocument = LoadXml(FilePath)
                If xml.DocumentElement IsNot Nothing Then
                    For Each a As XmlAttribute In xml.DocumentElement.Attributes
                        result(a.Name) = a.Value
                    Next
                End If
            Catch ex As XmlException
                ' file đang hỏng: bỏ qua thuộc tính gốc, vẫn cho lưu đè để sửa lại file
            End Try
            Return result
        End Function

        ' ------------------------------------------------------------------ ghi

        ''' <summary>Kiểm tra, chuẩn hóa rồi ghi đè menu.xml. Trả về tài liệu sau khi lưu (có version mới).</summary>
        ''' <param name="expectedVersion">Version lúc client tải; khác version hiện tại thì ném <see cref="MenuStoreException"/> Conflict.</param>
        Public Function Save(input As MenuDocument, expectedVersion As String, force As Boolean) As MenuDocument
            If input Is Nothing Then Throw New MenuStoreException("Thiếu dữ liệu menu.")
            Dim items As List(Of MenuNode) = If(input.Items, New List(Of MenuNode)())
            Normalize(items)

            SyncLock SaveLock
                If Not force AndAlso If(expectedVersion, String.Empty) <> CurrentVersion() Then
                    Throw New MenuStoreException("Menu vừa được người khác cập nhật. Tải lại để xem bản mới, hoặc lưu đè.", True)
                End If

                Directory.CreateDirectory(_folder)
                Dim rootAttributes As Dictionary(Of String, String) = ReadRootAttributes()
                If File.Exists(FilePath) Then Backup()

                Dim rootName As String = SafeName(input.RootName, "Tree")
                Dim itemName As String = SafeName(input.ItemName, "Node")
                Dim xml As New XmlDocument()
                xml.AppendChild(xml.CreateXmlDeclaration("1.0", "utf-8", Nothing))
                Dim root As XmlElement = xml.CreateElement(rootName)
                For Each kv As KeyValuePair(Of String, String) In rootAttributes
                    root.SetAttribute(kv.Key, kv.Value)
                Next
                xml.AppendChild(root)
                WriteChildren(xml, root, items, itemName)

                ' ghi ra file tạm rồi mới thay để skin không bao giờ đọc phải file ghi dở
                Dim tmp As String = FilePath & ".tmp"
                Dim settings As New XmlWriterSettings()
                settings.Indent = True
                settings.IndentChars = "  "
                settings.Encoding = New UTF8Encoding(False)
                Using w As XmlWriter = XmlWriter.Create(tmp, settings)
                    xml.Save(w)
                End Using
                If File.Exists(FilePath) Then
                    File.Replace(tmp, FilePath, Nothing)
                Else
                    File.Move(tmp, FilePath)
                End If
            End SyncLock

            ClearSkinCache()
            Return Load()
        End Function

        Private Shared Sub WriteChildren(xml As XmlDocument, parent As XmlElement, nodes As IEnumerable(Of MenuNode), itemName As String)
            For Each n As MenuNode In nodes
                Dim el As XmlElement = xml.CreateElement(itemName)
                ' skin đọc .Attributes("X").Value không kiểm tra Nothing => luôn ghi đủ các thuộc tính
                el.SetAttribute("Text", n.Text)
                el.SetAttribute("Background", n.Description)
                el.SetAttribute("NavUrl", n.NavUrl)
                el.SetAttribute("Value", n.Value)
                el.SetAttribute("TabId", n.TabId.ToString(CultureInfo.InvariantCulture))
                el.SetAttribute("Target", n.Target)
                el.SetAttribute("Params", n.Params)
                For Each kv As KeyValuePair(Of String, String) In n.Extra
                    If Not KnownAttributes.Contains(kv.Key) AndAlso IsXmlName(kv.Key) Then
                        el.SetAttribute(kv.Key, If(kv.Value, String.Empty))
                    End If
                Next
                parent.AppendChild(el)
                WriteChildren(xml, el, n.Children, itemName)
            Next
        End Sub

        ' ------------------------------------------------------------------ chuẩn hóa / kiểm tra

        Private Class NormalizeState
            Public Count As Integer
            Public ReadOnly UsedRootValues As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        End Class

        ''' <summary>Cắt khoảng trắng, kiểm tra bắt buộc/độ sâu, tự sinh Value duy nhất cho mục cấp 1.</summary>
        Public Shared Sub Normalize(items As List(Of MenuNode))
            NormalizeLevel(items, 1, String.Empty, New NormalizeState())
        End Sub

        Private Shared Sub NormalizeLevel(nodes As List(Of MenuNode), depth As Integer, trail As String, state As NormalizeState)
            If nodes.Count = 0 Then Return
            If depth > MaxDepth Then
                Throw New MenuStoreException(String.Format("Menu chỉ cho phép tối đa {0} cấp ({1}).", MaxDepth, trail.TrimEnd("."c)))
            End If

            For i As Integer = 0 To nodes.Count - 1
                Dim n As MenuNode = nodes(i)
                If n Is Nothing Then Throw New MenuStoreException("Có mục menu rỗng.")

                state.Count += 1
                If state.Count > MaxItems Then
                    Throw New MenuStoreException(String.Format("Menu vượt quá {0} mục.", MaxItems))
                End If

                n.Text = If(n.Text, String.Empty).Trim()
                n.Description = If(n.Description, String.Empty).Trim()
                n.NavUrl = If(n.NavUrl, String.Empty).Trim()
                n.Value = If(n.Value, String.Empty).Trim()
                n.Target = If(n.Target, String.Empty).Trim()
                n.Params = If(n.Params, String.Empty).Trim()
                If n.Extra Is Nothing Then n.Extra = New Dictionary(Of String, String)()
                If n.Children Is Nothing Then n.Children = New List(Of MenuNode)()
                If n.TabId < -1 Then n.TabId = -1

                Dim label As String = trail & (i + 1).ToString(CultureInfo.InvariantCulture)
                If n.Text.Length = 0 Then
                    Throw New MenuStoreException(String.Format("Mục {0}: chưa nhập tên.", label))
                End If
                If n.Text.Length > 200 Then
                    Throw New MenuStoreException(String.Format("Mục ""{0}"": tên quá dài (tối đa 200 ký tự).", n.Text))
                End If
                If n.NavUrl.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase) Then
                    Throw New MenuStoreException(String.Format("Mục ""{0}"": link không hợp lệ.", n.Text))
                End If

                If depth = 1 Then
                    If n.Value.Length = 0 Then n.Value = NewValue(state.UsedRootValues)
                    If Not state.UsedRootValues.Add(n.Value) Then
                        Throw New MenuStoreException(String.Format("Mã ""{0}"" bị trùng giữa các mục cấp 1 (mục ""{1}"").", n.Value, n.Text))
                    End If
                End If

                NormalizeLevel(n.Children, depth + 1, label & ".", state)
            Next
        End Sub

        Private Shared Function NewValue(used As HashSet(Of String)) As String
            Dim i As Integer = 1
            While used.Contains("mn" & i.ToString(CultureInfo.InvariantCulture))
                i += 1
            End While
            Return "mn" & i.ToString(CultureInfo.InvariantCulture)
        End Function

        Private Shared Function IsXmlName(name As String) As Boolean
            If String.IsNullOrEmpty(name) Then Return False
            Try
                XmlConvert.VerifyName(name)
                Return True
            Catch ex As XmlException
                Return False
            End Try
        End Function

        Private Shared Function SafeName(name As String, fallback As String) As String
            If Not String.IsNullOrWhiteSpace(name) AndAlso IsXmlName(name) Then Return name
            Return fallback
        End Function

        ' ------------------------------------------------------------------ sao lưu

        Private Sub Backup()
            Directory.CreateDirectory(BackupFolder)
            Dim backupName As String = "menu_" & DateTime.Now.ToString("yyyyMMdd_HHmmss_fff", CultureInfo.InvariantCulture) & ".xml"
            File.Copy(FilePath, Path.Combine(BackupFolder, backupName), True)

            Dim olds As List(Of FileInfo) = New DirectoryInfo(BackupFolder).GetFiles("menu_*.xml") _
                .OrderByDescending(Function(f) f.Name).Skip(BackupKeep).ToList()
            For Each old As FileInfo In olds
                Try
                    old.Delete()
                Catch ex As IOException
                    ' bỏ qua: lần lưu sau sẽ dọn tiếp
                End Try
            Next
        End Sub

        Public Function Backups() As List(Of FileInfo)
            If Not Directory.Exists(BackupFolder) Then Return New List(Of FileInfo)()
            Return New DirectoryInfo(BackupFolder).GetFiles("menu_*.xml").OrderByDescending(Function(f) f.Name).ToList()
        End Function

        ''' <summary>Đọc một bản sao lưu (chỉ nhận tên file trong thư mục backup, không nhận đường dẫn).</summary>
        Public Function LoadBackup(fileName As String) As MenuDocument
            If String.IsNullOrEmpty(fileName) OrElse fileName <> Path.GetFileName(fileName) _
                OrElse Not fileName.StartsWith("menu_", StringComparison.Ordinal) _
                OrElse Not fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase) Then
                Throw New MenuStoreException("Tên bản sao lưu không hợp lệ.")
            End If
            Dim backupFile As String = Path.Combine(BackupFolder, fileName)
            If Not File.Exists(backupFile) Then Throw New MenuStoreException("Không tìm thấy bản sao lưu.")

            Dim doc As New MenuDocument()
            doc.Version = CurrentVersion()
            FillDocument(doc, LoadXml(backupFile))
            Return doc
        End Function

        ''' <summary>Skin cache theo khóa "Menu" + ngôn ngữ (hiện skin chưa SetCache nhưng xóa để chắc chắn).</summary>
        Private Shared Sub ClearSkinCache()
            DataCache.RemoveCache("Menuvi-VN")
            DataCache.RemoveCache("Menuen-US")
        End Sub

    End Class

End Namespace
