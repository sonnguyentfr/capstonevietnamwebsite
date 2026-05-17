<%@ Control Language="vb" EnableViewState="false" AutoEventWireup="false" Explicit="true" CodeFile="Index.ascx.vb" Inherits="NVCMS.Modules.School.IndexSearch" %>
<%@ Register TagPrefix="cap" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<style type="text/css">
    .icontitle {
        position: absolute;
        top: 0px;
        right: 0px;
        background: #f7d636db;
        border-radius: 0px 0px 0px 7px;
        padding: 2px 6px;
        color: #fff;
        font-size: 11px;
    }

    .list-school {
    }

        .list-school .postlist {
            padding: 30px 15px;
            background-color: #fff;
            -webkit-box-shadow: 0 0 1.25rem rgb(108 118 134 / 10%);
            box-shadow: 0 0 1.25rem rgb(108 118 134 / 10%);
            font-size: 13px;
            border: 1px #e5e5e5 solid;
            margin-bottom: 20px;
            position: relative;
            min-height: 390px;
        }

            .list-school .postlist:hover {
                box-shadow: 0 0 1.25rem rgb(108 118 134 / 35%);
                border: 1px #dbdbdb solid;
            }

            .list-school .postlist .post-title {
            }

                .list-school .postlist .post-title h2 {
                    font-size: 20px;
                    font-weight: 500;
                    letter-spacing: -0.8px;
                    padding: 20px 0px 10px 0px;
                }

            .list-school .postlist .post-entry p {
                text-overflow: ellipsis;
                position: relative;
                white-space: nowrap;
                overflow: hidden;
                margin-bottom: 5px;
            }

        .list-school .form-group {
            position: relative;
            margin-bottom: 15px;
        }

            .list-school .form-group label {
                z-index: 1;
                display: block;
                margin-bottom: 0;
                position: absolute;
                left: 15px;
                color: #107cbe;
                font-size: 22px;
                top: 50%;
                -webkit-transform: translateY(-50%);
                transform: translateY(-50%);
            }

            .list-school .form-group .form-control {
                padding: 5px 10px 5px 45px;
                color: #202647;
                background-color: #fbfeff;
                border: 1px solid #72d6ff;
                font-size: 13px;
                font-weight: 400;
                height: 35px;
                -webkit-transition: .5s;
                transition: .5s;
                border-radius: 5px;
            }
</style>
<div class="list-school row">
    <div class="col-lg-4 col-sm-4">
        <div class="form-group">
            <asp:TextBox ID="txttentruong" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="col-lg-4 col-sm-6">
        <div class="form-group">
            <label><i class="fa fa-phone" aria-hidden="true"></i></label>
            <asp:DropDownList ID="ddlQuocgia" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
    </div>
    <div class="col-lg-4 col-sm-6">
        <div class="form-group">
            <label><i class="fa fa-phone" aria-hidden="true"></i></label>
            <asp:DropDownList ID="ddlTuvanChiTra" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
    </div>
</div>
<div class="list-school row">

    <div class="col-lg-4 col-sm-6">
        <div class="form-group">
            <label><i class="fa fa-phone" aria-hidden="true"></i></label>
            <asp:DropDownList ID="ddlLoaitruong2" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
    </div>
    <div class="col-lg-4 col-md-6">
        <div class="form-group">
            <label><i class='flaticon-doctor-2'></i></label>
            <asp:DropDownList ID="ddlMajor" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
    </div>
    <div class="col-lg-4 col-sm-6">
        <div class="form-group">
            <asp:LinkButton ID="lbtFind" CssClass="default-btn" runat="server"><i class="bx bx-search-alt"></i> Tìm </asp:LinkButton>
        </div>
    </div>
</div>
<div class=" list-school row">
    <asp:Repeater ID="rptContent" runat="server">
        <ItemTemplate>
            <div class="col-lg-3">
                <div class="postlist">
                    <div class="news-image">
                        <a href='<%# Ultis.FormatLink_School(PortalSettings.ActiveTab.TabID, CType(DataBinder.Eval(Container.DataItem, "id"), Integer), CType(DataBinder.Eval(Container.DataItem, "NameofSchool"), String)) %>'>
                            <img class="responsive-img" src="<%#Ultis.FormatThumbImage(Eval("Logo"), 310, 209, "constrain", "middlecenter", "") %>" alt="news Image" onerror="LoadImage(this, '/data/noimage.png?width=310&height=209&mode=constrain&anchor=middlecenter')" />
                        </a>
                        <span class="icontitle">
                            <i class="fa fa-clock-o"></i><%#BL.FormatDate(Eval("CreatedDate"))%>
                            <asp:Label Font-Bold="true" ForeColor="#0093ff" ID="lblPartner" runat="server" Text="<i class='fa fa-check-square' aria-hidden='true'></i>"></asp:Label>
                        </span>
                    </div>
                    <div class="post-title">
                        <h2>
                            <a href='<%# Ultis.FormatLink_School(PortalSettings.ActiveTab.TabID, CType(DataBinder.Eval(Container.DataItem, "id"), Integer), CType(DataBinder.Eval(Container.DataItem, "NameofSchool"), String)) %>'>
                                <%#Eval("NameofSchool") %></a>
                        </h2>
                    </div>
                    <div class="post-entry">
                        <p>Năm thành lập: <%#Eval("Namthanhlap") %></p>
                        <p>Website: <%#Eval("Website") %></p>
                        <p><%# Ultis.SubString(Eval("Tomtat"), 20, "..") %></p>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="cl"></div>
</div>

<div class="list-page">
    <cap:PAGING ID="vbPaging" runat="server" />
</div>
<script type="text/javascript">

    $(document).ready(
        function LoadImage(n, t) {
            var i = new Image; i.src = t;
            i.onload = function () {
                n.src = t; n.onerror = null
            };
            i.onerror = function () {
                n.src = "/data/noimage.png"
            }
        }
    );
</script>
