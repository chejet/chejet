<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CollItem.aspx.cs" Inherits="XkCms.WebForm.Manager.CollItem" %>

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
                </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:GridView ID="gvList" runat="server" Width="98%" AutoGenerateColumns="False" CellPadding="5" GridLines="None" CssClass="tableBorder" OnRowDataBound="gvList_RowDataBound" OnRowDeleting="gvList_RowDeleting">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID">
                    <ItemStyle HorizontalAlign="Center" Width="40px" Wrap="False" />
                    <HeaderStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="title" HeaderText="项目名称">
                    <ItemStyle HorizontalAlign="Center" Width="80px" Wrap="False" />
                    <HeaderStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="webname" HeaderText="采集来源">
                    <ItemStyle HorizontalAlign="Center" Width="80px" Wrap="False" />
                    <HeaderStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="channelname" HeaderText="所属频道">
                    <ItemStyle HorizontalAlign="Center" Width="80px" Wrap="False" />
                    <HeaderStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="columnname" HeaderText="所属栏目">
                    <ItemStyle HorizontalAlign="Center" Width="80px" Wrap="False"  />
                    <HeaderStyle Width="80px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="状态">
                    <ItemStyle HorizontalAlign="Center" Width="40px" Font-Bold="True" Wrap="False" />
                    <HeaderStyle Width="40px" />
                    <ItemTemplate>
                        <%# chkFlag(Eval("flag","{0}")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Width="120px" Wrap="False" />
                    <ItemTemplate>
                        <a href='?action=add&id=<%# Eval("id","{0}") %>'>编辑</a>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="LinkButton2_Command">复制</asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Width="120px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="设置">
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a href='?action=setList&id=<%# Eval("id","{0}") %>'>列表设置</a>
                        <a href='?action=setLink&id=<%# Eval("id","{0}") %>'>链接设置</a>
                        <a href='?action=setContent&id=<%# Eval("id","{0}") %>'>正文设置</a>
                        <a href='?action=test&id=<%# Eval("id","{0}") %>'>采样测试</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="采集">
                    <ItemStyle Width="50px" Wrap="False" HorizontalAlign="Center" />
                    <HeaderStyle Width="50px" Wrap="False" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a href='?action=start&id=<%# Eval("id","{0}") %>'>采集</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="plAdd" runat="server" Width="100%" HorizontalAlign="Center">
        <table border="0" cellpadding="5" class="tableBorder" cellspacing="0" width="98%">
            <tr>
                <th colspan="2">
                    编辑采集项目</th>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    项目名称</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    所属频道</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlChannel" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged">
                        <asp:ListItem Value="0">请选择</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    每页图片</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtPageSize" runat="server" Width="40px">5</asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtPageSize"
                        ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator>(适用图片频道)</td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    所属栏目</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlColumn" runat="server">
                        <asp:ListItem Value="0">请选择</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    所属专题</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlDiss" runat="server">
                        <asp:ListItem Value="0">请选择</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    页面编码方式</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlEncode" runat="server">
                        <asp:ListItem Value="0">默认编码</asp:ListItem>
                        <asp:ListItem Value="1">GB2312</asp:ListItem>
                        <asp:ListItem Value="2">UTF-8</asp:ListItem>
                        <asp:ListItem Value="3">Unicode</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    网站名称</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtWebName" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    网站地址</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtWebUrl" runat="server" Width="299px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtWebUrl"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    项目说明</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtItemDemo" runat="server" Height="66px" TextMode="MultiLine" Width="300px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" class="forumRowHighlight" colspan="2">
                    采集属性
                </td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    阅读次数</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtViewNum" runat="server" Width="35px">0</asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtViewNum"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtViewNum"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    作者设置</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtAuthor" runat="server">佚名</asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    来源设置</td>
                <td style="text-align: left">
                    <asp:DropDownList ID="ddlFrom" runat="server" AppendDataBoundItems="True">
                        <asp:ListItem Value="0">请选择</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    过滤标签</td>
                <td style="text-align: left">
                    <asp:CheckBox ID="chkIframe" runat="server" Height="24px" Text=" Iframe" Width="72px" />&nbsp;<asp:CheckBox
                        ID="chkObject" runat="server" Height="24px" Text=" Object" Width="72px" />
                    <asp:CheckBox ID="chkScript" runat="server" Height="24px" Text=" Script" Width="72px" />
                    <asp:CheckBox ID="chkDiv" runat="server" Height="24px" Text=" Div" Width="72px" />
                    <asp:CheckBox ID="chkTable" runat="server" Height="24px" Text=" Table" Width="72px" /><br />
                    <asp:CheckBox ID="chkSpan" runat="server" Height="24px" Text=" Span" Width="72px" />
                    <asp:CheckBox ID="chkImg" runat="server" Height="24px" Text=" Img" Width="72px" />
                    <asp:CheckBox ID="chkFont" runat="server" Height="24px" Text=" Font" Width="72px" />
                    <asp:CheckBox ID="chkA" runat="server" Height="24px" Text=" A" Width="72px" />
                    <asp:CheckBox ID="chkHtml" runat="server" Height="24px" Text=" Html" Width="72px" /></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    采集数量</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtTopNum" runat="server" Width="35px">0</asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTopNum" Display="Dynamic"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                            runat="server" ControlToValidate="txtTopNum" Display="Dynamic" ErrorMessage="*"
                            ValidationExpression="^\d*$" ToolTip="必须为整数"></asp:RegularExpressionValidator>
                    0为无限制</td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    其它选项</td>
                <td style="text-align: left">
                    <asp:CheckBox ID="chkSaveImg" runat="server" Text="保存图片" />
                    <asp:CheckBox ID="chkIsDesc" runat="server" Text="倒序采集" /></td>
            </tr>
            <tr class="tablefoot">
                <td style="width: 30%; text-align: right">
                    &nbsp;</td>
                <td style="text-align: left">
                    <asp:Button ID="btnStep1" runat="server" Text="保 存" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="plSetList" runat="server" HorizontalAlign="Center" Width="100%">
        <table border="0" cellpadding="5" class="tableBorder" cellspacing="0" width="98%">
            <tr>
                <th colspan="2">
                    编辑采集项目&nbsp; --&nbsp; 列表设置</th>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    列表页面地址</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtListIndex" runat="server" Width="281px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtListIndex"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtListIndex"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    列表开始标记</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtListStart" runat="server" Height="89px" TextMode="MultiLine"
                        Width="318px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtListStart"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    列表结束标记</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtListEnd" runat="server" Height="89px" TextMode="MultiLine" Width="318px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtListEnd"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr class="tablefoot">
                <td style="width: 30%; text-align: right">
                    &nbsp;</td>
                <td style="text-align: left">
                    <input type="button" value="上一步" onclick="history.go(-1)" />&nbsp; &nbsp;<asp:Button ID="btnSetList" runat="server" Text="保 存" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="plSetLink" runat="server" Width="100%" HorizontalAlign="Center">
        <table border="0" cellpadding="5" class="tableBorder" cellspacing="0" width="98%">
            <tr>
                <th colspan="2">
                    列表截取测试</th>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Literal ID="ltListTest" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <th colspan="2">
                    编辑采集项目&nbsp; --&nbsp; 链接设置</th>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    链接开始标记</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtLinkStart" runat="server" Height="46px" TextMode="MultiLine"
                        Width="299px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtLinkStart"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    链接结束标记</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtLinkEnd" runat="server" Height="46px" TextMode="MultiLine" Width="299px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtLinkEnd"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr class="tablefoot">
                <td style="width: 30%; text-align: right">
                </td>
                <td style="text-align: left"><input type="button" value="上一步" onclick="history.go(-1)" />&nbsp; &nbsp;<asp:Button ID="btnSetLink" runat="server" Text="保 存" /></td>
            </tr>
        </table>
    </asp:Panel><asp:Panel ID="plSetContent" runat="server" Width="100%" HorizontalAlign="Center">
        <table border="0" cellpadding="5" class="tableBorder" cellspacing="0" width="98%">
            <tr>
                <th colspan="2">
                    新闻链接截取测试</th>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    请检查取得的链接是否正确，如不正确请检查链接设置和网站地址</td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Literal ID="ltLinkTest" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <th colspan="2">
                    编辑采集项目&nbsp; --&nbsp; 正文设置</th>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    标题开始标记</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtTitleStart" runat="server" Height="46px" TextMode="MultiLine"
                        Width="299px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtTitleStart"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    标题结束标记</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtTitleEnd" runat="server" Height="46px" TextMode="MultiLine" Width="299px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtTitleEnd"
                        ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    正文开始标记</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtContentStart" runat="server" Height="46px" TextMode="MultiLine"
                        Width="299px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator12"
                            runat="server" ControlToValidate="txtContentStart" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    正文结束标记</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtContentEnd" runat="server" Height="46px" TextMode="MultiLine"
                        Width="299px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator13"
                            runat="server" ControlToValidate="txtContentEnd" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    下页链接开始标记</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtNPageStart" runat="server" Height="46px" TextMode="MultiLine"
                        Width="299px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: right">
                    下页链接结束标记</td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtNPageEnd" runat="server" Height="46px" TextMode="MultiLine" Width="299px"></asp:TextBox></td>
            </tr>
            <tr class="tablefoot">
                <td style="width: 30%; text-align: right">
                </td>
                <td style="text-align: left">
                    <input type="button" value="上一步" onclick="history.go(-1)" />&nbsp;
                    <asp:Button ID="btnSetContent" runat="server" Text="保 存" />
                    <asp:HiddenField ID="hfIsPaging" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="plTest" runat="server" Width="100%" HorizontalAlign="Center">
        <table border="0" cellpadding="5" class="tableBorder" cellspacing="0" width="98%">
            <tr>
                <th>采样测试</th>
            </tr>
            <tr>
                <td align="center" class="forumRowHighlight">
                    <asp:Literal ID="ltTestTitle" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Literal ID="ltTestContent" runat="server"></asp:Literal><br />
                    <asp:Literal ID="ltPhotoUrl" runat="server"></asp:Literal></td>
            </tr>
            <tr class="tablefoot">
                <td align="center">
                    <input type="button" value="上一步" onclick="history.go(-1)" />&nbsp;
                    <input type="button" value="完成" onclick="window.location='CollItem.aspx'" id="Button1" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>