<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="menuwebsite.ascx.vb" Inherits="_0._NVCMS.Modules.Hethong.menuwebsite" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Menu Website</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jstree/3.3.12/themes/default/style.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jstree/3.3.12/jstree.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#menuTree').jstree({
                'core': {
                    'data': {
                        'url': 'menuwebsite.ashx',
                        'dataType': 'json'
                    }
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="menuTree"></div>
    </form>
</body>
</html>
