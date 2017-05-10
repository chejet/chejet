﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentTemplate.aspx.cs" Inherits="XkCms.WebForm.Manager.ContentTemplate" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <table align="center" cellpadding="1" cellspacing="1" class="TableBorder" width="98%">
        <tr>
            <td align="left" class="forumRow" height="22" width="30%">
                当前位置：内容模板管理
            </td>
            <td class="forumRow" align="center" width="40%">
                <a href="?"><b>管理首页</b></a> | <a href="?action=add"><b>添加</b></a> |
                <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" OnClick="LinkButton1_Click">更新XML</asp:LinkButton></td>
            <td align="right" class="forumRow" width="30%" style="color: #ff0000">
                操作之后请更新XML</td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:GridView ID="gvList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            CssClass="TableBorder" EmptyDataText="暂无信息" GridLines="None" OnPageIndexChanging="gvList_PageIndexChanging"
            OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting" PageSize="15"
            Width="98%">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID">
                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="title" HeaderText="名称">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                    <HeaderStyle HorizontalAlign="Center" Width="120px" />
                    <ItemTemplate>
                        <a href='?action=add&id=<%# Eval("id") %>&ChannelId=<%=Channel.Id %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings FirstPageText="第一页" LastPageText="最末页" Mode="NextPreviousFirstLast"
                NextPageText="下一页" PreviousPageText="上一页" />
            <FooterStyle HorizontalAlign="Right" />
            <PagerStyle ForeColor="RoyalBlue" HorizontalAlign="Right" VerticalAlign="Bottom" />
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="plEdit" runat="server" Width="100%">
        <table align="center" border="0" cellpadding="5" cellspacing="0" class="TableBorder"
            width="98%">
            <tr align="center" valign="middle">
                <th colspan="2" height="22">
                    内容模板管理</th>
            </tr>
            <tr bgcolor="#ffffff" valign="middle">
                <td align="right" width="20%">
                    模板名称
                </td>
                <td align="left">
                    <asp:TextBox ID="txtName" runat="server" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr bgcolor="#ffffff" valign="middle">
                <td align="right" width="20%">
                    模板描述
                </td>
                <td align="left">
                    <asp:TextBox ID="txtDescription" runat="server" Height="50px" MaxLength="200" TextMode="MultiLine"
                        Width="520px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDescription"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr bgcolor="#ffffff" valign="middle">
                <td align="right" width="20%">
                    模板内容&nbsp;</td>
                <td align="left">
                    <fckeditorv2:fckeditor id="FCKeditor1" runat="server" height="220px"
                        width="520px">&nbsp;</fckeditorv2:fckeditor>
                </td>
            </tr>
            <tr class="tablefoot">
                <td></td>
                <td align="left" valign="middle">
                    <asp:Button ID="Button1" runat="server" Text="保 存" /></td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>