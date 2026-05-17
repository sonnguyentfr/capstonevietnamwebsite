<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="SearchResult.ascx.vb" Inherits="NVCMS.Modules.School.IndexSearch" %>
<%@ Register TagPrefix="cap" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<style type="text/css">
    .ttm-timtruong_form .text-input .form-control:disabled,
    .ttm-timtruong_form .text-input .form-control[readonly] {
        background: #e1e1e1;
    }
</style>
<asp:UpdatePanel ID="updatepane" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-lg-12 m-auto">
                <div class="border-18px-solid white-border ttm-bgcolor-grey spacing-14">
                    <div class="ttm-timtruong_form wrap-form spacing-13 row">
                        <div class="col-lg-3 m-auto text-input">
                            <input name="name" type="text" class="form-control" id="txttentruong" runat="server" placeholder="Tên trường*">
                        </div>
                        <div class="col-lg-3 m-auto text-input">
                            <asp:DropDownList ID="ddlQuocGia" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-lg-3 m-auto text-input">
                            <asp:DropDownList ID="ddlLoaitruong" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-lg-3 m-auto text-input">
                            <asp:DropDownList ID="ddlMajor" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-lg-12 m-auto">
                            <asp:LinkButton ID="ltbTimtruong2" OnClientClick="return checkvalidate();" runat="server" CssClass="submit ttm-btn ttm-btn-size-md ttm-btn-shape-square ttm-btn-style-fill ttm-btn-color-dark mt-8" Text="Tìm thông tin trường"></asp:LinkButton>
                        </div>
                    </div>

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
<div class="row mt-100">
    <asp:Repeater ID="rptContent" runat="server">
        <ItemTemplate>
            <div class="ttm-box-col-wrapper col-lg-3 col-md-4 col-sm-6">
                <!-- featured-imagebox-course -->
                <div class="featured-imagebox featured-imagebox-course">
                    <div class="ttm-post-thumbnail featured-thumbnail">
                        <img class="img-fluid lazyload blur-up" data-src="<%#Ultis.FormatThumbImage(Eval("Logo"), 230, 150, "constrain", "middlecenter", "") %>" src="/data/no-photo.png?width=230&height=150&mode=constrain&anchor=middlecenter" alt="<%#Eval("NameofSchool") %>">
                    </div>
                    <div class="featured-content featured-content-post">
                        <div class="featured-content-post-inner">
                            <div class="post-desc featured-desc">
                                <small><%#Eval("StateCityName") %>, <%#Eval("CountryName") %></small>
                            </div>
                            <div class="post-title featured-title">
                                <h5><a href="courses-single.html"><%#Eval("NameofSchool") %></a></h5>
                            </div>
                            <div class="post-desc featured-desc">
                                <p><b>Lọai trường</b>:----</p>
                                <p><b>Hạn nộp hồ sơ</b>: Quanh năm</p>
                                <p><b>Chi phí</b>: $36926</p>
                            </div>
                        </div>
                        <div class="ttm-course-box-meta">
                            <div class="ttm-enrolled">
                                <span class="ttm-count ttm-meta-line"><i class="fa fa-user" aria-hidden="true"></i>78</span>
                                <span class="ttm-comments ttm-meta-line"><i class="fa fa-comment-o"></i>2</span>
                            </div>
                            <span class="ttm-lp-price"><ins>$69.00</ins></span>
                        </div>
                    </div>
                </div>
                <!-- featured-imagebox-course end-->
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div>
        <div class="list-page">
            <cap:PAGING ID="vbPaging" runat="server" />
        </div>
    </div>
</div>
<asp:HiddenField ID="hdftxttentruong" runat="server" />
<asp:HiddenField ID="hdfddlQuocGia" runat="server" />
<asp:HiddenField ID="hdfddlLoaitruong" runat="server" />
<asp:HiddenField ID="hdfmajor" runat="server" />
<script type="text/javascript">
        function checkvalidate() {
            document.getElementById('<%=hdftxttentruong.ClientID %>').value = document.getElementById('<%=txttentruong.ClientID%>').value;
            document.getElementById('<%=hdfddlQuocGia.ClientID %>').value = document.getElementById('<%=ddlQuocGia.ClientID%>').value;
            document.getElementById('<%=hdfddlLoaitruong.ClientID %>').value = document.getElementById('<%=ddlLoaitruong.ClientID%>').value;
            document.getElementById('<%=hdfmajor.ClientID %>').value = document.getElementById('<%=ddlMajor.ClientID%>').value;
            
        }
</script>
