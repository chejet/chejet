<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectCorrelations.aspx.cs" Inherits="XkCms.WebForm.Manager.SelectCorrelations" %>

<%@ Register Assembly="XkCms.Common" Namespace="XkCms.Common.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>添加新相关内容</title>
    <base target="_self">
    </base>
    <meta http-equiv="pragma" content="no-cache">
</head>
<body>
    <form id="form1" runat="server">
    <table align="center" cellpadding="1" cellspacing="1" class="TableBorder" width="100%">
        <tr>
            <td align="left" class="forumRow" height="22">
                添加新相关内容
            </td>
        </tr>
    </table>
    <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
    <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" CellPadding="5"
        CssClass="tableBorder" GridLines="None" Width="100%" OnRowDataBound="gvList_RowDataBound" ShowHeader="False">
        <Columns>
            <asp:TemplateField>
                <HeaderStyle Width="10%" />
                <ItemTemplate>
                    <input id="chkContentId" class="checkbox" name="chkContentId" type="checkbox" value='<%#Eval("id") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="标题" DataField="title" />
        </Columns>
    </asp:GridView>
    <table align="center" cellpadding="1" cellspacing="1" class="TableFooter" width="100%">
        <tr>
            <td>
                <cc1:AspNetPager ID="AspNetPager1" runat="server" AlwaysShow="True" OnPageChanged="AspNetPager1_PageChanged">
                </cc1:AspNetPager>
            </td>
            <td width="70">
                <asp:Button ID="btnReturnOk" runat="server" OnClick="btnReturnOk_Click" Text="确定" /></td>
        </tr>
    </table>
    </asp:Panel>
    </form>
</body>
</html>
