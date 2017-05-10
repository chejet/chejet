<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Diss.aspx.cs" Inherits="XkCms.WebForm.Manager.Diss" %>
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
            <td align="left" class="forumRow" height="22" width="40%">
                当前位置：<asp:Label ID="lblChannel" runat="server" Text="Label"></asp:Label> --&gt; 专题管理
            </td>
            <td align="center" class="forumRow" width="20%">
                <a href="?ChannelId=<%=Channel.Id %>"><b>管理首页</b></a> | <a href="?action=add&ChannelId=<%=Channel.Id %>">
                    <b>添加</b></a>
            </td>
            <td align="right" class="forumRow" width="40%">
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" CellPadding="5" GridLines="None" OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting" EmptyDataText="暂无专题" CssClass="tableBorder" DataKeyNames="id" Width="98%">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" DataFormatString="[{0}]">
                    <itemstyle width="50px" HorizontalAlign="Right" ForeColor="DodgerBlue" />
                    <headerstyle width="50px"/>
                </asp:BoundField>
                <asp:BoundField DataField="title" HeaderText="专题名称">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="推荐">
                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                    <ItemTemplate>
                        <%# chkTop(Eval("isTop","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <itemstyle width="120px" HorizontalAlign="Center" />
                    <headerstyle width="120px" HorizontalAlign="Center" />
                    <itemtemplate>
                        <a href='?action=add&ChannelId=<%# Eval("ChannelId") %>&id=<%#Eval("id") %>'>编辑</a>
                        <asp:LinkButton id="LinkButton6" runat="server" Text="删除" CausesValidation="False" CommandName="Delete">
                    </asp:LinkButton> 
</itemtemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
        <asp:Panel ID="plEdit" runat="server" HorizontalAlign="Center" Width="100%">
            <table border="0" cellpadding="5" cellspacing="0" class="tableBorder" width="98%">
                <tr>
                    <th colspan="2">
                        添加专题</th>
                </tr>
                <tr>
                    <td style="text-align: right">
                        专题名称</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtName" runat="server" MaxLength="20" Width="239px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                            ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator>
                        <asp:CheckBox ID="chkIsTop" runat="server" Text="推荐" /></td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        专题图片</td>
                    <td style="text-align: left">
                        <select id="upimgSelect" onchange="changeImg()">
                            <option selected="selected" value="">选择图片</option>
                        </select>
                        <asp:TextBox ID="txtImg" runat="server" MaxLength="150" Width="238px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        专题列表模板</td>
                    <td style="text-align: left">
                        <asp:DropDownList ID="ddlTemplate" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        每页记录数</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtPageSize" runat="server" Width="37px">20</asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPageSize"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        专题简介</td>
                    <td style="text-align: left;">
                        <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" BasePath="" Height="220px"
                            Width="515px">
                        </FCKeditorV2:FCKeditor>
                    </td>
                </tr>
                <tr class="tablefoot">
                    <td style="text-align: right">
                        &nbsp;</td>
                    <td style="text-align: left">
                        <asp:Button ID="btnSaveColumn" runat="server" Text="保 存" /></td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>