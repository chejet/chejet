<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempProject.aspx.cs" Inherits="XkCms.WebForm.Manager.TempProject" %>

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
                当前位置： 模板方案管理
            </td>
            <td class="forumRow" width="20%">
                <a href="?"><b>管理首页</b></a> | <a href="?action=add"><b>添加</b></a>
            </td>
            <td class="forumRow"  width="40%" align="right">
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
                <asp:TemplateField HeaderText="方案名称">
                    <ItemStyle HorizontalAlign="Center" Width="180px" />
                    <HeaderStyle HorizontalAlign="Center" Width="180px" />
                    <ItemTemplate>
                        <a href='Template.aspx?pid=<%# Eval("id","{0}") %>'><%# Eval("Title") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="简介" DataField="info" />
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="160px" />
                    <HeaderStyle HorizontalAlign="Center" Width="160px" />
                    <ItemTemplate>
                        <a href='?action=add&id=<%# Eval("id","{0}") %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                        <%# isDefault(Eval("isDefault","{0}"), Eval("id","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="plAdd" runat="server" HorizontalAlign="Center" Width="100%">
        <table border="0" cellpadding="5" cellspacing="0" class="tableBorder" width="98%">
            <tr>
                <th colspan="2">
                    添加模板方案</th>
            </tr>
            <tr>
                <td style="text-align: right">
                    方案名称</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtName" runat="server" MaxLength="20" Width="181px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtName"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    方案目录</td>
                <td style="text-align: left">
                    /Template/<asp:TextBox ID="txtDir" runat="server" Width="121px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDir"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    说明</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtInfo" runat="server" Height="65px" TextMode="MultiLine" Width="300px"></asp:TextBox></td>
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