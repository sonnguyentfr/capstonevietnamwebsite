<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Viewer.ascx.vb" Inherits="NVCMS.Modules.Marketing.TemplateView" %>

<style type="text/css">
    .dx-datagrid .dx-data-row > td.bullet {
        padding-top: 0;
        padding-bottom: 0;
    }

    .dx-datagrid-table tr td {
        font-size: 12px !important;
        font-weight: 400;
        color: #353535;
        padding: 8px 10px !important;
    }

    table input {
        margin-top: 5px;
        height: 30px !important;
        border-radius: 0px !important;
    }
</style>

<div class="nk-block nk-block-lg">

    <div class="card card-preview">
        <div class="card-inner">
            <div class="nk-block-des">
                <asp:LinkButton ID="lbtAddTop" runat="server" Font-Bold="true" ValidationGroup="InputValidate" CssClass="btn btn-primary waves-effect waves-light mb-10">Thêm mới</asp:LinkButton>
            </div>
            <table id="datatablez" class="table table-stripped nk-tb-list nk-tb-ulist" data-auto-responsive="true">
                <thead>
                    <tr class="nk-tb-head">
                        <th class="nk-tb-col nk-tb-col-check">#
                        </th>
                        <th class="select-filter"><span class="sub-text">Tiêu đề</span></th>
                        <th class="select-filter"><span class="sub-text">Link file</span></th>
                        <th class="nk-tb-col nk-tb-col-tools text-right">#</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="drgViewData" runat="server">
                        <ItemTemplate>
                            <tr class="nk-tb-item">
                                <td class="nk-tb-col-check">

                                    <%# Eval("id") %>
                                </td>
                                <td>
                                    <asp:HyperLink ID="HyperLink1" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                         <%# Eval("TemplateName") %>
                                    </asp:HyperLink>
                                </td>
                                <td>
                                    <%# Eval("FilePath") %>
                                </td>

                                <td class="nk-tb-col nk-tb-col-tools">
                                    <ul class="nk-tb-actions gx-1">
                                        <li class="nk-tb-action-hidden">
                                            <asp:HyperLink ID="hplEdit" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                        <em class="icon ni ni-edit-fill"></em><span>Sửa</span>
                                            </asp:HyperLink>
                                        </li>

                                    </ul>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <!-- .nk-tb-item  -->
                </tbody>
            </table>
            <asp:LinkButton ID="lbtAdd" runat="server" Font-Bold="true" ValidationGroup="InputValidate" CssClass="btn btn-primary waves-effect waves-light">Thêm mới</asp:LinkButton>
        </div>
    </div>
    <!-- .card-preview -->



</div>
<script type="text/javascript">

    $(document).ready(function () {
        var table = $('#datatablez').DataTable({
            "pageLength": 50,
            "info": false,
            "dom": 'frtip',
            "searching": false,
            buttons: ['copy', 'excel', 'csv', 'pdf'],
        });

        table.columns('.select-filter').every(function () {
            var that = this;
            // Create the select list and search operation
            var select = $('<input class="form-control" />')
                .appendTo(
                    this.header()
                )
                .on('change', function () {
                    that
                        .search($(this).val());
                });
            this
                .cache('search')
                .unique()
                .each(function (d) {
                    select.append($('<option value="' + d + '">' + d + '</option>'));
                });
        });

    });
</script>
