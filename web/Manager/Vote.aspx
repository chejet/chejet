<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vote.aspx.cs" Inherits="XkCms.WebForm.Manager.Vote" %>

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
                当前位置：<asp:Label ID="lblChannel" runat="server" Text="Label"></asp:Label> --> 投票管理
            </td>
            <td class="forumRow" align="center" width="20%">
                <a href="?ChannelId=<%=Channel.Id %>"><b>管理首页</b></a> | <a href="?action=add&ChannelId=<%=Channel.Id %>"><b>添加</b></a>
            </td>
            <td align="right" class="forumRow" width="40%"></td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" HorizontalAlign="Center" Width="100%">
    <asp:GridView id="gvList" runat="server" Width="98%" OnRowDeleting="gvList_RowDeleting" OnPageIndexChanging="gvList_PageIndexChanging" AllowPaging="True" OnRowDataBound="gvList_RowDataBound" PageSize="15" GridLines="None" CssClass="TableBorder" AutoGenerateColumns="False" EmptyDataText="暂无内容">
        <Columns>
            <asp:BoundField HeaderText="ID" DataField="id">
                <ItemStyle Width="60px" HorizontalAlign="Center"  />
                <HeaderStyle Width="60px" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField HeaderText="主题" DataField="title">
                <ItemStyle HorizontalAlign="Left"  />
                <HeaderStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField HeaderText="总数" DataField="votetotal" >
                <ItemStyle Width="60px" HorizontalAlign="Center"  />
                <HeaderStyle Width="60px" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="类型">
                <ItemStyle HorizontalAlign="Center" Width="80px" />
                <HeaderStyle HorizontalAlign="Center" Width="80px" />
                <ItemTemplate>
                    <%# chkThisType(Eval("type","{0}")) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作" ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                        Text="更新"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="取消"></asp:LinkButton>
                </EditItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="120px" />
                <HeaderStyle HorizontalAlign="Center" Width="120px" />
                <ItemTemplate>
                    <a href='?action=add&id=<%# Eval("id") %>&ChannelId=<%=Channel.Id %>'>编辑</a>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="删除"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# Eval("id") %>'
                        OnCommand="LinkButton3_Command"><%# chkEnabled(Eval("IsPass","{0}")) %></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings FirstPageText="第一页" LastPageText="最末页" Mode="NextPreviousFirstLast"
            NextPageText="下一页" PreviousPageText="上一页"  />
        <FooterStyle HorizontalAlign="Right"  />
        <PagerStyle HorizontalAlign="Right" VerticalAlign="Bottom" ForeColor="RoyalBlue"  />
    </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="plEdit" runat="server" Width="100%">
    <table width="98%" border="0" cellpadding="5" cellspacing="0" align="center" class=TableBorder>
        <tr align="center" valign="middle"> 
          <th colspan="2" height="22">投票</th>
        </tr>
        <tr bgcolor="#FFFFFF" valign="middle"> 
          <td width="25%" align="right">标题</td>
          <td align="left"><asp:TextBox ID="txtTitle" runat="server" Width="300px"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle"
                  ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
        </tr>
        <tr bgcolor="ffffff" valign="middle"> 
          <td align="right" width="25%">投票项目<br><br>
              每个项目以“|”分开<br />
              最后不要加“|”<br />
              <BR></td>
          <td align="left"><asp:TextBox ID="txtContent" runat="server" Height="120px" TextMode="MultiLine"
                  Width="300px"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtContent"
                  ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
        </tr>
        <tr bgcolor="#ffffff" valign="middle">
            <td align="right" width="25%">
                类别</td>
            <td align="left">
                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0" Selected="True">单选</asp:ListItem>
                    <asp:ListItem Value="1">多选</asp:ListItem>
                </asp:RadioButtonList></td>
        </tr>
        <tr class="tablefoot"> 
          <td align="center" valign="middle" colspan="2">
              <asp:Button ID="Button1" runat="server" Text="保 存"/>
          </td>
        </tr>
    </table>
    </asp:Panel>
    </form>
</body>
</html>