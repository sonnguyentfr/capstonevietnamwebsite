<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PageSettings.ascx.vb" Inherits="PageSettings" %>
<%--<asp:UpdatePanel runat="server" ID="uptpanefils">
    <ContentTemplate>--%>
<div class="row">
    <div class="col-md-7 col-sm-12 col-xs-12">
        <div class="card card-bordered">
            <div class="card-inner">
                <div class="card-head">
                    <h5 class="card-title">THÔNG TIN WEBSITE</h5>
                </div>
                <div class="gy-3">
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label" for="site-name">Tên website</label>
                                <span class="form-note">Thông tin đầy đủ về website</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitename" runat="server" placeholder="Đại học Ngân hàng Thành Phố Hồ Chí Minh">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Website</label>
                                <span class="form-note">Thông tin địa chỉ domain website</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="siteweb" runat="server" placeholder="buh.edu.vn">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Địa chỉ chính</label>
                                <span class="form-note">Thông tin địa chỉ văn phòng</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitediachi" runat="server" placeholder="36 Tôn Thất Đạm, Quận 1, TP.Hồ Chí Minh">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Địa chỉ Email</label>
                                <span class="form-note">Thông tin Email liên hệ</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="siteemail" runat="server" placeholder="info@mail.com">
                                    <mark><small> settingPagesiteemail</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Điện thoại</label>
                                <span class="form-note">Thông tin số điện thoại liên hệ</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitedienthoai" runat="server" placeholder="(028) 38 291901">
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Tên chi nhánh 1:</label>
                                <span class="form-note">Ví dụ: Cơ sở Hàm Nghi</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="tenchinhnhanh1" runat="server" placeholder="Cơ sở Hàm Nghi">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Địa chỉ</label>
                                <span class="form-note">Thông tin địa chỉ chi nhánh</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitediachi1" runat="server" placeholder="39 Hàm Nghi, Quận 1, TP.Hồ Chí Minh">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Địa chỉ Email</label>
                                <span class="form-note">Thông tin Email liên hệ</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="siteemail1" runat="server" placeholder="info@buh.edu.vn">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Điện thoại</label>
                                <span class="form-note">Thông tin số điện thoại liên hệ</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitedienthoai1" runat="server" placeholder="(028) 38 291901">
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Tên chi nhánh 2:</label>
                                <span class="form-note">Ví dụ: Cơ sở Hoàng Diệu</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="tenchinhnhanh2" runat="server" placeholder="Cơ sở Hoàng Diệu">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Địa chỉ</label>
                                <span class="form-note">Thông tin địa chỉ chi nhánh</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitediachi2" runat="server" placeholder=" 56 Hoàng Diệu II, Q.Thủ Đức, TP.Hồ Chí Minh">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Địa chỉ Email</label>
                                <span class="form-note">Thông tin Email liên hệ</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="siteemail2" runat="server" placeholder="info@buh.edu.vn">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Điện thoại</label>
                                <span class="form-note">Thông tin số điện thoại liên hệ</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitedienthoai2" runat="server" placeholder="(028) 38 291901">
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Giới thiệu ngắn</label>
                                <span class="form-note">Tóm tắt giới thiệu về thông tin website</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:TextBox ID="sitetomtat" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="3" placeholder="Tóm tắt giới thiệu vể Website"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Thẻ Tag</label>
                                <span class="form-note">Nhập thẻ Tag cách nhau dấu "," </span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitetag" runat="server" placeholder="đại học ngân hàng,khoa ngân hàng,ngân hàng,thành phố hồ chí minh">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Facebook Page</label>
                                <span class="form-note">Link Facebook Page của Website</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitefacebookpage" runat="server" placeholder="https://www.facebook.com/DHNH.BUH/">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Youtube</label>
                                <span class="form-note">Link Youtube Page của Website</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="siteyoutube" runat="server" placeholder="https://www.youtube.com">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Instagram</label>
                                <span class="form-note">Link Instagram</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="siteInstagram" runat="server" placeholder="">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Zalo</label>
                                <span class="form-note">Link Zalo</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="siteZalo" runat="server" placeholder="">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Twitter</label>
                                <span class="form-note">Link Twitter</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="siteTwitter" runat="server" placeholder="">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Linkedin</label>
                                <span class="form-note">Link Linkedin</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="siteLinkedin" runat="server" placeholder="https://www.linkedin.com">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">whatsapp</label>
                                <span class="form-note">Link Linkedin</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitewhatsapp" runat="server" placeholder="">
                                    <mark><small> settingPagesitewhatsapp</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Skype</label>
                                <span class="form-note"></span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="siteSkype" runat="server" placeholder="">
                                    <mark><small> settingPagesiteSkype</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="row g-3 align-center">
                        <div class="col-lg-2">
                            <div class="form-group">
                                <label class="form-label">Chèn CODE vào Header</label>
                                <span class="form-note"></span>
                            </div>
                        </div>
                        <div class="col-lg-10">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:TextBox ID="siteHeaderCode" CssClass="form-control" runat="server" TextMode="MultiLine"  height="250px" placeholder=""></asp:TextBox>
                                    <mark><small> settingPagesiteHeaderCode</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-2">
                            <div class="form-group">
                                <label class="form-label">Chèn CODE vào Footer</label>
                                <span class="form-note"></span>
                            </div>
                        </div>
                        <div class="col-lg-10">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:TextBox ID="siteFooterCode" CssClass="form-control" runat="server" TextMode="MultiLine" height="250px" placeholder=""></asp:TextBox>
                                    <mark><small> settingPagesiteFooterCode</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <div class="col-md-5 col-sm-12 col-xs-12">
        <div class="card card-bordered">
            <div class="card-inner">
                <div class="card-head">
                    <h5 class="card-title">THIẾT LẬP HIỆN TRANG HIỆN THỊ</h5>
                </div>
                <div class="gy-3">
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label" for="site-name">PortalID</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:DropDownList ID="ddlPortalId" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label" for="site-name">Trang Tiếng Anh (Nếu có)</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:DropDownList ID="ddlPortalIdEn" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label" for="site-name">Chọn Trang Hiện thị tin tức:</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:DropDownList ID="ddlPageTinTuc" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label">Trang Hiện thị Tin Ảnh</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:DropDownList ID="ddlPageTinAnh" runat="server" CssClass="form-control "></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label">Trang Hiện thị Video</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:DropDownList ID="ddlPageVideo" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label">Trang Hiện thị Sự kiện</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:DropDownList ID="ddlPageEvents" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label">Thư mục Uplload</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:DropDownList ID="ddlfolder" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="card card-bordered">
            <div class="card-inner">
                <div class="card-head">
                    <h5 class="card-title">THIẾT LẬP CHUNG</h5>
                </div>
                <div class="gy-3">
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label" for="site-name">Logo</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input id="filelogo" runat="server" type="file" />
                                </div>
                                <div class="form-control-wrap">
                                    <div id="dvPreviewlogo" runat="server"></div>
                                    <asp:HiddenField ID="hpflinkimage" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </div>
        <div class="card card-bordered">
            <div class="card card-bordered">
                <div class="card-inner">
                    <div class="card-head">
                        <h5 class="card-title">THÔNG TIN KHÁC</h5>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label" for="site-name">Nhận Email Liên hệ</label>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:CheckBox ID="chkNhanEmail" runat="server" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center" id="EmailLienhe" runat="server" visible="false">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Danh sách Email nhận</label>
                                <span class="form-note">Các Email các nhau dấu <strong>,</strong> Và viết liền: mail@mail.com,mail2@mail.com </span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="sitemaillist" runat="server" placeholder="mail@mail.com,mail2@mail.com">
                                    <mark><small> settingPagesiteNhanMailList</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-head">
                        <h5 class="card-title">CDN</h5>
                    </div>
                    <div class="gy-3">
                        <div class="row g-3 align-center">
                            <div class="col-lg-5">
                                <div class="form-group">
                                    <label class="form-label" for="site-name">Static CDN</label>
                                </div>
                            </div>
                            <div class="col-lg-7">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type="text" class="form-control" id="sitecdn" runat="server">
                                        <mark><small> settingPageSiteCDN</small></mark>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="gy-3">
                        <div class="row g-3 align-center">
                            <div class="col-lg-5">
                                <div class="form-group">
                                    <label class="form-label" for="site-name">Files Server</label>
                                </div>
                            </div>
                            <div class="col-lg-7">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type="text" class="form-control" id="sitefileserver" runat="server">
                                        <mark><small> settingPageSiteFilesServer</small></mark>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-head">
                        <h5 class="card-title">GOOGLE CAPCHA</h5>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Google Capchat PublicKey</label>
                                <span class="form-note">Key</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="txtgoooglekey" runat="server">
                                    <mark><small> settingPageGooogleCapcha</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row g-3 align-center">
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="form-label">Google Capchat PrivateKey</label>
                                <span class="form-note">Secret Key</span>
                            </div>
                        </div>
                        <div class="col-lg-8">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="txtgoooglekeysecret" runat="server">
                                    <mark><small> settingPageGooogleCapchaSecret</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </div>
        <div class="card card-bordered">
            <div class="card-inner">
                <div class="card-head">
                    <h5 class="card-title">THIẾT LẬP MAIL SERVER</h5>
                </div>
                <div class="gy-3">
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label" for="site-name">SMTP Server</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="emailsmtp" runat="server">
                                    <mark><small> settingPageMailSMTP</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="gy-3">
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label" for="site-name">Tên hiện thị</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="emailtenhienthi" runat="server">
                                    <mark><small> settingPageMailTenHienThi</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="gy-3">
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label" for="site-name">Email</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="emailEmail" runat="server">
                                    <mark><small> settingPageMailEmail</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="gy-3">
                    <div class="row g-3 align-center">
                        <div class="col-lg-5">
                            <div class="form-group">
                                <label class="form-label" for="site-name">Mật khẩu</label>
                            </div>
                        </div>
                        <div class="col-lg-7">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <input type="text" class="form-control" id="emailmatkhau" runat="server">
                                    <mark><small> settingPageMailMatkhau</small></mark>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <div class="col-lg-7 offset-lg-5">
        <div class="form-group mt-2">
            <asp:LinkButton ID="lbtUpdate2" OnClientClick="return Validate();" runat="server" Font-Bold="True" class="btn btn-success">Cập nhật</asp:LinkButton>
            <asp:LinkButton ID="lbtCancelTop2" runat="server" Font-Bold="True" CssClass="btn btn-primary"> Thoát</asp:LinkButton>
        </div>
    </div>
</div>
<script type="text/javascript">
    window.onload = function () {
        fileUpload = document.getElementById('<%=filelogo.ClientID%>');
        fileUpload.onchange = function () {
            if (typeof (FileReader) != "undefined") {
                var dvPreviewlogo = document.getElementById('<%=dvPreviewlogo.ClientID%>');
                dvPreviewlogo.innerHTML = "";
                var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
                for (var i = 0; i < fileUpload.files.length; i++) {
                    var file = fileUpload.files[i];
                    if (regex.test(file.name.toLowerCase())) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            var img = document.createElement("IMG");
                            img.height = "100";
                            img.src = e.target.result;
                            dvPreviewlogo.appendChild(img);
                        }
                        reader.readAsDataURL(file);
                    } else {
                        alert(file.name + " is not a valid image file.");
                        dvPreviewlogo.innerHTML = "";
                        return false;
                    }
                }
            } else {
                alert("This browser does not support HTML5 FileReader.");
            }
        }
    };

</script>
<script type="text/javascript">

    function Validate() {
        var res = true;
        var chkNhanEmail = document.getElementById('<%=chkNhanEmail.ClientID %>').checked;
        if (chkNhanEmail == true) {
            var sitemaillist = document.getElementById('<%=sitemaillist.ClientID %>').value;
            if (sitemaillist == "") {
                alert('Vui lòng nhập email.');
                $('#<%= sitemaillist.ClientID%>').focus();
                return false;
            }
        }
        return res;
    }

</script>
<%--    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="PageUpdateProgress">
    <ProgressTemplate>
        <div id="loading">
            <div class="loading">
                <div></div>
                <div></div>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>

