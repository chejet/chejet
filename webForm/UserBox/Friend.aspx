<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Friend.aspx.cs" Inherits="XkCms.WebForm.UserBox.Friend" %>

<%@ Register Assembly="XkCms.Common" Namespace="XkCms.Common.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
            <asp:GridView ID="gvList" runat="server" CellPadding="3" CssClass="Usertableborder" AutoGenerateColumns="False" BorderWidth="1px" EmptyDataText="暂无内容" OnRowDeleting="gvList_RowDeleting" OnRowDataBound="gvList_RowDataBound">
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="LinkURL" DataTextField="LinkName" HeaderText="名称"
                        Target="_blank">
                        <ItemStyle Width="15%" Wrap="False" />
                    </asp:HyperLinkField>
                    <asp:ImageField DataImageUrlField="LinkImgPath" HeaderText="LOGO">
                        <ItemStyle HorizontalAlign="Center" Width="10%" Wrap="False" />
                    </asp:ImageField>
                    <asp:BoundField DataField="LinkInfo" HeaderText="简介" />
                    <asp:TemplateField HeaderText="状态">
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                        <ItemTemplate>
                            <%# ChkState(Eval("ispass", "{0}")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" ShowHeader="False">
                        <ItemStyle Width="15%" />
                        <ItemTemplate>
                            <a href='?action=add&id=<%#Eval("id") %>'>编辑</a>
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                                Text="删除"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <cc1:AspNetPager ID="AspNetPager1" runat="server" AlwaysShow="True" OnPageChanged="AspNetPager1_PageChanged">
            </cc1:AspNetPager>
        </asp:Panel>
        <asp:Panel ID="plEdit" runat="server" Width="100%">
            <table class="Usertableborder" cellspacing="1" cellpadding="3" align="center" border="0">
                <tr>
                    <th colspan="2">申请友情链接</th>
                </tr>
                <tr>
                    <td width="30%" align="right">
                        网站名称</td>
                    <td align="left">
                        <asp:TextBox ID="txtName" runat="server" MaxLength="50" Width="240px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td width="30%" align="right">
                        网站地址</td>
                    <td align="left">
                        <asp:TextBox ID="txtUrl" runat="server" MaxLength="200" Width="240px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUrl"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td align="right" width="30%">
                        申请频道</td>
                    <td align="left">
                        <asp:DropDownList ID="ddlChannel" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td width="30%" align="right">
                        网站LOGO</td>
                    <td align="left">
                        <asp:TextBox ID="txtLogo" runat="server" MaxLength="200" Width="240px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtLogo"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td width="30%" align="right">
                        网站简介</td>
                    <td align="left">
                        <asp:TextBox ID="txtInfo" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="70px"
                            MaxLength="254" TextMode="MultiLine" Width="240px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtInfo"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td width="30%" align="right">
                        链接样式</td>
                    <td align="left">
                        <asp:RadioButtonList ID="rblStyle" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">图片</asp:ListItem>
                            <asp:ListItem Selected="True" Value="0">文本</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td width="30%" align="right">
                        申请人</td>
                    <td align="left">
                        <asp:Label ID="lblUserName" runat="server" ForeColor="Red" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td width="30%" align="right"></td>
                    <td align="left">
                        <asp:Button ID="btnSave" runat="server" Text="Button" Width="85px" /></td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
