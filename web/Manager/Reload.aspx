<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reload.aspx.cs" Inherits="XkCms.WebForm.Manager.Reload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <table width="95%" cellpadding="0" cellspacing="0" align="center" class=TableBorder>
	    <tr height=22>
		    <th>系统数据更新</th>
	    </tr>
	    <tr>
		    <td class=forumRowHighlight>&nbsp;<B>注意</B>：下列这些操作可能会非常消耗服务器资源，请慎重使用</td>
	    </tr>
        <tr height=22>
		    <td align="center" id="show"></td>
	    </tr>
        <tr height=22>
		    <td style="padding-left: 50px;" align="left">
                <asp:Button ID="Button1" runat="server" Text="更新系统缓存"  Width="140px" OnClick="Button1_Click" CausesValidation="False" />&nbsp;
                <asp:Button ID="Button2" runat="server" Text="更新栏目统计数据" Width="140px" OnClick="Button2_Click" CausesValidation="False" /><br />
                <br />
                <asp:Button ID="Button3" runat="server" Text="更新系统统计数据" Width="140px" OnClick="Button3_Click" CausesValidation="False" />&nbsp;
                <asp:Button ID="Button4" runat="server" Text="生成系统JS" Width="140px" OnClick="Button4_Click" CausesValidation="False" />
                <br />
                <br />
                <asp:Button ID="Button5" runat="server" CausesValidation="False" OnClick="Button5_Click"
                    Text="压缩数据库" Width="140px" />
                <span style="color:Red">*压缩前请先备份</span><br />
                <br />
                备份/还原数据库：<asp:TextBox ID="txtMdbName" runat="server" ValidationGroup="backup" Width="100px">backup.mdb</asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMdbName"
                    ErrorMessage="*" ValidationGroup="backup" ToolTip="此项不允许为空"></asp:RequiredFieldValidator>&nbsp;<asp:Button
                        ID="btnBackup" runat="server" Text="备份" ValidationGroup="backup" OnClick="btnBackup_Click" />&nbsp;<asp:Button
                            ID="btnRestore" runat="server" Text="还原" ValidationGroup="backup" OnClick="btnRestore_Click" />(Access版使用)</td>
	    </tr>
        <tr height="22">
            <td style="height: 22px"></td>
        </tr>
    </table>
    </form>
</body>
</html>
