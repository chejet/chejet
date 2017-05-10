<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Channel.aspx.cs" Inherits="XkCms.WebForm.Manager.Channel" %>

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
                当前位置：<asp:Label ID="lblChannel" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="forumRow" width="20%">
                <a href="?"><b>管理首页</b></a> | <a href="?action=add"><b>添加</b></a>
            </td>
            <td class="forumRow"  width="40%" align="right">
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" CellPadding="5" GridLines="None" EmptyDataText="暂无频道" CssClass="tableBorder" DataKeyNames="id" Width="98%" OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" DataFormatString="[{0}]">
                    <itemstyle width="50px" HorizontalAlign="Right" ForeColor="DodgerBlue" />
                    <headerstyle width="50px"/>
                </asp:BoundField>
                <asp:HyperLinkField HeaderText="频道名称" DataNavigateUrlFields="id" DataNavigateUrlFormatString="?action=add&amp;id={0}" DataTextField="title" >
                    <ItemStyle HorizontalAlign="Center" />
                </asp:HyperLinkField>
                <asp:TemplateField HeaderText="打开方式">
                    <ItemStyle HorizontalAlign="Center" Width="56px" />
                    <HeaderStyle Width="56px" />
                    <ItemTemplate>
                        <%# ChkTarget(Eval("Target","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="频道类型">
                    <ItemStyle HorizontalAlign="Center" Width="56px" />
                    <HeaderStyle Width="56px" />
                    <ItemTemplate>
                        <%# ChkOut(Eval("isOut","{0}"),"0",Eval("dir","{0}"),Eval("outUrl","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="频道路径/链接地址">
                    <ItemStyle HorizontalAlign="Left" Width="120px" />
                    <HeaderStyle Width="120px" />
                    <ItemTemplate>
                        <%# ChkOut(Eval("isOut","{0}"),"1",Eval("dir","{0}"),Eval("outUrl","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="项目名称" DataField="ItemName">
                    <ItemStyle Width="56px" HorizontalAlign="Center" />
                    <HeaderStyle Width="56px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="功能模块">
                    <ItemStyle HorizontalAlign="Center" Width="56px" />
                    <HeaderStyle Width="56px" />
                    <ItemTemplate>
                        <%# ChkType(Eval("Type","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="排序">
                    <itemstyle horizontalalign="Center" width="70px" CssClass="forumRow" />
                    <headerstyle width="70px" />
                    <itemtemplate>
                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="upChannel">上升</asp:LinkButton>
                        | 
                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="downChannel">下降</asp:LinkButton>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <itemstyle width="140px" HorizontalAlign="Center" />
                    <headerstyle width="140px" />
                    <itemtemplate>
                        <a href='?action=add&ChannelId=<%#Eval("id") %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("id") %>'>更新数据</asp:LinkButton>
                        <asp:LinkButton id="LinkButton6" runat="server" Text="删除" CausesValidation="False" CommandName="Delete">
                    </asp:LinkButton> 
                        <asp:LinkButton ID="LinkButton4" runat="server" Text='<%# ChkEnabled(Eval("Enabled","{0}")) %>' CommandArgument='<%# Eval("id") %>' OnCommand="doEnabled"></asp:LinkButton>
</itemtemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
        <asp:Panel ID="plEdit" runat="server" HorizontalAlign="Center" Width="100%">
            <table border="0" cellpadding="5" cellspacing="0" class="tableBorder" width="98%">
                <tr>
                    <th colspan="2">
                        添加频道</th>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        栏目频道</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtName" runat="server" MaxLength="20"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtName"
                            ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        频道简介</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtInfo" runat="server" Height="70px" MaxLength="200" TextMode="MultiLine"
                            Width="225px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        频道类型</td>
                    <td style="text-align: left">
                        <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                            <asp:ListItem Selected="True" Value="0">内部频道</asp:ListItem>
                            <asp:ListItem Value="1">外部频道</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        功能模块</td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" ToolTip="请慎重选择，添加之后不能修改">
                            <asp:ListItem Value="Article">文章</asp:ListItem>
                            <asp:ListItem Value="Soft">下载</asp:ListItem>
                            <asp:ListItem Value="Photo">图片</asp:ListItem>
                        </asp:DropDownList>
                        </td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        频道目录</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtDir" runat="server" MaxLength="20" ToolTip="添加之后不能修改,必须只含英文字母"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDir" runat="server" ControlToValidate="txtDir"
                            ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        项目名称</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtItemName" runat="server" MaxLength="20"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvItemName" runat="server" ControlToValidate="txtItemName"
                            ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        项目单位</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtItemUnit" runat="server" MaxLength="8" Width="60px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvItemUnit" runat="server" ControlToValidate="txtItemUnit"
                            ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        频道首页模板</td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="ddlTemplate" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        过往专题页模板</td>
                    <td style="text-align: left"><asp:DropDownList ID="ddlTemplate2" runat="server">
                    </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        过往专题页大小</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtDissPageSize" runat="server" MaxLength="3" Width="60px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDissPageSize"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RangeValidator
                                ID="RangeValidator1" runat="server" ControlToValidate="txtDissPageSize" Display="Dynamic"
                                ErrorMessage="*" MaximumValue="100" MinimumValue="1" Type="Integer"></asp:RangeValidator></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        允许用户投稿</td>
                    <td style="text-align: left">
                        <asp:RadioButtonList ID="rblIsPost" runat="server" EnableViewState="False" RepeatColumns="2">
                            <asp:ListItem Selected="True" Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        允许用户评论</td>
                    <td style="text-align: left">
                        <asp:RadioButtonList ID="rblIsReview" runat="server" EnableViewState="False" RepeatColumns="2">
                            <asp:ListItem Selected="True" Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        外部连接</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtOuturl" runat="server" MaxLength="100" Width="225px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOutUrl" runat="server" ControlToValidate="txtOuturl"
                            ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        打开方式</td>
                    <td style="text-align: left">
                        <asp:RadioButtonList ID="rblTarget" runat="server" EnableViewState="False" RepeatColumns="2">
                            <asp:ListItem Selected="True" Value="0">原窗口</asp:ListItem>
                            <asp:ListItem Value="1">新窗口</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        是否启用</td>
                    <td style="text-align: left">
                        <asp:RadioButtonList ID="rblEnabled" runat="server" EnableViewState="False" RepeatColumns="2">
                            <asp:ListItem Selected="True" Value="1">启用</asp:ListItem>
                            <asp:ListItem Value="0">禁用</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr class="tablefoot">
                    <td style="width: 20%; text-align: right">
                        &nbsp;</td>
                    <td style="text-align: left">
                        <asp:Button ID="btnSave" runat="server" Text="保 存" /></td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>