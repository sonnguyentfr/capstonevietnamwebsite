<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="CapV2_HomeTruongSearch.ascx.vb" Inherits="NVCMS.Modules.LibCRM.IndexSearch" %>

<style type="text/css">
    #listddlmajoir {
        display: none
    }

    .ttm-timtruong_form .text-input .form-control:disabled,
    .ttm-timtruong_form .text-input .form-control[readonly] {
        background: #e1e1e1;
    }

    .ttm-timtruong_form .text-input .form-control {
        border: solid 1px #c0c0c0;
    PADDING: 11px 14px;
    font-size: 12px;
    margin-bottom: 5px;
    }

    .ttm-timtruong_form .text-input {
        padding: 0px 7PX !important;
    }

    .ttm-timtruong_form .submit {
        border-radius: 15px;
        float: right;
        text-align: center;
        text-transform: uppercase;
    }
</style>
<asp:UpdatePanel ID="updatepane" runat="server">
    <ContentTemplate>

        <!--row-->
        <div class="col-lg-12 m-auto">
            <div class="row-title style1 text-center">
                <!-- section title -->
                <div class="section-title mb-5">
                    <div class="title-header ttm-textcolor-white">
                        <h2 class="title">CÔNG CỤ TÌM TRƯỜNG</h2>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-12 m-auto">
            <div class="ttm-timtruong_form wrap-form spacing-13 row">
                <div class="col-lg-4 m-auto text-input">
                    <input name="name" type="text" class="form-control" id="txttentruong" runat="server" placeholder="TÊN TRƯỜNG...">
                </div>
                <div class="col-lg-2 m-auto text-input">
                    <asp:DropDownList ID="ddlQuocGia" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-lg-2 m-auto text-input">
                    <asp:DropDownList ID="ddlLoaitruong" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLoaitruong_SelectIndexChange"></asp:DropDownList>
                </div>
                <div class="col-lg-2 m-auto text-input">
                    <asp:DropDownList ID="ddlMajor" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-lg-2 m-auto text-input">
                    <asp:DropDownList ID="ddlTuvanChiTra" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-lg-4 text-center m-auto">
                    <asp:LinkButton ID="ltbTimtruong" runat="server" CssClass="default-btn colo-red" Text="Tìm thông tin trường"></asp:LinkButton>
                </div>

            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="UpdateProgress2">
    <ProgressTemplate>
        <div class="loading" id="loadizng">
            <img src="/images/loading3.gif" alt="Loading" width="200px" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

