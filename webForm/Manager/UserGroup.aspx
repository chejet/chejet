<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserGroup.aspx.cs" Inherits="XkCms.WebForm.Manager.UserGroup" %>

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
                <span style="color:Red" title="①系统默认用户组不能删除和编辑用户等级。<br>②删除用户组时会将用户移动普通用户组中。">注意事项</span></td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:GridView ID="gvList" runat="server" CellPadding="5" AutoGenerateColumns="False" GridLines="None" EmptyDataText="没有用户组" CssClass="tableBorder" Width="98%" OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting">
            <Columns>
                <asp:BoundField DataField="groupname" HeaderText="组名">
                    <ItemStyle Width="30%" HorizontalAlign="Center" />
                    <HeaderStyle Width="30%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="usertotal" HeaderText="用户数量">
                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                    <HeaderStyle Width="20%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="用户等级" DataField="Grades">
                    <ItemStyle HorizontalAlign="Center" Width="20%" />
                    <HeaderStyle HorizontalAlign="Center" Width="20%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="30%" />
                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                    <ItemTemplate>
                        <a href='?action=add&id=<%#Eval("id") %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="plAdd" runat="server" Width="100%" HorizontalAlign="Center">
        <table align="center" cellpadding="5" cellspacing="0" width="98%" class="tableBorder">
            <tr>
                <th colspan="2">
                    用户组设置</th>
            </tr>
            <tr>
                <td width="40%" align=right class=forumRowHighlight>
                    用户组名称</td>
                <td width="60%" class=forumRowHighlight align=left>
                    <asp:TextBox ID="txtName" runat="server" ToolTip="<=20 Chars" Width="150px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align=right>
                    用户组等级</td>
                <td align=left>
                    <asp:TextBox ID="txtGrades" runat="server" ToolTip="数字越大级别越高, 大于4小于998" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtGrades"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtGrades"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator><asp:RangeValidator
                                ID="RangeValidator1" runat="server" ControlToValidate="txtGrades" Display="Dynamic"
                                ErrorMessage="*" MaximumValue="998" MinimumValue="4" ToolTip="应该大于4并小于998" Type="Integer"></asp:RangeValidator></td>
            </tr>
            <tr>
                <th colspan="2">
                    用户基本使用设置</th>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    用户是否可以修改密码</td>
                <td class=forumRowHighlight align=left>
                    <asp:RadioButtonList ID="GroupSet0" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right>
                    用户是否可以修改资料</td>
                <td align=left>
                    <asp:RadioButtonList ID="GroupSet1" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    发布内容信息是否使用验证码</td>
                <td class=forumRowHighlight align=left>
                    <asp:RadioButtonList ID="GroupSet2" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right>
                    是否可以使用收藏夹</td>
                <td align=left>
                    <asp:RadioButtonList ID="GroupSet3" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    是否可以添加好友</td>
                <td class=forumRowHighlight align=left>
                    <asp:RadioButtonList ID="GroupSet4" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right>
                    最多收藏多少条信息</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet5" runat="server" ToolTip="不限制请设置为0" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="GroupSet5"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator2" runat="server" ControlToValidate="GroupSet5"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    最多添加多少好友</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet6" runat="server" ToolTip="不限制请设置为0" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="GroupSet6"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="GroupSet6"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <th colspan="2">
                    发布权限设置</th>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    可以发布文章</td>
                <td class=forumRowHighlight align=left>
                    <asp:RadioButtonList ID="GroupSet7" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right>
                    可以管理自己发布的文章</td>
                <td align=left>
                    <asp:RadioButtonList ID="GroupSet8" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    发布文章增加的点数</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet9" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="GroupSet9"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator4" runat="server" ControlToValidate="GroupSet9"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right>
                    每天可以发布多少篇文章</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet10" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="GroupSet10"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator5" runat="server" ControlToValidate="GroupSet10"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    可以发布软件</td>
                <td class=forumRowHighlight align=left>
                    <asp:RadioButtonList ID="GroupSet11" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right>
                    可以管理自己发布的软件</td>
                <td align=left>
                    <asp:RadioButtonList ID="GroupSet12" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    发布软件增加的点数</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet13" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="GroupSet13"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator6" runat="server" ControlToValidate="GroupSet13"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right>
                    每天可以发布多少个软件</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet14" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="GroupSet14"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator7" runat="server" ControlToValidate="GroupSet14"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    可以发布图片</td>
                <td class=forumRowHighlight align=left>
                    <asp:RadioButtonList ID="GroupSet35" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right>
                    可以管理自己发布的图片</td>
                <td align=left>
                    <asp:RadioButtonList ID="GroupSet36" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    发布图片增加的点数</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet37" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="GroupSet37"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator8" runat="server" ControlToValidate="GroupSet37"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right>
                    每天可以发布多少个图片</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet38" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="GroupSet38"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator9" runat="server" ControlToValidate="GroupSet38"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    发布以上信息需要管理员审核</td>
                <td class=forumRowHighlight align=left><asp:RadioButtonList ID="GroupSet15" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1">是</asp:ListItem>
                    <asp:ListItem Value="0">否</asp:ListItem>
                </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right>
                    发布文章内容的最多字节</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet16" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    byte<asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="GroupSet16"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator23" runat="server" ControlToValidate="GroupSet16"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    发布软件简介的最多字节</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet17" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    byte<asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="GroupSet17"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator24" runat="server" ControlToValidate="GroupSet17"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right>
                    发布图片简介的最多字节</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet39" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    byte<asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="GroupSet39"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator25" runat="server" ControlToValidate="GroupSet39"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    删除文章扣除的点数</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet18" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="GroupSet18"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator10" runat="server" ControlToValidate="GroupSet18"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right>
                    删除软件扣除的点数</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet19" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="GroupSet19"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator11" runat="server" ControlToValidate="GroupSet19"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    删除图片扣除的点数</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet40" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="GroupSet40"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator12" runat="server" ControlToValidate="GroupSet40"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right>
                    是否可以上传文件</td>
                <td align=left>
                    <asp:RadioButtonList ID="GroupSet20" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    每天可以上传文件数</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet21" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="GroupSet21"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator13" runat="server" ControlToValidate="GroupSet21"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <th colspan="2">
                    站内短信设置</th>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    是否可以发送短信</td>
                <td class=forumRowHighlight align=left>
                    <asp:RadioButtonList ID="GroupSet22" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right>
                    发送短信内容限制</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet23" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    byte<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="GroupSet23"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator14" runat="server" ControlToValidate="GroupSet23"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    信箱大小限制 </td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet24" runat="server" ToolTip="不限制请设置为0" Width="80px"></asp:TextBox>
                    条<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="GroupSet24"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator15" runat="server" ControlToValidate="GroupSet24"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right>
                    每天可以发送多少条短信</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet29" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="GroupSet29"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator16" runat="server" ControlToValidate="GroupSet29"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <th colspan="2">
                    其它设置</th>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    每次登陆增加的点数</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet25" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="GroupSet25"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator17" runat="server" ControlToValidate="GroupSet25"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right>
                    每次登陆增加经验值</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet32" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="GroupSet32"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator18" runat="server" ControlToValidate="GroupSet32"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    每次登陆增加的魅力值</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet33" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="GroupSet33"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator19" runat="server" ControlToValidate="GroupSet33"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right>
                    发布信息增加的点数</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet26" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="GroupSet26"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator20" runat="server" ControlToValidate="GroupSet26"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    回复留言增加的点数</td>
                <td class=forumRowHighlight align=left>
                    <asp:TextBox ID="GroupSet27" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="GroupSet27"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator21" runat="server" ControlToValidate="GroupSet27"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right>
                    允许申请友情链接数量</td>
                <td align=left>
                    <asp:TextBox ID="GroupSet28" runat="server" ToolTip="数字" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="GroupSet28"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator22" runat="server" ControlToValidate="GroupSet28"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为数字"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    是否可以申请友情链接</td>
                <td class=forumRowHighlight align=left>
                    <asp:RadioButtonList ID="GroupSet30" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right>
                    是否可以下载软件</td>
                <td align=left>
                    <asp:RadioButtonList ID="GroupSet31" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=right class=forumRowHighlight>
                    下载软件是否直接显示下载地址</td>
                <td class=forumRowHighlight align=left>
                    <asp:RadioButtonList ID="GroupSet34" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="0">否</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align=left class="tablefoot">
                </td>
                <td align=left class="tablefoot">
                    <asp:Button ID="btnSave" runat="server" Text="保 存" OnClick="btnSave_Click" /></td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
