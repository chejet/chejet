<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="XkCms.WebForm.Controls.Search" %>
<%@ Register Assembly="XkCms.Common" Namespace="XkCms.Common.Web" TagPrefix="cc1" %>
<asp:label id="lblId" runat="server" text="1" visible="False"></asp:label>
<asp:literal runat="server" id="ltTop"></asp:literal>
<form id="form1" runat="server">
<asp:panel runat="server" cssclass="SearchBar">
    ¹Ø¼ü×Ö£º<asp:TextBox runat="server" id="txtkeyWords"></asp:TextBox>
    <asp:Button runat="server" Text="ËÑË÷" id="btnSearch" OnClick="btnSearch_Click"></asp:Button>
</asp:panel>
<asp:panel runat="server" cssclass="SearchList" id="SearchList">
<asp:repeater runat="server" id="rpList">
    <HeaderTemplate>
    <ul class="repeater">
    <li class="repeaterTitle">ËÑË÷½á¹û</li>
    </HeaderTemplate>
    <ItemTemplate>
        <li class="row1">
           [<a href='List.aspx?id=<%# Eval("ColumnId") %>' target="_blank"><%#Eval("ColumnName") %></a>]
           <a href='View.aspx?id=<%# Eval("id") %>' target="_blank"><%# Eval("title") %></a>  
           <%#Eval("AddDate") %>
        </li>
        <li class="row2">
            <b>¹Ø¼ü×Ö£º</b><%# Eval("KeyWord") %>
        </li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:repeater>
<cc1:AspNetPager ID="AspNetPager1" runat="server"  AlwaysShow="True" OnPageChanged="AspNetPager1_PageChanged"></cc1:AspNetPager>
</asp:panel>
    <asp:hiddenfield runat="server" id="hfChannelType"></asp:hiddenfield>
</form>
<asp:literal id="ltBottom" runat="server"></asp:literal>