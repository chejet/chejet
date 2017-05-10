<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Photo.aspx.cs" Inherits="XkCms.WebForm.UserBox.Photo" %>
<%@ Register Assembly="XkCms.Common" Namespace="XkCms.Common.Web" TagPrefix="cc1" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="plEdit" runat="server" Width="100%">
            <table class="Usertableborder" cellspacing="1" cellpadding="3" align="center" border="0">
                <tr>
                    <th colspan="2" align="center">
                        发布图片</th>
                </tr>
                <tr>
                    <td width="10%" nowrap="nowrap" align="right">
                        所属栏目
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlColumn" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="right">
                        标题</td>
                    <td align="left">
                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="240" Width="400px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle"
                            ErrorMessage="*"></asp:RequiredFieldValidator></td>
                </tr>
                <tr style="color: #000000">
                    <td align="right">
                        作者</td>
                    <td align="left">
                        <asp:TextBox ID="txtContentAuthor" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtContentAuthor"
                            ErrorMessage="*"></asp:RequiredFieldValidator></td>
                </tr>
                <tr style="color: #000000">
                    <td align="right">
                        关键字</td>
                    <td align="left">
                        <asp:TextBox ID="txtContentKeyWord" runat="server" MaxLength="150" ToolTip="以英语逗号分隔"
                            Width="400px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right">
                        标题图片</td>
                    <td align="left">
                        <select id="upimgSelect" onchange="changeImg()" style="width: 74px" title="可以选择在编辑器中上传的图片">
                            <option selected="selected" value="">选择图片</option>
                        </select>
                        <asp:TextBox ID="txtImg" runat="server" MaxLength="250" ToolTip="图片的网络地址或选择上传图片"
                            Width="322px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align="right">简介</td>
                    <td align="left">
                    <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" ToolbarSet="Basic" BasePath="../fckEditor/"
                            Height="200px" Width="500px">
                        </FCKeditorV2:FCKeditor>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        图片地址</td>
                    <td align="left">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td width="402" valign="top">
                                    <select id="lbDownUrl" multiple="multiple" style="width: 400px; height: 82px">
                                    </select>
                                </td>
                                <td valign="top">
                                    <asp:HiddenField ID="hfDownUrl" runat="server" />
                                    <input onclick="ShowOneUpload('Photo')" type="button" value="上传本地图片" /><br />
                                    <input onclick="AddPhoto()" type="button" value="添加外部地址" /><br />
                                    <input onclick="return ModifyPhoto()" type="button" value="修改当前地址" /><br />
                                    <input onclick="DelPhoto()" type="button" value="删除当前地址" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        验证码</td>
                    <td align="left">
                    <asp:TextBox id="txtCheckCode" runat="server" Width="76px" AutoCompleteType="Disabled"></asp:TextBox><IMG id="ValidateCode" src="../ValidateImg.aspx" align=absMiddle /><asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" ControlToValidate="txtCheckCode" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td align="right">
                    </td>
                    <td align="left">
                        <asp:Button ID="btnSave" runat="server" Text="发 布" Width="120px" OnClick="btnSave_Click" /></td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
            <asp:GridView ID="gvList" runat="server" CellPadding="3" CssClass="Usertableborder" AutoGenerateColumns="False" BorderWidth="1px" EmptyDataText="暂无内容" OnRowDeleting="gvList_RowDeleting" OnRowDataBound="gvList_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="title" HeaderText="标题" />
                    <asp:TemplateField HeaderText="状态">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ispass") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                        <HeaderStyle HorizontalAlign="Center" Width="40px" />
                        <ItemTemplate>
                            <%# ChkState(Eval("ispass", "{0}")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作" ShowHeader="False">
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                        <ItemTemplate>
                            <a href="?action=add&ChannelId=<%# Eval("channelid") %>&id=<%# Eval("id") %>">编辑</a> 
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                                Text="删除"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <cc1:AspNetPager ID="AspNetPager1" runat="server" AlwaysShow="True" OnPageChanged="AspNetPager1_PageChanged">
            </cc1:AspNetPager>
        </asp:Panel>
    </form>
</body>
</html>
