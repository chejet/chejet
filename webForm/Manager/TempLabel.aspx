<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempLabel.aspx.cs" Inherits="XkCms.WebForm.Manager.TempLabel" %>

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
                当前位置： 自定义标签管理
            </td>
            <td class="forumRow" width="20%">
                <a href="?"><b>管理首页</b></a> | <a href="?action=add"><b>添加</b></a>
            </td>
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
            OnRowDeleting="gvList_RowDeleting" Width="98%" EmptyDataText="暂无标签">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID">
                    <ItemStyle HorizontalAlign="Right" Width="40px" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="标签名称">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        {$MY_<%# Eval("Title") %>$}
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="sort" HeaderText="优先级">
                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="模板方案" DataField="pTitle" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="info" HeaderText="简介" />
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="160px" />
                    <HeaderStyle HorizontalAlign="Center" Width="160px" />
                    <ItemTemplate>
                        <a href='?action=add&id=<%# Eval("id","{0}") %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="plAdd" runat="server" HorizontalAlign="Center" Width="100%">
        <table border="0" cellpadding="5" cellspacing="0" class="tableBorder" width="98%">
            <tr>
                <th colspan="2">
                    添加标签</th>
            </tr>
            <tr>
                <td style="text-align: right">
                    标签名称</td>
                <td style="text-align: left">
                    {$MY_<asp:TextBox ID="txtName" runat="server" MaxLength="20" Width="141px"></asp:TextBox>$}
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtName"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    标签简介</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtInfo" runat="server" Height="50px" TextMode="MultiLine" Width="300px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    模板方案</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlTempProject" runat="server">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    优先级</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtSort" runat="server" Width="39px" ToolTip="数字越大，优先级越高。当标签中调用其他自定义标签时，就需要决定优先级。">0</asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtSort" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    标签文件</td>
                <td style="text-align: left">
                    <span>
                        <asp:TextBox ID="txtSource" runat="server" Width="150px" ToolTip="请把文件放在相应方案的目录下"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSource"
                            Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></span></td>
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