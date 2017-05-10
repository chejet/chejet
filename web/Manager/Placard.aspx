<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Placard.aspx.cs" Inherits="XkCms.WebForm.Manager.Placard" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>

<%@ Register Assembly="XkCms.Common" Namespace="XkCms.Common.Web" TagPrefix="cc1" %>

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
                当前位置：<asp:Label ID="lblChannel" runat="server" Text="Label"></asp:Label> --> 公告管理
            </td>
            <td class="forumRow" align="center" width="20%">
                <a href="?ChannelId=<%=Channel.Id %>"><b>管理首页</b></a> | <a href="?action=add&ChannelId=<%=Channel.Id %>"><b>添加</b></a>
            </td>
            <td align="right" class="forumRow" width="40%"></td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" CssClass="TableBorder"
            Width="98%" GridLines="None" PageSize="15" OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting" EmptyDataText="暂无内容">
            <Columns>
                <asp:BoundField HeaderText="ID" DataField="id">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="标题" DataField="title">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                    <HeaderStyle HorizontalAlign="Center" Width="120px" />
                    <ItemTemplate>
                        <a href='?action=add&id=<%# Eval("id") %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table align="center" cellpadding="1" cellspacing="1" class="TableFooter" width="98%">
            <tr>
                <td>
                    <cc1:AspNetPager ID="AspNetPager1" runat="server" OnPageChanged="AspNetPager1_PageChanged">
                    </cc1:AspNetPager>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="plEdit" runat="server" Width="100%">
    <table width="98%" border="0" cellpadding="5" cellspacing="0" class=TableBorder align="center">
        <tr align="center" valign="middle"> 
          <th colspan="2" height="22">公告管理</th>
        </tr>
        <tr bgcolor="#ffffff" valign="middle"> 
          <td width="20%" align="right">标题</td>
          <td align="left"><asp:TextBox ID="txtTitle" runat="server" MaxLength="100" Width="280px"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle"
                  ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
        </tr>
	    <tr bgcolor="#ffffff" valign="middle"> 
          <td width="20%" align="right">内容</td>
          <td align="left">
              <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" BasePath="" Height="220px"
                  Width="520px" ManagerPath="">
              </FCKeditorV2:FCKeditor>
          </td>
        </tr>
        <tr class="tablefoot"> 
          <td align="center" valign="middle" colspan="2">
              <asp:Button ID="Button1" runat="server" Text="保 存" />
          </td>
        </tr>
    </table>
    </asp:Panel>
    </form>
</body>
</html>
