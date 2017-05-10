<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="XkCms.WebForm.Manager.Users" %>

<%@ Register Assembly="XkCms.Common" Namespace="XkCms.Common.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <table align="center" cellpadding="1" cellspacing="1" class="TableBorder" width="98%">
        <tr>
            <td align="left" class="forumRow" height="22" width="40%">
                当前位置：用户组管理
            </td>
            <td class="forumRow" align="center" width="20%">
                <a href="?"><b>管理首页</b></a> | <a href="?action=add"><b>添加</b></a>
            </td>
            <td align="right" class="forumRow" width="40%">
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:GridView ID="gvList" runat="server" CellPadding="5" AutoGenerateColumns="False" GridLines="None" EmptyDataText="没有用户" CssClass="tableBorder" Width="98%" OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting">
            <Columns>
                <asp:TemplateField HeaderText="选">
                    <ItemStyle Width="10px" />
                    <HeaderStyle Width="10px" />
                    <ItemTemplate>
                        <input id="chkUserId" class="checkbox" name="chkUserId" type="checkbox" value='<%#Eval("id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="username" HeaderText="用户名">
                    <ItemStyle Width="20%" HorizontalAlign="Center" Wrap="False" />
                    <HeaderStyle Width="20%" HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="usergroup" HeaderText="用户组">
                    <ItemStyle Width="10%" HorizontalAlign="Center"  Wrap="False" />
                    <HeaderStyle Width="10%" HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="性别" DataField="usersex">
                    <ItemStyle HorizontalAlign="Center" Width="5%" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" Width="5%" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="JoinTime" HeaderText="注册时间">
                    <ItemStyle HorizontalAlign="Center" Width="17%" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" Width="17%" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="LastTime" HeaderText="最后登陆">
                    <ItemStyle HorizontalAlign="Center" Width="17%" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" Width="17%" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="userlogin" HeaderText="登陆次数">
                    <ItemStyle HorizontalAlign="Center" Width="8%" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" Width="8%" Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="状态">
                    <ItemStyle HorizontalAlign="Center" Width="5%" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" Width="5%" Wrap="False" />
                    <ItemTemplate>
                        <%# ChkState(Eval("ispass","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemTemplate>
                        <a href='?action=add&id=<%#Eval("id") %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table align="center" cellpadding="0" cellspacing="0" class="TableFooter" width="98%">
            <tr>
                <td style="text-align:left">
                    <input name="chkall" onclick="CheckAll(this.form)" type="checkbox" />全选
                    <asp:DropDownList ID="ddlOper" runat="server">
                        <asp:ListItem Value="pass">审核通过</asp:ListItem>
                        <asp:ListItem Value="nopass">审核未通过</asp:ListItem>
                        <asp:ListItem Value="move">批量移动到-=&gt;</asp:ListItem>
                        <asp:ListItem Value="del">直接删除</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlGroup" runat="server">
                    </asp:DropDownList>
                    <asp:Button ID="btnExecute" runat="server" OnClick="btnExecute_Click" Text="执 行" />
                </td>
                <td>
                 <cc1:aspnetpager id="AspNetPager1" runat="server" OnPageChanged="AspNetPager1_PageChanged"></cc1:aspnetpager></td>
            </tr>
            </table>
            <br />
            <table align="center" border="0" cellpadding="0" cellspacing="0" style="border-right: #6bba21 1px solid;
                border-top: #6bba21 1px solid; border-left: #6bba21 1px solid; border-bottom: #6bba21 1px solid"
                width="98%">
                <tr align="center" bgcolor="#f7fbff" height="28" valign="middle">
                    <td>
                        快速搜索用户：<asp:DropDownList ID="ddlKeyType" runat="server">
                            <asp:ListItem Value="username">用户名称</asp:ListItem>
                            <asp:ListItem Value="nickname">用户昵称</asp:ListItem>
                            <asp:ListItem Value="TrueName">简介</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlKeyGroup" runat="server">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtKeyWord" runat="server" Width="200px"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="搜 索" /></td>
                </tr>
            </table>
    </asp:Panel>
    <asp:Panel ID="plEditBox" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:Panel ID="plAdd" runat="server" Width="100%" HorizontalAlign="Center">
            <table align="center" cellpadding="5" cellspacing="0" width="98%" class="tableBorder">
                <tr>
	                <th colspan="2">添加会员</th>
                </tr>
                <tr>
	                <td width='40%' align=right>
                        登陆名称</td>
	                <td width='60%' align=left>
                        <asp:TextBox ID="txtAddName" runat="server" Width="140px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddName"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
	                <td align=right>
                        用户密码</td>
	                <td align=left>
                        <asp:TextBox ID="txtAddPass" runat="server" MaxLength="20" TextMode="Password" ToolTip="6到20个字符"
                            Width="140px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAddPass"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAddPass"
                                ErrorMessage="*" ToolTip="6到20个字符" ValidationExpression="^.{6,20}$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
	                <td align=right>
                        确认密码</td>
	                <td align=left>
                        <asp:TextBox ID="txtAddPass2" runat="server" TextMode="Password" Width="140px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAddPass2"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:CompareValidator
                                ID="CompareValidator1" runat="server" ControlToCompare="txtAddPass" ControlToValidate="txtAddPass2"
                                Display="Dynamic" ErrorMessage="*" ToolTip="两次密码不一致"></asp:CompareValidator></td>
                </tr>
                <tr>
	                <td align=right>
                        用户昵称</td>
	                <td align=left>
                        <asp:TextBox ID="txtAddNickName" runat="server" Width="140px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtAddNickName"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
	                <td align=right>
                        用户邮箱</td>
	                <td align=left>
                        <asp:TextBox ID="txtAddEmail" runat="server" Width="140px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAddEmail"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
	                <td align=right>
                        用户性别</td>
	                <td align=left>
                        <asp:RadioButtonList ID="rblAddSex" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True">男</asp:ListItem>
                            <asp:ListItem>女</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
	                <td align=right>
                        所属用户组</td>
	                <td align=left>
                        <asp:DropDownList ID="ddlAddGroup" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
	                <td align=right>
                        用户点数</td>
	                <td align=left>
                        <asp:TextBox ID="txtAddPoint" runat="server" Width="60px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtAddPoint"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                ID="RegularExpressionValidator25" runat="server" ControlToValidate="txtAddPoint"
                                Display="Dynamic" ErrorMessage="*" ToolTip="必须为整数" ValidationExpression="^\d*$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr align=center>
	                <td colspan=2 class="tablefoot">
                        <asp:Button ID="btnAddUser" runat="server" OnClick="btnAddUser_Click" Text="添加会员" /></td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="plEdit" runat="server" Width="100%" HorizontalAlign="Center">
            <table align="center" cellpadding="5" cellspacing="0" width="98%" class="tableBorder">
                <tr>
	                <th colspan="4">编辑会员</th>
                </tr>
                <tr>
	                <td width='10%' align="right">会员名称</td>
	                <td width='40%' align="left">
                        <asp:Label ID="lblEditName" runat="server" Text="Label"></asp:Label></td>
	                <td width='10%' align="right">真实姓名</td>
	                <td width='40%' align="left">
                        <asp:TextBox ID="txtEditTrueName" runat="server" Width="140px" MaxLength="4"></asp:TextBox></td>
                </tr>
                <tr>
	                <td align="right">用户密码</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditPass" runat="server" MaxLength="20" TextMode="Password" ToolTip="如果不修改密码请留空"
                            Width="140px"></asp:TextBox>
                        <asp:RegularExpressionValidator
                                ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtEditPass"
                                ErrorMessage="*" ToolTip="6到20个字符" ValidationExpression="^.{6,20}$"></asp:RegularExpressionValidator></td>
	                <td align="right">用户邮箱</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditMail" runat="server" Width="140px" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtEditMail"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                ID="RegularExpressionValidator11" runat="server" ControlToValidate="txtEditMail"
                                Display="Dynamic" ErrorMessage="*" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="邮箱格式错误"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
	                <td align="right">
                        确认密码</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditPass2" runat="server" TextMode="Password" Width="140px" ToolTip="如果不修改密码请留空"></asp:TextBox>
                        <asp:CompareValidator
                                ID="CompareValidator2" runat="server" ControlToCompare="txtEditPass2" ControlToValidate="txtEditPass"
                                Display="Dynamic" ErrorMessage="*" ToolTip="两次密码不一致"></asp:CompareValidator><asp:CompareValidator
                                    ID="CompareValidator3" runat="server" ControlToCompare="txtEditPass" ControlToValidate="txtEditPass2"
                                    Display="Dynamic" ErrorMessage="*" ToolTip="两次密码不一致"></asp:CompareValidator></td>
	                <td align="right">用户状态</td>
	                <td align="left"><asp:RadioButtonList ID="rblEditIsPass" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="1">已审</asp:ListItem>
                        <asp:ListItem Value="0">未审</asp:ListItem>
                    </asp:RadioButtonList></td>
                </tr>
                <tr>
	                <td align="right">用户等级</td>
	                <td align="left"><asp:DropDownList ID="ddlEditGroup" runat="server">
                    </asp:DropDownList></td>
	                <td align="right">
                        用户昵称</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditNickName" runat="server" Width="140px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtEditNickName"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
	                <td align="right">用户点数</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditPoint" runat="server" Width="60px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtEditPoint"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEditPoint"
                                Display="Dynamic" ErrorMessage="*" ToolTip="必须为整数" ValidationExpression="^\d*$"></asp:RegularExpressionValidator></td>
	                <td align="right">用户魅力值</td>
	                <td align="left"> 
                        <asp:TextBox ID="txtEditCharm" runat="server" Width="60px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtEditCharm"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtEditCharm"
                                Display="Dynamic" ErrorMessage="*" ToolTip="必须为整数" ValidationExpression="^\d*$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
	                <td nowrap align="right">用户经验值</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditExperience" runat="server" Width="60px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtEditExperience"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEditExperience"
                                Display="Dynamic" ErrorMessage="*" ToolTip="必须为整数" ValidationExpression="^\d*$"></asp:RegularExpressionValidator></td>
	                <td nowrap align="right">用户图像</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditFace" runat="server" Width="140px"></asp:TextBox></td>
                </tr>
                <tr>
	                <td align="right">身分证号码</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditUserIDCard" runat="server" Width="140px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtEditUserIDCard"
                            Display="Dynamic" ErrorMessage="*" ToolTip="身份证格式错误" ValidationExpression="\d{17}[\d|X]|\d{15}"></asp:RegularExpressionValidator></td>
	                <td align="right">
                        性别</td>
	                <td align="left"><asp:RadioButtonList ID="rblEditSex" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">男</asp:ListItem>
                        <asp:ListItem>女</asp:ListItem>
                    </asp:RadioButtonList></td>
                </tr>
                <tr>
	                <td align="right">用户电话</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditPhone" runat="server" Width="140px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtEditPhone"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^(\(?\d{3,4}\)?)?[\s\-]?\d{7,8}$"></asp:RegularExpressionValidator></td>
	                <td align="right">用户QQ</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditOicq" runat="server" Width="100px"></asp:TextBox>
                        <asp:RegularExpressionValidator
                                ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtEditOicq"
                                Display="Dynamic" ErrorMessage="*" ToolTip="必须为整数" ValidationExpression="^\d*$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
	                <td align="right">邮政编码</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditPostCode" runat="server" Width="140px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                            ControlToValidate="txtEditPostCode" Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{6}"></asp:RegularExpressionValidator></td>
	                <td align="right">联系地址</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditAddress" runat="server" Width="140px"></asp:TextBox></td>
                </tr>
                <tr>
	                <td align="right">密码问题</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditQuestion" runat="server" Width="140px"></asp:TextBox></td>
	                <td align="right">密码答案</td>
	                <td align="left">
                        <asp:TextBox ID="txtEditAnswer" runat="server" Width="140px" ToolTip="如果不修改密码请留空"></asp:TextBox></td>
                </tr>
                <tr>
	                <td nowrap align="right">最后登陆时间</td>
	                <td align="left">
                        <asp:Label ID="lblEditLastTime" runat="server" Text=""></asp:Label></td>
	                <td align="right">最后登陆IP</td>
	                <td align="left">
                        <asp:Label ID="lblEditUserLastIp" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
	                <td align="right">注册时间</td>
	                <td align="left">
                        <asp:Label ID="lblEditJoinTime" runat="server" Text=""></asp:Label></td>
	                <td align="right">登陆次数</td>
	                <td align="left">
                        <asp:Label ID="lblEditUserlogin" runat="server" Text="0"></asp:Label></td>
                </tr>
                <tr align=center>
	                <td colspan=4 class="tableFoot">
                        <asp:Button ID="btnSaveUser" runat="server" OnClick="btnSaveUser_Click" Text="保存用户" /></td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    </form>
</body>
</html>
