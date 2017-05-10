<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Master.aspx.cs" Inherits="XkCms.WebForm.Manager.Master" %>

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
                当前位置：管理员管理
            </td>
            <td class="forumRow" align="center" width="20%">
                <a href="?"><b>管理首页</b></a> | <a href="?action=add"><b>添加</b></a>
            </td>
            <td align="right" class="forumRow" width="40%">
                <span style="color:Red" title="添加管理后，请设定相应的权限<br>否则将不能执行任何操作。">注意事项</span>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:GridView ID="gvMasterList" runat="server" AutoGenerateColumns="False" CellPadding="5" GridLines="None" EmptyDataText="未找到帐号" OnRowDeleting="gvMasterList_RowDeleting" OnRowDataBound="gvMasterList_RowDataBound" OnRowEditing="gvMasterList_RowEditing" CssClass="tableBorder" Width="98%">
            <Columns>
                <asp:BoundField DataField="master_ID" HeaderText="ID">
                    <itemstyle width="30px" />
                    <headerstyle width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="master_name" HeaderText="登陆名">
                </asp:BoundField>
                <asp:BoundField DataField="lastime" HeaderText="上次登陆时间">
                    <itemstyle width="150px" HorizontalAlign="Center" />
                    <headerstyle width="150px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="lastIp" HeaderText="上次登陆IP">
                    <itemstyle width="120px" HorizontalAlign="Center" />
                    <headerstyle width="120px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="状态">
                    <ItemStyle Width="60px" HorizontalAlign="Center" />
                    <HeaderStyle Width="60px" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%# chkMasterState(Eval("state","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <itemstyle width="120px" HorizontalAlign="Center" />
                    <headerstyle width="120px" HorizontalAlign="Center" />
                    <itemtemplate>
                        <a href='?action=add&id=<%# Eval("master_id") %>'>编辑</a>
                        <asp:LinkButton id="LinkButton11" runat="server" Text="编辑权限" CommandName="Edit" CausesValidation="False"></asp:LinkButton>
                        <asp:LinkButton id="LinkButton12" runat="server" Text="删除" CommandName="Delete" CausesValidation="False" ></asp:LinkButton> 
                    </itemtemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="plAdd" runat="server" Width="100%" HorizontalAlign="Center">
        <table border="0" cellpadding="5" class="tableBorder" cellspacing="0" width="98%">
            <tr>
                <th colspan="2">
                    编辑管理员</th>
            </tr>
            <tr>
                <td style="text-align: right; width: 30%;">
                    登陆名：</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtMasterName" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtMasterName"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="text-align: right; width: 30%;">
                    密&nbsp; 码：</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtMasterPass" runat="server" TextMode="Password" Width="120px" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredPass" runat="server" Display="Dynamic"
                        ErrorMessage="*" ControlToValidate="txtMasterPass" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="text-align: right; width: 30%;">
                    状&nbsp; 态：</td>
                <td style="text-align: left">
                    <asp:RadioButtonList ID="rbtnMasterState" runat="server" RepeatColumns="2">
                        <Items>
                            <asp:ListItem Value="1" Selected="True">启用</asp:ListItem>
                            <asp:ListItem Value="0">禁用</asp:ListItem>
                        </Items>
                    </asp:RadioButtonList></td>
            </tr>
            <tr class="tablefoot">
                <td style="width: 30%; text-align: right">
                    &nbsp;</td>
                <td style="text-align: left">
                    <asp:Button ID="btnSaveMaster" runat="server" Text="保 存" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="plEdit" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:Literal ID="ltMasterSetting" runat="server"></asp:Literal>
        <table cellpadding="5" border="0" cellspacing="0" width="98%">
            <tr class="tablefoot">
                <td style="text-align: left" align="center">
                    &nbsp;<input name="chkall" type="checkbox" value="on" onclick="CheckAll(this.form)" />选择所有权限&nbsp;<asp:Button ID="btnSaveSetting" runat="server" OnClick="btnSaveSetting_Click"
                        Text="保 存" />
                    &nbsp;&nbsp; &nbsp;&nbsp;
                    <asp:HiddenField ID="hfMasterSettingId" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>