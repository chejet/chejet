<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Uploader.aspx.cs" Inherits="XkCms.WebForm.UserBox.Uploader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>文件上传</title>
    <base target="_self">
    </base>
    <meta http-equiv="pragma" content="no-cache">
    <script type="text/javascript">
        function ReturnOk(){
            window.returnValue = document.getElementById("HiddenFieldFiles").value;
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:Panel ID="plList" runat="server" HorizontalAlign="Center" width="330">
        <P class="UserTableBorder" style="width: 330px; height: 122px;text-align:left;margin:1px"><INPUT contentEditable="false" type="file" size="40" name="m_file" id="m_file">
        <asp:Label ID="Label2" runat="server" Text="Label" Width="320px"></asp:Label></P>
        </asp:Panel>
        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center" width="330">
        <P class="UserTableBorder" style="width: 330px; height: 122px">
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </P>
        </asp:Panel>
        <table cellpadding="1" cellspacing="1" class="TableFooter" width="330">
            <tr>
                <td align="right">
                    <asp:CheckBox ID="chkReName" runat="server" Text="重命名文件" Checked="True" />&nbsp;
                <asp:Button id="Button1" runat="server" Text="上传" onclick="Button1_Click" onload="Button1_Load"></asp:Button>&nbsp;
                <input id="ButtonOk" type="button" value="确定" onclick="ReturnOk();" />&nbsp;</td>
        </tr>
        </table>
        <asp:HiddenField ID="HiddenFieldType" runat="server" />
        <asp:HiddenField ID="HiddenFieldFiles" runat="server" />
    </form>
</body>
</html>
