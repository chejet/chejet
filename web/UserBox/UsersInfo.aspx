<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsersInfo.aspx.cs" Inherits="XkCms.WebForm.UserBox.UsersInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
        <table class="Usertableborder" cellspacing="1" cellpadding="3" align="center" border="0">
            <tr>
                <th colspan="2">修改个人资料</th>
            </tr>
            <tr>
                <td width="20%" align="right">用户名：</td>
                <td align="left">
                    <asp:Label ID="lblUserName" runat="server" ForeColor="Red" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td align="right">
                    昵称：</td>
                <td align="left">
                    <asp:TextBox ID="txtEditNickName" runat="server" Width="140px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtEditNickName"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align="right">
                    真实姓名：</td>
                <td align="left">
                    <asp:TextBox ID="txtEditTrueName" runat="server" MaxLength="4" Width="140px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    邮箱：</td>
                <td align="left">
                    <asp:TextBox ID="txtEditMail" runat="server" MaxLength="50" Width="140px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtEditMail"
                        Display="Dynamic" ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator11" runat="server" ControlToValidate="txtEditMail"
                            Display="Dynamic" ErrorMessage="*" ToolTip="邮箱格式错误" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align="right">
                    性别：</td>
                <td align="left">
                    <asp:RadioButtonList ID="rblEditSex" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True">男</asp:ListItem>
                        <asp:ListItem>女</asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>
            <tr>
                <td align="right">
                    密码提示问题：</td>
                <td align="left">
                    <asp:TextBox ID="txtEditQuestion" runat="server" Width="140px"></asp:TextBox>
                    <select onChange="txtEditQuestion.value=this.value;")>
			            <option value="" selected>[请选择]</option>
			            <option value="最喜欢的宠物？">最喜欢的宠物？</option>
			            <option value="最喜爱的电影？">最喜爱的电影？</option>
			            <option value="周年纪念日？">周年纪念日？</option>
			            <option value="父亲的名字？">父亲的名字？</option>
			            <option value="配偶的名字？">配偶的名字？</option>
			            <option value="第一个孩子的爱称？">第一个孩子的爱称？</option>
			            <option value="中学的校名？">中学的校名？</option>
			            <option value="最尊敬的老师？">最尊敬的老师？</option>
			        </select>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEditQuestion"
                        Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align="right">
                    密码问题答案：</td>
                <td align="left" style="color: red">
                    <asp:TextBox ID="txtEditAnswer" runat="server" ToolTip="如果不修改密码请留空" Width="140px"></asp:TextBox>
                    不修改请留空</td>
            </tr>
            <tr>
                <td align="right">
                    联系电话：</td>
                <td align="left" style="color: red">
                    <asp:TextBox ID="txtEditPhone" runat="server" Width="140px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtEditPhone"
                        Display="Dynamic" ErrorMessage="*" ValidationExpression="^(\(?\d{3,4}\)?)?[\s\-]?\d{7,8}$"></asp:RegularExpressionValidator>
                    如：0531-86361000</td>
            </tr>
            <tr>
                <td align="right">
                    你的QQ：</td>
                <td align="left">
                    <asp:TextBox ID="txtEditOicq" runat="server" Width="100px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtEditOicq"
                        Display="Dynamic" ErrorMessage="*" ToolTip="必须为整数" ValidationExpression="^\d*$"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align="right">
                    邮政编码：</td>
                <td align="left">
                    <asp:TextBox ID="txtEditPostCode" runat="server" Width="140px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                        ControlToValidate="txtEditPostCode" Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{6}"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align="right">
                    身份证：</td>
                <td align="left">
                    <asp:TextBox ID="txtEditUserIDCard" runat="server" Width="200px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtEditUserIDCard"
                        Display="Dynamic" ErrorMessage="*" ToolTip="身份证格式错误" ValidationExpression="\d{17}[\d|X]|\d{15}"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align="right">
                    联系地址：</td>
                <td align="left">
                    <asp:TextBox ID="txtEditAddress" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    个人主页：</td>
                <td align="left">
                    <asp:TextBox ID="txtHomePage" runat="server" Width="200px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtHomePage"
                        Display="Dynamic" ErrorMessage="格式错误，以http://开头" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align="right">
                    用户密码：</td>
                <td align="left" style="color: red">
                    <asp:TextBox ID="txtPass" runat="server" TextMode="Password" Width="100px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                        ErrorMessage="*" ControlToValidate="txtPass"></asp:RequiredFieldValidator>
                    输入正确的密码才能修改用户资料</td>
            </tr>
            <tr>
                <td align="right"></td>
                <td align="left">
                    <asp:Button ID="btnSave" runat="server" Text="确 认" OnClick="btnSave_Click" Width="100px" /></td>
            </tr>
        </table>
    </form>
</body>
</html>
