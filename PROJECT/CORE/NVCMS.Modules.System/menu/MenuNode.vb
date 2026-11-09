Imports Newtonsoft.Json

Namespace NVCMS.Modules.HeThong.Menus

    ''' <summary>
    ''' Một mục menu quản trị. Tên thuộc tính XML giữ nguyên như skin _admin (controls/menu.ascx.vb) đang đọc:
    ''' Text, Background, NavUrl, Value, TabId, Target, Params.
    ''' </summary>
    Public Class MenuNode

        ''' <summary>XML: Text - tên hiển thị.</summary>
        <JsonProperty("text")>
        Public Property Text As String = String.Empty

        ''' <summary>XML: Background - tooltip của mục cấp 1, đồng thời là tiêu đề khối menu con.</summary>
        <JsonProperty("description")>
        Public Property Description As String = String.Empty

        ''' <summary>XML: NavUrl - link của mục.</summary>
        <JsonProperty("navUrl")>
        Public Property NavUrl As String = String.Empty

        ''' <summary>XML: Value - mã mục; ở cấp 1 dùng làm data-content nối với khối menu con nên phải duy nhất.</summary>
        <JsonProperty("value")>
        Public Property Value As String = String.Empty

        ''' <summary>XML: TabId - trang DNN gắn với mục (quyết định quyền xem và trạng thái active). -1 = không gắn.</summary>
        <JsonProperty("tabId")>
        Public Property TabId As Integer = -1

        ''' <summary>XML: Target - vd _blank.</summary>
        <JsonProperty("target")>
        Public Property Target As String = String.Empty

        ''' <summary>XML: Params - tham số thêm vào link.</summary>
        <JsonProperty("params")>
        Public Property Params As String = String.Empty

        ''' <summary>
        ''' Các thuộc tính XML khác có sẵn trong file (ToolTip, MenuPermissions, UserPermissions, Title, Expanded...):
        ''' giữ nguyên khi lưu, không sửa trên giao diện.
        ''' </summary>
        <JsonProperty("extra")>
        Public Property Extra As Dictionary(Of String, String) = New Dictionary(Of String, String)()

        <JsonProperty("children")>
        Public Property Children As List(Of MenuNode) = New List(Of MenuNode)()

    End Class

    ''' <summary>Toàn bộ file menu.xml cùng phiên bản (dùng để phát hiện hai người sửa cùng lúc).</summary>
    Public Class MenuDocument

        <JsonProperty("items")>
        Public Property Items As List(Of MenuNode) = New List(Of MenuNode)()

        ''' <summary>Thời điểm sửa file (ticks UTC) lúc đọc; "" nếu file chưa tồn tại.</summary>
        <JsonProperty("version")>
        Public Property Version As String = String.Empty

        ''' <summary>Tên phần tử gốc. Menu website do RadTreeView sinh ra nên mặc định là Tree.</summary>
        <JsonProperty("rootName")>
        Public Property RootName As String = "Tree"

        ''' <summary>Tên phần tử mục. Mặc định là Node (skin đọc theo vị trí nên tên nào cũng được).</summary>
        <JsonProperty("itemName")>
        Public Property ItemName As String = "Node"

    End Class

End Namespace
