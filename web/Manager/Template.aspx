<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template.aspx.cs" Inherits="XkCms.WebForm.Manager.Template" %>

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
                当前位置： 模板管理<asp:DropDownList ID="ddlProject" runat="server" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                </asp:DropDownList></td>
            <td class="forumRow" width="20%">
                <asp:HyperLink ID="hlIndex" runat="server" Font-Bold="True" NavigateUrl="TempProject.aspx">管理首页</asp:HyperLink>
                |
                <asp:HyperLink ID="hlAdd" runat="server" Font-Bold="True">添加</asp:HyperLink></td>
            <td class="forumRow"  width="40%" align="right">
                <a href="#" onclick="javascript:window.open('../HTMLTagInfo.Txt','','scrollbars=yes,width=700,height=450')"><font
                        color="red"><u>模版标签说明</u></font></a>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" CellPadding="5"
            CssClass="tableBorder" GridLines="None" OnRowDataBound="gvList_RowDataBound"
            OnRowDeleting="gvList_RowDeleting" Width="98%" EmptyDataText="暂无模板">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID">
                    <ItemStyle HorizontalAlign="Right" Width="40px" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Title" HeaderText="模板名称" />
                <asp:BoundField DataField="source" HeaderText="模板文件" />
                <asp:TemplateField HeaderText="模板类型">
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                    <HeaderStyle HorizontalAlign="Center" Width="120px" />
                    <ItemTemplate>
                        <%# chkTempType(Eval("type","{0}"), Eval("stype","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="160px" />
                    <HeaderStyle HorizontalAlign="Center" Width="160px" />
                    <ItemTemplate>
                        <a href='?action=add&pid=<%= pId %>&id=<%# Eval("id","{0}") %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                        <%# isDefault(Eval("type","{0}"),Eval("stype","{0}"),Eval("isDefault","{0}"), Eval("id","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="plAdd" runat="server" HorizontalAlign="Center" Width="100%">
        <table border="0" cellpadding="5" cellspacing="0" class="tableBorder" width="98%">
            <tr>
                <th colspan="2">
                    添加模板</th>
            </tr>
            <tr>
                <td style="text-align: right">
                    模板名称</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtName" runat="server" MaxLength="20" Width="181px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtName"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    模板方案</td>
                <td style="text-align: left">
                    <asp:Label ID="lblProject" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align: right" width="30%">
                    模板类型</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="System">系统首页</asp:ListItem>
                        <asp:ListItem Value="Article">文章类</asp:ListItem>
                        <asp:ListItem Value="Soft">下载类</asp:ListItem>
                        <asp:ListItem Value="Photo">图片类</asp:ListItem>
                        <asp:ListItem Value="Diss">专题页</asp:ListItem>
                        <asp:ListItem Value="User">用户相关</asp:ListItem>
                        <asp:ListItem Value="Other">其它页</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlSType" runat="server">
                        <asp:ListItem Selected="True" Value="Index">系统首页</asp:ListItem>
                        <asp:ListItem Value="Channel">频道首页</asp:ListItem>
                        <asp:ListItem Value="Column">列表页</asp:ListItem>
                        <asp:ListItem Value="Content">内容页</asp:ListItem>
                        <asp:ListItem Value="MoreDiss">过往专题</asp:ListItem>
                        <asp:ListItem Value="DissList">专题显示</asp:ListItem>
                        <asp:ListItem Value="RegLogin">注册登陆</asp:ListItem>
                        <asp:ListItem Value="UserBox">用户中心</asp:ListItem>
                        <asp:ListItem Value="Friend">友情链接</asp:ListItem>
                        <asp:ListItem Value="Placard">站内公告</asp:ListItem>
                        <asp:ListItem Value="Other">其它信息</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="text-align: right" width="30%">
                    模板文件</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtSource" runat="server" Width="181px" ToolTip="请把文件放相应的方案目录下。"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSource"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr class="tablefoot">
                <td style="text-align: right">
                </td>
                <td style="text-align: left">
                    <asp:Button ID="btnAdd" runat="server" Text="保存" /></td>
            </tr>
         </table>
    </asp:Panel>
    </form>
</body>
</html>