<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Review.aspx.cs" Inherits="XkCms.WebForm.Controls.Review" %>
<%@ Register Assembly="XkCms.Common" Namespace="XkCms.Common.Web" TagPrefix="cc1" %>
<asp:label id="lblId" runat="server" text="{$ChannelId$}" visible="False"></asp:label>
<asp:literal runat="server" id="ltTop"></asp:literal>
<form id="form1" runat="server">
<asp:panel runat="server" cssclass="ReviewList">
<asp:repeater runat="server" id="rpList">
    <HeaderTemplate>
    <ul class="repeater">
    <li class="repeaterTitle">查看评论</li>
    </HeaderTemplate>
    <ItemTemplate>
        <li class="row1"><b>用户名：</b><asp:Label runat="server" Text='<%# Eval("UserName") %>'  ForeColor="Red" id="author"></asp:Label>&nbsp;
            &nbsp;&nbsp;<b>发表时间：</b><asp:Label runat="server" Text='<%# Eval("AddDate") %>' id="datetime"></asp:Label>&nbsp;&nbsp;&nbsp;
            <b>用户IP：</b><asp:Label runat="server" Text='<%# Eval("IP") %>' id="ip"></asp:Label>
        </li>
        <li class="row2">
        <asp:Label runat="server" Text='<%# Eval("content") %>' id="content"></asp:Label>
        </li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:repeater>
<cc1:AspNetPager ID="AspNetPager1" runat="server" AlwaysShow="True" OnPageChanged="AspNetPager1_PageChanged"></cc1:AspNetPager>
</asp:panel>
</form>
<asp:literal id="ltBottom" runat="server"></asp:literal>