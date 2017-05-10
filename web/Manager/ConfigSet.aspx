<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigSet.aspx.cs" Inherits="XkCms.WebForm.Manager.ConfigSet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <table align="center" cellpadding="1" cellspacing="1" class="TableBorder" width="98%">
        <tr height="22" align="center">
            <td align="left" class="forumRow" width="40%">
                当前位置：系统设置
            </td>
            <td align="right" class="forumRow">
                <span style="color:Red" title="下面各项请不要使用"|"，否则将会影响系统正常访问。">注意事项</span>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="Panel1" runat="server" Width="100%">
    <table width="98%" border="0" cellspacing="0" cellpadding="5" align=center class="tableBorder">
        <tr>
          <th height=25 colspan=2>基本设置</th>
        </tr>
        <tr>
          <td align=right width="40%">系统名称</td>
          <td width="60%"><asp:TextBox ID="txtName" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                  ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
          <td align=right class=forumRowHighlight>系统URL</td>
          <td class=forumRowHighlight>
            <asp:TextBox ID="txtUrl" runat="server" MaxLength="100" Width="200px"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                安装目录
            </td>
            <td>
                <asp:TextBox ID="txtInstallDir" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtInstallDir"
                    Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
          <td align=right class=forumRowHighlight>系统是否打开</td>
          <td class=forumRowHighlight><asp:RadioButtonList ID="rblOpen" runat="server" RepeatDirection="Horizontal">
                  <asp:ListItem Value="1">开</asp:ListItem>
                  <asp:ListItem Value="0">关</asp:ListItem>
              </asp:RadioButtonList></td>
        </tr>
        <tr>
          <td align=right>关闭时的公告语</td>
          <td><asp:TextBox ID="txtMessage" runat="server" Height="60px" MaxLength="200" TextMode="MultiLine"
                  Width="300px"></asp:TextBox></td>
        </tr>
        <tr>
          <td align=right class=forumRowHighlight>系统关键字&nbsp;</td>
          <td class=forumRowHighlight><asp:TextBox ID="txtKeys" runat="server" MaxLength="50" Width="200px" ToolTip="英文逗号隔开"></asp:TextBox></td>
        </tr>
	    <tr>
          <td align=right>系统描述&nbsp;</td>
          <td><asp:TextBox ID="txtDescription" runat="server" Height="60px" MaxLength="100" TextMode="MultiLine"
                  Width="300px"></asp:TextBox></td>
        </tr>
	    <tr>
          <td align=right class=forumRowHighlight>系统是否定时打开</td>
          <td class=forumRowHighlight><asp:RadioButtonList ID="rblClose" runat="server" RepeatDirection="Horizontal">
                  <asp:ListItem Value="1">是</asp:ListItem>
                  <asp:ListItem Value="0">否</asp:ListItem>
              </asp:RadioButtonList></td>
        </tr>
        <tr>
          <td align=right>定时开关的时间</td>
          <td><asp:TextBox ID="txtOpenTime" runat="server" MaxLength="5" Width="80px" ToolTip="起止小时以英文逗号隔开"></asp:TextBox></td>
        </tr>
	    <tr>
          <td align=right class=forumRowHighlight>系统板式</td>
          <td class=forumRowHighlight><asp:RadioButtonList ID="rblSkin" runat="server" RepeatDirection="Horizontal">
                  <asp:ListItem Value="1">Aspx版</asp:ListItem>
                  <asp:ListItem Value="0">Html版</asp:ListItem>
              </asp:RadioButtonList></td>
        </tr>
        <tr>
          <td align=right>Html首页文件名</td>
          <td><asp:TextBox ID="txtHtmlIndex" runat="server" MaxLength="20" Width="200px" ToolTip="不能为Default.aspx"></asp:TextBox></td>
        </tr>
        <tr>
          <td align=right class=forumRowHighlight>是否开启注册</td>
          <td class=forumRowHighlight><asp:RadioButtonList ID="rblReg" runat="server" RepeatDirection="Horizontal">
                  <asp:ListItem Value="1">是</asp:ListItem>
                  <asp:ListItem Value="0">否</asp:ListItem>
              </asp:RadioButtonList></td>
        </tr>
        <tr>
          <td align=right>是否EMAIL只允许注册一个ID</td>
          <td><asp:RadioButtonList ID="rblEmail" runat="server" RepeatDirection="Horizontal">
                  <asp:ListItem Value="1">是</asp:ListItem>
                  <asp:ListItem Value="0">否</asp:ListItem>
              </asp:RadioButtonList></td>
        </tr>
        <tr>
          <td align=right class=forumRowHighlight>
              注册需要管理员审核</td>
          <td class=forumRowHighlight><asp:RadioButtonList ID="rblAdmin" runat="server" RepeatDirection="Horizontal">
                  <asp:ListItem Value="1">是</asp:ListItem>
                  <asp:ListItem Value="0">否</asp:ListItem>
              </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td align="right">
            注册会员增加的点数
            </td>
            <td>
                <asp:TextBox ID="txtUserPoint" runat="server" ToolTip="不限制请设置为0" Width="80px">50</asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUserPoint"
                    Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                        ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtUserPoint"
                        Display="Dynamic" ErrorMessage="*" ToolTip="必须为整数" ValidationExpression="^\d*$"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td align="right" class=forumRowHighlight>
                会员上传文件大小限制</td>
            <td class=forumRowHighlight>
                <asp:TextBox ID="txtUserUpload" runat="server" ToolTip="不限制请设置为0" Width="80px" MaxLength="6">2048</asp:TextBox>
                <b>K</b><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtUserUpload"
                    Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                        ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtUserUpload"
                        Display="Dynamic" ErrorMessage="*" ToolTip="必须为整数" ValidationExpression="^\d*$"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td align="right">
                后台上传文件大小限制</td>
            <td>
                <asp:TextBox ID="txtMasterUpload" runat="server" MaxLength="6" ToolTip="不限制请设置为0" Width="80px">204800</asp:TextBox>
                <b>K</b><asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtMasterUpload"
                    Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                        ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtMasterUpload"
                        Display="Dynamic" ErrorMessage="*" ToolTip="必须为整数" ValidationExpression="^\d*$"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <th align="center" colspan="2">
                其它设置</th>
        </tr>
        <tr>
            <td align="right" class="forumRowHighlight" width="40%">
                后台目录名
            </td>
            <td class="forumRowHighlight" width="60%">
                <asp:TextBox ID="txtManagerDir" runat="server" MaxLength="100" Width="200px" ToolTip="相对安装目录的名称">Manager</asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtInstallDir"
                    Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right" width="40%">
                FckEditor路径名</td>
            <td>
                <asp:TextBox ID="txtEditorDir" runat="server" MaxLength="100" Width="200px" ToolTip="相对安装目录的名称">fckEditor</asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtInstallDir"
                    Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td align="right" class="forumRowHighlight" width="40%">
                是否添加水印</td>
            <td width="60%" class="forumRowHighlight"><asp:RadioButtonList ID="rblIsWater" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="1">是</asp:ListItem>
                <asp:ListItem Value="0">否</asp:ListItem>
            </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td align="right" width="40%">
                水印图片名称</td>
            <td>
                <asp:TextBox ID="txtImagePath" runat="server" MaxLength="100" Width="200px" ToolTip="相对安装目录的名称">Images/xkwaterpic.png</asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtInstallDir"
                    Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
        </tr>
        <tr class="tablefoot" align=center>
          <td colspan="2" height="25"><asp:Button ID="Button1" runat="server" Text="保  存" OnClick="Button1_Click" /></td>
        </tr>
    </table>
    </asp:Panel>
    </form>
</body>
</html>