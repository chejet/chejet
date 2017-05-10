<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MakeHtml.aspx.cs" Inherits="XkCms.WebForm.Manager.MakeHtml" %>

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
                当前位置：<asp:Label ID="lblChannel" runat="server" Text="Label"></asp:Label> --&gt; 生成其它页面
            </td>
            <td align="center" class="forumRow" width="20%">
                <a href="?ChannelId=<%=Channel.Id %>"><b>管理首页</b></a> | <a href="?action=add&ChannelId=<%=Channel.Id %>">
                    <b>添加</b></a>
            </td>
            <td align="right" class="forumRow" width="40%">
            </td>
        </tr>
    </table>
    <br />
    <table width="98%" cellpadding="5" cellspacing="0" align="center" class=TableBorder>
        <tr height=22>
		    <th>生成Html管理</th>
	    </tr>
        <tr>
		    <td align="left">
                <asp:Button ID="btnMakeAll" runat="server" OnClick="btnMakeAll_Click" Text="生成本频道所有页面"
                    Width="140px" />
                <asp:Button ID="btnMakeIndex" runat="server" OnClick="btnMakeIndex_Click" Text="生成频道首页" Width="140px" />&nbsp;<asp:Button
                    ID="btnMakeMoreDiss" runat="server" OnClick="btnMakeMoreDiss_Click" Text="生成过往专题页" Width="140px" />
                <asp:Button ID="btnMakeDissList" runat="server" OnClick="btnMakeDissList_Click" Text="生成专题列表页" Width="140px" /></td>
	    </tr>
	    <tr height=22>
		    <td align="center" class="forumRowHighlight"><b>按栏目生成</b></td>
	    </tr>
        <tr>
            <td align="left">
                选择栏目
                <asp:DropDownList ID="ddlColumn" runat="server">
                </asp:DropDownList>
                <asp:Button ID="btnMakeColumn" runat="server" Text="生成列表页" OnClick="btnMakeColumn_Click" />
                <asp:Button ID="btnMakeView" runat="server" Text="生成内容页" OnClick="btnMakeView_Click" />
                此操作同时生成下属栏目</td>
        </tr>
        <tr height=22>
		    <td align="center" class=forumRowHighlight><b>按ID批量生成内容浏览页</b></td>
	    </tr>
	    <tr valign="middle"> 
		    <td height="22" bgcolor="ffffff" align="left">&nbsp;&nbsp;开始id：<asp:TextBox ID="txtId1" runat="server" Width="60px" ValidationGroup="id"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtId1"
                    Display="Dynamic" ErrorMessage="*" ValidationGroup="id" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                        ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtId1" Display="Dynamic"
                        ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数" ValidationGroup="id"></asp:RegularExpressionValidator>
                内容ID，只填开始id，则生成从开始id到最大id的内容</td>
        </tr>
        <tr valign="middle"> 
		    <td height="22" bgcolor="ffffff" align="left">&nbsp;&nbsp;结束id：<asp:TextBox ID="txtId2" runat="server" Width="60px" ValidationGroup="id"></asp:TextBox>&nbsp;<asp:RegularExpressionValidator
                ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtId2" Display="Dynamic"
                ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数" ValidationGroup="id"></asp:RegularExpressionValidator>
                结束id不能小于开始id，如果和开始id相同，则只生成此id的内容
            </td>
        </tr>
	    <tr height=22>
		    <td>&nbsp;&nbsp;<asp:Button ID="btnId" runat="server" Text="确 定" OnClick="btnId_Click" ValidationGroup="id" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtId1"
                    ControlToValidate="txtId2" ErrorMessage="结束id不能小于开始id" Operator="GreaterThanEqual"
                    ValidationGroup="id"></asp:CompareValidator></td>
	    </tr>
        <tr height="22">
            <td class="forumRowHighlight"></td>
        </tr>
    </table>
    </form>
</body>
</html>