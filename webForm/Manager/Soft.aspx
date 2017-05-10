<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Soft.aspx.cs" Inherits="XkCms.WebForm.Manager.Soft" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
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
                <a href="?d=1d&ChannelId=<%=Channel.Id %>">今日</a> | <a href="?d=1w&ChannelId=<%=Channel.Id %>">
                    今周</a> | <a href="?d=1m&ChannelId=<%=Channel.Id %>">今月</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <a href="?e=0&ChannelId=<%=Channel.Id %>">未审核</a> | <a href="?e=1&ChannelId=<%=Channel.Id %>">
                    已审核</a> | <a href="?e=-1&ChannelId=<%=Channel.Id %>">回收站</a>&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plEdit" runat="server" HorizontalAlign="Center" Width="100%">
        <table bgcolor="#ffffff" border="1" bordercolor="#cccccc" cellpadding="1" cellspacing="0"
            class="tableBorder" style="border-collapse: collapse" width="98%">
            <tr>
                <th colspan="4">
                    编辑下载</th>
            </tr>
            <tr>
                <td style="text-align: right">
                    所属栏目</td>
                <td style="text-align: left;">
                    <asp:DropDownList ID="ddlContentColumn" runat="server">
                    </asp:DropDownList></td>
                <td nowrap="noWrap" style="text-align: right;">
                    所属专题</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlDiss" runat="server">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    名称</td>
                <td nowrap="noWrap" style="text-align: left" colspan="3">
                    <asp:DropDownList ID="ddlTcolor" runat="server" Width="74px">
                    </asp:DropDownList><asp:TextBox ID="txtContentTitle" runat="server" MaxLength="150" Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtContentTitle"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    作者/开发商</td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtContentAuthor" runat="server" MaxLength="20" Width="176px" ToolTip="<=20 Chars">本站编辑</asp:TextBox></td>
                <td nowrap="noWrap" style="text-align: right">
                    来源</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlSource" runat="server" Width="74px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    缩略图</td>
                <td style="text-align: left;" nowrap="noWrap">
                        <select id="upimgSelect" onchange="changeImg()" style="width: 74px" title="可以选择在编辑器中上传的图片">
                            <option value="">选择图片</option>
                        </select><asp:TextBox ID="txtImg" runat="server" Width="210px" MaxLength="150" ToolTip="图片的网络地址或选择上传图片"></asp:TextBox></td>
                <td nowrap="noWrap" style="text-align: right;">
                    推荐</td>
                <td style="text-align: left">
                    <asp:RadioButtonList ID="rbtnContentTop" runat="server" RepeatColumns="2">
                        <Items>
                            <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                            <asp:ListItem Value="1">是</asp:ListItem>
                        </Items>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    软件类别</td>
                <td nowrap="noWrap" style="text-align: left;">
                    <asp:DropDownList ID="ddlSoftType" runat="server">
                        <asp:ListItem Selected="True">国产软件</asp:ListItem>
                        <asp:ListItem>国外软件</asp:ListItem>
                        <asp:ListItem>汉化补丁</asp:ListItem>
                        <asp:ListItem>程序源码</asp:ListItem>
                        <asp:ListItem>音频视频</asp:ListItem>
                        <asp:ListItem>其它</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp; 软件语言<asp:DropDownList ID="ddlLanguage" runat="server">
                        <asp:ListItem Selected="True">英语</asp:ListItem>
                        <asp:ListItem>简体中文</asp:ListItem>
                        <asp:ListItem>繁体中文</asp:ListItem>
                        <asp:ListItem>简繁中文</asp:ListItem>
                        <asp:ListItem>多国语言</asp:ListItem>
                        <asp:ListItem>其它语言</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp; 授权形式<asp:DropDownList ID="ddlCopyrightType" runat="server">
                        <asp:ListItem Selected="True">免费版</asp:ListItem>
                        <asp:ListItem>共享版</asp:ListItem>
                        <asp:ListItem>试用版</asp:ListItem>
                        <asp:ListItem>演示版</asp:ListItem>
                        <asp:ListItem>注册版</asp:ListItem>
                        <asp:ListItem>破解版</asp:ListItem>
                        <asp:ListItem>零售版</asp:ListItem>
                        <asp:ListItem>OEM版</asp:ListItem>
                    </asp:DropDownList></td>
                <td nowrap="noWrap" style="text-align: right;">
                    软件版本</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtVersion" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    软件平台</td>
                <td nowrap="nowrap" style="text-align: left">
                    <asp:TextBox ID="txtOperatingSystem" runat="server" Width="241px">Win9x/NT/2000/XP/2003</asp:TextBox><br />
                    <font color='#808080'>
                    平台选择：
                    <a href="javascript:ToSystem('Linux')">Linux</a>/<a 
                    href="javascript:ToSystem('DOS')">DOS</a>/<a
                     href="javascript:ToSystem('9x')">9x</a>/<a
                      href="javascript:ToSystem('95')">95</a>/<a
                       href="javascript:ToSystem('98')">98</a>/<a
                        href="javascript:ToSystem('Me')">Me</a>/<a
                         href="javascript:ToSystem('NT')">NT</a>/<a
                          href="javascript:ToSystem('2000')">2000</a>/<a
                           href="javascript:ToSystem('XP')">XP</a>/<a
                            href="javascript:ToSystem('.NET')">.NET</a>/<a
                             href="javascript:ToSystem('Vista')">Vista</a></font>
                    </td>
                <td nowrap="nowrap" style="text-align: right">
                    解压密码</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtPassword" runat="server" Width="101px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    演示地址</td>
                <td nowrap="nowrap" style="text-align: left">
                    <asp:TextBox ID="txtDemoUrl" runat="server" Width="320px">http://</asp:TextBox></td>
                <td nowrap="nowrap" style="text-align: right">
                    注册地址</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtRegUrl" runat="server" Width="241px">http://</asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    关键字</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtContentKeyWord" runat="server" MaxLength="150" Width="320px" ToolTip="英文逗号分隔"></asp:TextBox></td>
                <td style="text-align: right">
                    相关文章</td>
                <td style="text-align: left">
                    <asp:HiddenField ID="HiddenFieldCorrelationIDs" runat="server" />
                    <input id="ButtonCorrelationIDs" type="button" value="选择" onclick="ShowDialog('Correlations.aspx?ChannelId=<%=Channel.Id %>&id=<%=id %>&ContentIDs='+$('HiddenFieldCorrelationIDs').value+'&keys='+$('txtContentKeyWord').value,600,600,0)" title="如果不选择相关内容，系统将根据关键字自动匹配。" /></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    简介</td>
                <td colspan="3" style="text-align: left">
                    <fckeditorv2:fckeditor id="FCKeditor1" runat="server" basepath="" height="200px"
                        toolbarset="" width="520px">
                    </fckeditorv2:fckeditor>
                </td>
            </tr>
            <tr>
                <td nowrap="noWrap" style="text-align: right">
                    下载地址</td>
                <td style="text-align: left;">
                    <select id="lbDownUrl" multiple="multiple" style="width: 400px; height: 99px;">
                    </select>
                </td>
                <td colspan="2" align="left">
                    <asp:HiddenField ID="hfDownUrl" runat="server" />
                    <input type="button" value="批量上传文件" onclick="ShowUpload('Soft')" /><br />
                    <input type='button' value='添加外部地址' onclick='AddUrl()'/><br/>
                    <input type='button' value='修改当前地址' onclick='return ModifyUrl()'/><br />
                    <input type='button' value='删除当前地址' onclick='DelUrl()'/>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    下载次数<asp:TextBox ID="txtViewNum" runat="server" Width="43px" ToolTip="数字">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtViewNum"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtViewNum"
                        ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数" Display="Dynamic"></asp:RegularExpressionValidator>
                        更新日期<asp:TextBox ID="txtAddDate" runat="server" Width="120px" ToolTip="请选择日期"></asp:TextBox><a 
                    href="#SelectDate" onclick="SD(this,'document.all.txtAddDate')"><img
                     border="0" src="images/date_picker.gif" width="30" height="19" align="absMiddle"></a>
                     <asp:CheckBox ID="chkIsEdit" runat="server" Text="立即发布" TextAlign="Left" /></td>
            </tr>
            <tr class="tablefoot">
                <td colspan="4" align="center">
                    <asp:Button ID="btnSaveContent" runat="server" Text="保 存" Width="180px" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="plList" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:GridView ID="gvArticleList" runat="server" AutoGenerateColumns="False" CellPadding="5"
            CssClass="tableBorder" EmptyDataText="未找到文章" GridLines="None" Width="98%" OnRowDataBound="gvArticleList_RowDataBound" OnRowDeleting="gvArticleList_RowDeleting">
            <Columns>
                <asp:TemplateField HeaderText="选">
                    <ItemStyle Width="10px" />
                    <HeaderStyle Width="10px" />
                    <ItemTemplate>
                        <input id="chkContentId" class="checkbox" name="chkContentId" type="checkbox" value='<%#Eval("id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="id" HeaderText="ID">
                    <ItemStyle HorizontalAlign="Right" Width="20px" />
                    <HeaderStyle Width="20px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="标题">
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%#Eval("title") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="columnName" HeaderText="栏目">
                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="下/评">
                    <ItemStyle HorizontalAlign="Center" Width="60px" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                    <ItemTemplate>
                        <%#Eval("viewNum") %>/<%#Eval("ReviewNum") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="状态">
                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                    <ItemTemplate>
                        <%# ChkState(Eval("ispass","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                    <HeaderStyle HorizontalAlign="Center" Width="90px" />
                    <ItemTemplate>
                        <a href='?action=add&id=<%#Eval("id") %>&ChannelId=<%#Eval("ChannelId") %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table align="center" cellpadding="0" cellspacing="0" class="TableFooter" width="98%">
        <tr>
            <td style="text-align:left">
                <input name="chkall" onclick="CheckAll(this.form)" type="checkbox" />全选
                <asp:DropDownList ID="ddlOper" runat="server">
                    <asp:ListItem Value="pass">审核通过</asp:ListItem>
                    <asp:ListItem Value="nopass">审核未通过</asp:ListItem>
                    <asp:ListItem Value="move">批量移动到-=&gt;</asp:ListItem>
                    <asp:ListItem Value="sdel">移到回收站</asp:ListItem>
                    <asp:ListItem Value="del">直接删除</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlColumn" runat="server">
                </asp:DropDownList>
                <asp:Button ID="btnExecute" runat="server" OnClick="btnExecute_Click" Text="执 行" />
            </td>
            <td>
                <asp:Literal ID="ltContentPager" runat="server"></asp:Literal>
            </td>
        </tr>
        </table>
        <br />
        <table align="center" border="0" cellpadding="0" cellspacing="0" style="border-right: #6bba21 1px solid;
            border-top: #6bba21 1px solid; border-left: #6bba21 1px solid; border-bottom: #6bba21 1px solid"
            width="98%">
            <tr align="center" bgcolor="#f7fbff" height="28" valign="middle">
                <td>
                    快速搜索文章：<asp:DropDownList ID="ddlKeyType" runat="server">
                        <asp:ListItem Value="title">标题</asp:ListItem>
                        <asp:ListItem Value="author">作者</asp:ListItem>
                        <asp:ListItem Value="Summary">简介</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlKeyColumn" runat="server">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtKeyWord" runat="server" Width="200px"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="搜 索" /></td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>