<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="TopView.ascx.vb" Inherits="BUH.Modules.Video.TopView" %>
<div id="video_topView" class="list-video"> 
    <ul class="list-item">
        <asp:Repeater runat="server" ID="rptVideo" ViewStateMode="Disabled">
            <ItemTemplate>
                <li class="item item-<%# Container.ItemIndex%>">
                    <a title=' <%# DataBinder.Eval(Container.DataItem, "Title")%>' href='<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem,"Danhmuc"), Integer)),CType(DataBinder.Eval(Container.DataItem,"id"), Integer),CType(DataBinder.Eval(Container.DataItem,"Title"), String)) %>'>
                        <div class="border-img-pl">
                            <img title='<%# DataBinder.Eval(Container.DataItem, "Title")%>' src='<%# Ultis.FormatThumbImage(CType(DataBinder.Eval(Container.DataItem,"Avatar"), String), 80, 55, "crop", "topcenter", "") %>'
                                alt='<%# DataBinder.Eval(Container.DataItem, "Title")%>' />                            
                            <div class="h2Title">
                            <%# Ultis.SubString(DataBinder.Eval(Container.DataItem, "Title"),14,"...")%>
                            </div>                          
                        </div>                        
                    </a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>
       