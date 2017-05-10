<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Column.aspx.cs" Inherits="XkCms.WebForm.Manager.Column" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <table align="center" cellpadding="1" cellspacing="1" class="TableBorder" width="98%">
        <tr>
            <td align="left" class="forumRow" height="22" width="32%">
                当前位置：<asp:Label ID="lblChannel" runat="server" Text="Label"></asp:Label> --> 栏目管理
            </td>
            <td class="forumRow" align="center" width="20%">
                <a href="?ChannelId=<%=Channel.Id %>"><b>管理首页</b></a> | <a href="?action=add&ChannelId=<%=Channel.Id %>"><b>添加</b></a> |
                <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" OnClick="LinkButton1_Click">生成导航JS</asp:LinkButton></td>
            <td align="right" class="forumRow" width="32%"></td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plEdit" runat="server" Width="100%" HorizontalAlign="Center">
        <table border="0" cellpadding="5" cellspacing="0" class="tableBorder" width="98%">
            <tr>
                <th colspan="2">
                    添加栏目</th>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    栏目名称</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtName" runat="server" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    所属分类</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlColumn" runat="server">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    栏目简介</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtInfo" runat="server" Height="70px" MaxLength="200" TextMode="MultiLine"
                        Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    列表页模板</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlTemplate" runat="server">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    内容页模板</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlTemplate2" runat="server">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    每页记录数</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtPageSize" runat="server" Width="37px">20</asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPageSize"
                        Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    是否外部链接</td>
                <td style="text-align: left"><asp:RadioButtonList ID="rblIsOut" runat="server" EnableViewState="False"
                        RepeatColumns="2" OnSelectedIndexChanged="rblIsOut_SelectedIndexChanged" AutoPostBack="True">
                    <Items>
                        <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                        <asp:ListItem Value="1">是</asp:ListItem>
                    </Items>
                </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    外部连接</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtOuturl" runat="server" MaxLength="100" Width="225px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvOutUrl" runat="server" ControlToValidate="txtOuturl"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    允许发表评论</td>
                <td style="text-align: left">
                    <asp:RadioButtonList ID="rblIsReview" runat="server" EnableViewState="False"
                        RepeatColumns="2">
                        <Items>
                            <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </Items>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    允许会员投稿</td>
                <td style="text-align: left">
                    <asp:RadioButtonList ID="rblIsPost" runat="server" EnableViewState="False"
                        RepeatColumns="2">
                        <Items>
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </Items>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    允许在导航菜单显示</td>
                <td style="text-align: left">
                    <asp:RadioButtonList ID="rblTop" runat="server" EnableViewState="False"
                        RepeatColumns="2">
                        <Items>
                            <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </Items>
                    </asp:RadioButtonList></td>
            </tr>
            <tr class="tablefoot">
                <td style="width: 30%; text-align: right">
                    &nbsp;</td>
                <td style="text-align: left">
                    <asp:Button ID="btnSaveColumn" runat="server"
                        Text="保 存" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" CellPadding="5" GridLines="None" OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting" EmptyDataText="暂无栏目" CssClass="tableBorder" DataKeyNames="id" Width="98%">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" DataFormatString="[{0}]">
                    <itemstyle width="50px" HorizontalAlign="Right" ForeColor="DodgerBlue" />
                    <headerstyle width="50px"/>
                </asp:BoundField>
                <asp:TemplateField HeaderText="栏目名称">
                    <itemstyle horizontalalign="Left" />
                    <itemtemplate>
                        <%# chkOut(Eval("isout", "{0}"))%><%# GetColumnListName(Eval("title","{0}"),Eval("code","{0}")) %>       
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="排序">
                    <itemstyle horizontalalign="Center" width="100px" CssClass="forumRow" />
                    <headerstyle width="100px" />
                    <itemtemplate>
                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("id") %>'
                            OnCommand="upColumn">上升</asp:LinkButton>
                        | 
                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# Eval("id") %>'
                            OnCommand="downColumn">下降</asp:LinkButton>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <itemstyle width="100px" HorizontalAlign="Center" />
                    <headerstyle width="100px" HorizontalAlign="Center" />
                    <itemtemplate>
                        <a href='?action=add&ChannelId=<%# Eval("ChannelId") %>&id=<%#Eval("id") %>'>编辑</a>
                        <asp:LinkButton id="LinkButton6" runat="server" Text="删除" CausesValidation="False" CommandName="Delete">
                    </asp:LinkButton> 
</itemtemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    </form>
</body>
</html>