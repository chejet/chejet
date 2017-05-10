<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Favorite.aspx.cs" Inherits="XkCms.WebForm.UserBox.Favorite" %>

<%@ Register Assembly="XkCms.Common" Namespace="XkCms.Common.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:GridView ID="gvList" runat="server" CellPadding="3" CssClass="Usertableborder" AutoGenerateColumns="False" BorderWidth="1px" EmptyDataText="暂无收藏" GridLines="None" OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting" CellSpacing="1">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="url" DataTextField="title" HeaderText="我的收藏"
                    Target="_blank" />
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="40px" Wrap="False" />
                    <HeaderStyle Width="40px" Wrap="False" />
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
            <cc1:aspnetpager id="AspNetPager1" runat="server" AlwaysShow="True"></cc1:aspnetpager>
        </asp:Panel>
    
    </form>
</body>
</html>
