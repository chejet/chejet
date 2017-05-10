<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSms.aspx.cs" Inherits="XkCms.WebForm.UserBox.UserSms" %>

<%@ Register Assembly="XkCms.Common" Namespace="XkCms.Common.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
        <table class="Usertableborder" cellspacing="1" cellpadding="3" align="center" border="0">
            <tr>
                <th colspan="2">用户短信服务</th>
            </tr>
            <tr>
                <td align="center" valign="middle" nowrap="nowrap">
                    <a href="?box=in"><img src="Images/m_inbox.gif" border="0" /></a>
                    <a href="?box=to"><img src="Images/M_issend.gif" border="0" /></a>
                    <a href="?action=delall"><img src="Images/recycle.gif" border="0" /></a>
                    <a href="?action=add"><img src="Images/m_write.gif" border="0" /></a>
                </td>
                <td valign="middle" nowrap="nowrap" align="center">
                    <table>
                    <tr>
                    <td align="right" nowrap="nowrap">邮箱容量：</td>
                    <td align="left" nowrap="nowrap" valign="middle">
                        <table width='250' height='10' border='1' cellpadding='0' cellspacing='0' style='TABLE-LAYOUT: fixed; WORD-BREAK: break-all;border-collapse:collapse' bordercolor='#FFCC00' bgcolor='#CCFF00'>
                        <tr><td width='250'><asp:Literal ID="ltSmsBar" runat="server"></asp:Literal></td>
                        </tr>
                        </table>
                    </td>
                    <td align="left" nowrap="nowrap">已使用：<asp:Label ID="lblSmsUse" runat="server" Text=""></asp:Label></td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br style="overflow: hidden; line-height: 10px">
        <asp:Panel ID="plEdit" runat="server" Width="100%" HorizontalAlign="Center">
            <table class="Usertableborder" cellspacing="1" cellpadding="3" align="center" border="0">
                <tr>
                    <th colspan="2">发送短信</th>
                </tr>
                <tr>
                    <td align="right">收件人</td>
                    <td align="left">
                        <asp:TextBox ID="txtToUser" runat="server" Width="400px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtToUser" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td align="right">标题</td>
                    <td align="left">
                        <asp:TextBox ID="txtTitle" runat="server" Width="400px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTitle" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td align="right">内容</td>
                    <td align="left">
                        <asp:TextBox ID="txtContent" runat="server" Height="110px" TextMode="MultiLine" Width="400px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtContent" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td></td>
                    <td class="foot" align="left">
                        <asp:Button ID="btnSend" runat="server" Text="发 送" Width="105px" OnClick="btnSend_Click" /></td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
            <asp:Repeater ID="rpInList" runat="server">
                <HeaderTemplate>
                    <table class="Usertableborder" cellspacing="1" cellpadding="3" align="center" border="0">
                        <tr>
                            <td class="title" colspan="5">收 件 箱(<a href="?action=delin">清空</a>)</td>
                        </tr>
                        <tr>
                            <th width="30"></th>
                            <th width="150">发件人</th>
                            <th width="200">主题</th>
                            <th width="120">日期</th>
                            <th width="30"></th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                        <tr>
                            <td nowrap="nowrap" width="30" align="center"><%# CheckState(Eval("IsRead", "{0}"), Eval("flag", "{0}"))%></td>
                            <td nowrap="nowrap" width="150"><%# Eval("fromUser") %></td>
                            <td nowrap="nowrap" width="200"><a href='?action=view&id=<%# Eval("id") %>'><%# Eval("title") %></a></td>
                            <td nowrap="nowrap" width="120"><%# Eval("SendTime") %></td>
                            <td nowrap="nowrap" width="50" align="center"><a href='?action=del&box=in&id=<%# Eval("id") %>'>删除</a></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Repeater ID="rpOutList" runat="server">
                <HeaderTemplate>
                    <table class="Usertableborder" cellspacing="1" cellpadding="3" align="center" border="0">
                        <tr>
                            <td class="title" colspan="5">发 件 箱(<a href="?action=delto">清空</a>)</td>
                        </tr>
                        <tr>
                            <th width="30"></th>
                            <th width="150">收件人</th>
                            <th width="200">主题</th>
                            <th width="120">日期</th>
                            <th width="50"></th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                        <tr>
                            <td nowrap="nowrap" width="30" align="center"><%# CheckState(Eval("IsRead", "{0}"), Eval("flag", "{0}"))%></td>
                            <td nowrap="nowrap" width="150"><%# Eval("toUser") %></td>
                            <td nowrap="nowrap" width="200"><a href='?action=view&id=<%# Eval("id") %>'><%# Eval("title") %></a></td>
                            <td nowrap="nowrap" width="120"><%# Eval("SendTime") %></td>
                            <td nowrap="nowrap" width="50" align="center"><a href='?action=del&box=to&id=<%# Eval("id") %>'>删除</a></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <cc1:AspNetPager ID="AspNetPager1" AlwaysShow="true" runat="server">
            </cc1:AspNetPager>
        </asp:Panel>
        <asp:Panel ID="plView" runat="server" Width="100%">
            <table class="Usertableborder" cellspacing="1" cellpadding="3" align="center" border="0">
                <tr>
                    <th colspan="2">
                        阅读短信：<asp:Label ID="lblSmsTitle" runat="server"></asp:Label></th>
                </tr>
                <tr>
                    <td align="left" class="oper">
                        From：<asp:Label ID="lblFromUser" runat="server"></asp:Label>
                        &nbsp;&nbsp; To：<asp:Label ID="lblToUser" runat="server"></asp:Label>
                        &nbsp;&nbsp; 时间：<asp:Label ID="lblSendTime" runat="server"></asp:Label></td>
                    <td align="right" class="oper">
                        <a href="?action=del&id=<%=id %>">删除</a>
                        <a href="?action=add&id=<%=id %>&oper=rw">回复</a>
                        <a href="?action=add&id=<%=id %>&oper=fw">转发</a>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblSmsContent" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
