<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="XkCms.WebForm.UserBox.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
        <table class="Usertableborder" cellspacing="1" cellpadding="3" align="center" border="0">
            <tr>
                <th colspan="2">用户控制面板 -- 首页</th>
            </tr>
            <tr>
                <td width="50%"><b class=userfont2>用户名称：</b><asp:Label ID="lblUserName" runat="server" Text="Label"></asp:Label></td>
                <td><b class=userfont2>用户身份：</b><asp:Label ID="lblUserGroup" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td width="50%"><b class=userfont2>用户昵称：</b><asp:Label ID="lblNickName" runat="server" Text="Label"></asp:Label></td>
                <td><b class=userfont2>真实姓名：</b><asp:Label ID="lblTrueName" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td width="50%"><b class=userfont2>可用点数：</b><asp:Label ID="lblUserPoint" runat="server" Text="Label"></asp:Label></td>
                <td><b class=userfont2>用户经验：</b><asp:Label ID="lblExperince" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td width="50%"><b class=userfont2>用户魅力：</b><asp:Label ID="lblCharm" runat="server" Text="Label"></asp:Label></td>
                <td><b class=userfont2>注册日期：</b><asp:Label ID="lblRegTime" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td width="50%"><b class=userfont2>上次登录时间：</b><asp:Label ID="lblLastTime" runat="server" Text="Label"></asp:Label></td>
                <td><b class=userfont2>上次登录IP：</b><asp:Label ID="lblLastIp" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td width="50%"><b class=userfont2>登录次数：</b><asp:Label ID="lblLogin" runat="server" Text="Label"></asp:Label></td>
                <td><a href="UserSms.aspx">我的信箱(<asp:Label ID="lblMsg" runat="server" Text="Label"></asp:Label>)</a></td>
            </tr>
        </table>
    </form>
</body>
</html>
