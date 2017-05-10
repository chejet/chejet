<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Friend.aspx.cs" Inherits="XkCms.WebForm.Manager.Friend" %>

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
                当前位置：<asp:Label ID="lblChannel" runat="server" Text="Label"></asp:Label> --> 友情链接管理
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
            Width="98%" GridLines="None" OnRowDeleting="gvList_RowDeleting" OnRowDataBound="gvList_RowDataBound" EmptyDataText="暂无链接">
            <Columns>
                <asp:BoundField HeaderText="排序" DataField="ordernum">
                    <ItemStyle Width="40px" HorizontalAlign="Center" />
                    <HeaderStyle Width="40px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField HeaderText="名称" DataField="linkname">
                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                    <HeaderStyle Width="150px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:HyperLinkField HeaderText="连接地址" DataTextField="linkurl" Target="_blank">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:HyperLinkField>
                <asp:TemplateField HeaderText="状态">
                    <ItemStyle Width="40px" HorizontalAlign="Center" />
                    <HeaderStyle Width="40px" />
                    <ItemTemplate>
                        <%# ChkState(Eval("ispass","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
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
              <th colspan="2" height="22">链接管理</th>
            </tr>
            <tr bgcolor="#FFFFFF" valign="middle"> 
              <td width="20%" align="right">网站名称&nbsp;
              </td>
              <td align="left">
                  <asp:TextBox ID="txtName" runat="server" MaxLength="20"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                      Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr bgcolor="#FFFFFF" valign="middle"> 
              <td width="20%" align="right">网站图片地址&nbsp;
              </td>
              <td align="left">
                  <asp:TextBox ID="txtImg" runat="server" Width="280px">http://www.xkzi.com/images/smalllogo.gif</asp:TextBox>
                  </td>
            </tr>
            <tr bgcolor="#FFFFFF" valign="middle"> 
              <td width="20%" align="right">网站连接地址&nbsp;
              </td>
              <td align="left">
                  <asp:TextBox ID="txtUrl" runat="server" Width="280px"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUrl"
                      Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr bgcolor="#FFFFFF" valign="middle"> 
              <td width="20%" align="right">网站简介&nbsp;
              </td>
              <td align="left">
                  <asp:TextBox ID="txtInfo" runat="server" Height="60px" TextMode="MultiLine" Width="280px"></asp:TextBox></td>
            </tr>
            <tr bgcolor="#FFFFFF" valign="middle"> 
              <td width="20%" align="right">排序号&nbsp;
              </td>
              <td align="left">
                  <asp:TextBox ID="txtSort" runat="server" MaxLength="5" Width="40px">0</asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSort"
                      Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                          ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSort" Display="Dynamic"
                          ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr bgcolor="#FFFFFF" valign="middle"> 
              <td width="20%" align="right">状态&nbsp;
              </td>
              <td align="left">
                  <asp:RadioButtonList ID="rblStata" runat="server" RepeatDirection="Horizontal">
                      <asp:ListItem Value="1" Selected="True">通过</asp:ListItem>
                      <asp:ListItem Value="0">不通过</asp:ListItem>
                  </asp:RadioButtonList></td>
            </tr>
            <tr bgcolor="#FFFFFF" valign="middle"> 
              <td width="20%" align="right">显示风格&nbsp;
              </td>
              <td align="left">
                  <asp:RadioButtonList ID="rblStyle" runat="server" RepeatDirection="Horizontal">
                      <asp:ListItem Value="0" Selected="True">文本</asp:ListItem>
                      <asp:ListItem Value="1">图片</asp:ListItem>
                  </asp:RadioButtonList></td>
            </tr>
            <tr bgcolor="#ffffff" valign="middle">
                <td align="right" width="20%">
                    申请人&nbsp;</td>
                <td align="left">
                    <asp:TextBox ID="txtUserName" runat="server" Enabled="False" MaxLength="20"></asp:TextBox></td>
            </tr>
            <tr class="tablefoot"> 
              <td align="center" valign="middle" colspan="2">
                  <asp:Button ID="Button1" runat="server" Text="保 存" /></td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
