<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Source.aspx.cs" Inherits="XkCms.WebForm.Manager.Source" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <table align="center" cellpadding="1" cellspacing="1" class="TableBorder" width="98%">
        <tr>
            <td align="left" class="forumRow" height="22" width="40%">
                当前位置：<asp:Label ID="lblChannel" runat="server" Text="Label"></asp:Label> --> 来源管理
            </td>
            <td class="forumRow" align="center" width="20%">
                <a href="?ChannelId=<%=Channel.Id %>"><b>管理首页</b></a> | <a href="?action=add&ChannelId=<%=Channel.Id %>"><b>添加</b></a>
            </td>
            <td align="right" class="forumRow" width="40%"></td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:GridView ID="gvList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            CssClass="TableBorder" EmptyDataText="暂无信息" GridLines="None" OnPageIndexChanging="gvList_PageIndexChanging"
            OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting" PageSize="15"
            Width="98%">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID">
                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="title" HeaderText="名称">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:HyperLinkField DataTextField="url" HeaderText="地址" Target="_blank">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:HyperLinkField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                    <HeaderStyle HorizontalAlign="Center" Width="120px" />
                    <ItemTemplate>
                        <a href='?action=add&id=<%# Eval("id") %>&ChannelId=<%=Channel.Id %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings FirstPageText="第一页" LastPageText="最末页" Mode="NextPreviousFirstLast"
                NextPageText="下一页" PreviousPageText="上一页" />
            <FooterStyle HorizontalAlign="Right" />
            <PagerStyle ForeColor="RoyalBlue" HorizontalAlign="Right" VerticalAlign="Bottom" />
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="plEdit" runat="server" Width="100%">
        <table align="center" border="0" cellpadding="5" cellspacing="0" class="TableBorder"
            width="98%">
            <tr align="center" valign="middle">
                <th colspan="2" height="22">
                    链接管理</th>
            </tr>
            <tr bgcolor="#ffffff" valign="middle">
                <td align="right" width="20%">
                    网站名称
                </td>
                <td align="left">
                    <asp:TextBox ID="txtName" runat="server" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr bgcolor="#ffffff" valign="middle">
                <td align="right" width="20%">
                    网站连接地址
                </td>
                <td align="left">
                    <asp:TextBox ID="txtUrl" runat="server" Width="280px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUrl"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr class="tablefoot">
                <td></td>
                <td align="left" valign="middle">
                    <asp:Button ID="Button1" runat="server" Text="保 存" /></td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
