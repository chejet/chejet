<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CollHistory.aspx.cs" Inherits="XkCms.WebForm.Manager.CollHistory" %>

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
                当前位置：采集项目管理
            </td>
            <td class="forumRow" align="center" width="20%">
                <a href="?"><b>管理首页</b></a> | <a href="?action=add"><b>添加</b></a>
            </td>
            <td align="right" class="forumRow" width="40%" style="color: #ff0000">
                注意：审核之后记录将被移入相应栏目</td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:GridView ID="gvList" runat="server" Width="98%" AutoGenerateColumns="False" CellPadding="5" GridLines="None" CssClass="tableBorder" OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting" EmptyDataText="暂无数据">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID">
                    <ItemStyle HorizontalAlign="Right" Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="title" HeaderText="标题">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="itemname" HeaderText="项目">
                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                    <ItemTemplate>
                        <a href='CollHistory.aspx?action=add&id=<%# Eval("id","{0}") %>' target="_blank">查看</a>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="footer">
            <div class="pagerLeft">
                <asp:Button ID="btnMove" runat="server" Text="全部审核通过" OnClick="btnMove_Click" />&nbsp;<asp:Button
                    ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="清空所有记录" /></div>
        </div>
    </asp:Panel>
    <asp:Panel ID="plEdit" runat="server" HorizontalAlign="Center" Width="100%">
        <table border="0" cellpadding="5" cellspacing="0" class="tableBorder" width="98%">
            <tr>
                <th>
                    <asp:Label ID="lblTitle" runat="server"></asp:Label></th>
            </tr>
            <tr>
                <td align="left">
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal><br />
                    <asp:Literal ID="Literal2" runat="server"></asp:Literal></td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>