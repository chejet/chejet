<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPass.aspx.cs" Inherits="XkCms.WebForm.UserBox.UserPass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
        <table class="Usertableborder" cellspacing="1" cellpadding="3" align="center" border="0">
            <tr>
                <th colspan="2">修改密码</th>
            </tr>
            <tr>
                <td width="40%" align="right">用户名：</td>
                <td align="left">
                    <asp:Label ID="lblUserName" runat="server" ForeColor="Red" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td width="40%" align="right">原密码：</td>
                <td align="left">
                    <asp:TextBox ID="txtOldPass" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                        ErrorMessage="*" ControlToValidate="txtOldPass"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td width="40%" align="right">新密码：</td>
                <td align="left">
                    <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                        ErrorMessage="*" ControlToValidate="txtNewPass"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                            runat="server" Display="Dynamic" ErrorMessage="6到20个字符" ValidationExpression="^.{6,20}$" ControlToValidate="txtNewPass"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td width="40%" align="right">确认密码：</td>
                <td align="left">
                    <asp:TextBox ID="txtNewPass2" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                        ErrorMessage="*" ControlToValidate="txtNewPass2"></asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValidator1"
                            runat="server" Display="Dynamic" ErrorMessage="两次密码不一致" ControlToCompare="txtNewPass" ControlToValidate="txtNewPass2"></asp:CompareValidator></td>
            </tr>
            <tr>
                <td></td>
                <td align="left">
                    <asp:Button ID="btnSave" runat="server" Text="确 认" OnClick="btnSave_Click" /></td>
            </tr>
        </table>
    </form>
</body>
</html>
