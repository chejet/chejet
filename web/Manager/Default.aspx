<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="XkCms.WebForm.Manager.Default" %>

<html>
<head runat="server">
    <title>小孔子内容管理系统</title>
    <style type="text/css">
    <!--
    BODY 
    {
        scrollbar-face-color: #A7E83C; 
        scrollbar-highlight-color: #A7E83C; 
	    scrollbar-shadow-color: #799AE1; 
	    scrollbar-darkshadow-color: #A7E83C; 
	    scrollbar-3dlight-color: #A7E83C; 
	    scrollbar-arrow-color: #fff;
	    scrollbar-track-color: #C9F779;
	    FONT-SIZE: 12px; 
	    margin: 0px;
	    padding: 0px;
	    background:url(images/bg.gif);
    }
    .left{
        font-size: 12px;
        color: #fff;
        font-weight:bold;
        FONT-FAMILY: "Verdana";
    }
    .ipt {
	    font-size: 9pt;
	    color: #5C8106;
	    border: 1px solid #5C8106;
	    width: 120px;
	    height: 18px;
	    background:#F2F5D4;
    }
    -->
    </style>
</head>
<body>
    <table width='100%' height='100%' border='0' cellpadding='0' cellspacing='0'><tr><td valign="middle" align="center">
    <form id="form1" runat="server">
        <table width="508" height="158" border="0" cellpadding="0" cellspacing="0">
	        <tr>
		        <td colspan="4" background="images/loginbg_01.gif" width="508" height="34"></td>
	        </tr>
	        <tr>
		        <td rowspan="2" background="images/loginbg_02.gif" width="284" height="73"></td>
		        <td rowspan="2" background="images/loginbg_03.gif" width="141" height="73" align="right">
		            <TABLE border=0 cellPadding=0 cellSpacing=0>
                        <TR> 
                          <TD class="left">ID：</TD>
                          <TD>
                              <asp:TextBox ID="txtName" runat="server" CssClass="ipt" Width="88px" Height="19px" AutoCompleteType="Disabled"></asp:TextBox></TD>
                          <td width="10">
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                  ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator>
                          </td>
                        </TR>
                        <TR> 
                          <TD class="left">PW：</TD>
                          <TD> 
                              <asp:TextBox ID="txtPass" runat="server" CssClass="ipt" TextMode="Password" Width="88px" Height="19px" AutoCompleteType="Disabled"></asp:TextBox><span
                                  style="color: #7b8ac3"></span></TD>
                           <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPass"
                                  ErrorMessage="*" ToolTip="此项不允许为空"></asp:RequiredFieldValidator>
                           </td>
                        </TR>
                        <tr>
                            <td class="left">
                                VC：</td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="ipt" Width="44px" Height="19px" AutoCompleteType="Disabled"></asp:TextBox><img src="../ValidateImg.aspx" align="absmiddle" /></td>
                            <td><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ToolTip="此项不允许为空" ErrorMessage="*" ControlToValidate="txtCode"></asp:RequiredFieldValidator></td>
                        </tr>
                    </TABLE>
		        </td>
		        <td>
			        <asp:ImageButton ID="btnLogin" runat="server" BorderStyle="None" Height="51px" ImageUrl="Images/loginbg_04.gif"
            OnClick="btnLogin_Click" Width="46px" /></td>
		        <td rowspan="2" background="images/loginbg_05.gif" width="37" height="73"></td>
	        </tr>
	        <tr>
		        <td background="images/loginbg_06.gif" width="46" height="22"></td>
	        </tr>
	        <tr>
		        <td colspan="4" background="images/loginbg_07.gif" width="508" height="51"></td>
	        </tr>
        </table>
        </form>
    </td></tr></table>
</body>
</html>
