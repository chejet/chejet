<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Review.aspx.cs" Inherits="XkCms.WebForm.Manager.Review" %>

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
                当前位置：<asp:Label ID="lblChannel" runat="server" Text="Label"></asp:Label>
                --> 内容管理
            </td>
            <td class="forumRow" align="center" width="20%">
                <a href="?ChannelId=<%=Channel.Id %>"><b>管理首页</b></a> | <a href="?action=add&ChannelId=<%=Channel.Id %>">
                    <b>添加</b></a>
            </td>
            <td align="right" class="forumRow" width="40%">
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:GridView ID="gvReviewList" runat="server" AutoGenerateColumns="False" CellPadding="5"
            GridLines="None" EmptyDataText="未找到评论" OnRowDataBound="gvReviewList_RowDataBound" OnRowDeleting="gvReviewList_RowDeleting" OnRowEditing="gvReviewList_RowEditing" CssClass="tableBorder" Width="98%">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID">
                    <itemstyle width="20px" horizontalalign="Right" />
                    <headerstyle width="20px" horizontalalign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="内容">
                    <itemstyle horizontalalign="Left" />
                    <itemtemplate>
                        <a href='<%=Cms.Dir+Channel.Dir %>/View.aspx?id=<%# Eval("cid","{0}") %>' title='<%# Eval("content","{0}") %>' target="_blank"><%# ClipContent(Eval("content","{0}")) %></a>
                    </itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="发表日期">
                    <itemstyle width="100px" HorizontalAlign="Center" />
                    <headerstyle width="100px" HorizontalAlign="Center" />
                    <itemtemplate>
                        <%# Convert.ToDateTime(Eval("addDate","{0}")).ToShortDateString() %>
                    
    </itemtemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="UserName" HeaderText="作者">
                    <itemstyle width="50px" HorizontalAlign="Center" />
                    <headerstyle width="50px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <itemstyle width="140px" HorizontalAlign="Center" />
                    <headerstyle width="140px" HorizontalAlign="Center" />
                    <itemtemplate>
                        <asp:LinkButton id="LinkButton9" runat="server" commandName="Edit" causesValidation="False" text="编辑"></asp:LinkButton>
                        <asp:LinkButton id="LinkButton10" runat="server" Text="删除" commandName="Delete" causesValidation="False"></asp:LinkButton>
                    </itemtemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table align="center" cellpadding="0" cellspacing="0" class="TableFooter" width="98%">
            <tr>
                <td>
                    <cc1:aspnetpager id="AspNetPager1" runat="server" AlwaysShow="True" OnPageChanged="AspNetPager1_PageChanged"></cc1:aspnetpager>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="plEdit" runat="server" Width="100%" HorizontalAlign="Center">
        <table border="0" cellpadding="5" cellspacing="0" class="tableBorder" width="98%">
            <tr>
                <th>编辑评论</th>
            </tr>
            <tr>
                <td align="center">
                    <asp:TextBox ID="txtReviewContent" runat="server" Height="120px" MaxLength="200"
                        TextMode="MultiLine" Width="274px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtReviewContent"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr class="tablefoot" align="center">
                <td>
                    <asp:Button ID="btnSaveReview" runat="server" OnClick="btnSaveReview_Click"
                        Text="保 存" />
                    <asp:HiddenField ID="hfReviewId" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>