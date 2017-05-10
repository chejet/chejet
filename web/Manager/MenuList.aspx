<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuList.aspx.cs" Inherits="XkCms.WebForm.Manager.MenuList" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <style type=text/css>
    <!--
    BODY{
	    margin:0px;
	    FONT-SIZE: 12px;
	    FONT-FAMILY:  "宋体", "Verdana", "Arial", "Helvetica", "sans-serif";
	    background-color: #799AE1;
	    scrollbar-face-color: #C7D4F7; 
        scrollbar-highlight-color: #DEE6FA; 
	    scrollbar-shadow-color: #799AE1; 
	    scrollbar-darkshadow-color: #DEE6FA; 
	    scrollbar-3dlight-color: #DEE6FA; 
	    scrollbar-arrow-color: #fff;
	    scrollbar-track-color: #E4EDF9;
    }
    table  { border:0px; }
    td  { font:normal 12px 宋体; }
    img  { vertical-align:bottom; border:0px; }
    a  { font:normal 12px 宋体; color:#000000; text-decoration:none; }
    a:hover  { color:#428EFF;text-decoration:underline; }
    .sec_menu  { border-left:1px solid white; border-right:1px solid white; border-bottom:1px solid white; overflow:hidden; background:#D6DFF7; }
    .menu_title  { cursor: pointer; }
    .menu_title span  { position:relative; top:2px; left:8px; color:#215DC6; font-weight:bold; }
    -->
    </style>
    <script type="text/javascript" src="../js/admin.js"></script>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="158" align="left">
        <tr>
		    <td background="images/admin_title.gif" style="height:38px;"></td>
	    </tr>
		<tr>         
			<td height="25" class="menu_title" background="images/title_bg_title.gif"> <span><a href="Right.html" target="fmain"><b>管理首页</b></a> | <a href="" onclick="if(confirm('确定退出吗?')){top.location='Default.aspx'}"><b>退出</b></a></span> </td>
	    </tr>
	    <tr>
	        <td height="12"></td>
	    </tr>
	    <tr>
	        <td>
	        <asp:Panel ID="plMenu" runat="server"></asp:Panel>
	        </td>
	    </tr>
	    <tr>
	        <td>
	            <table cellpadding=0 cellspacing=0 width=158 align=center>
                <tr>         
	                <td height=25 class=menu_title background="images/title_bg_info.gif"> <span>系统信息</span> </td>
                </tr>
                <tr> 
                    <td> <div class=sec_menu style="width:158"> 
                    <table cellpadding=0 cellspacing=0 align=center width=135>
		                <tr>
			                <td height=20>&nbsp;<br>版权所有：<BR><a href="http://www.xkzi.com/" target=_blank>www.xkzi.com</a><BR>
                                <br />
			                </td>
		                </tr>
	                </table>
                    </div></td>
                </tr>
                </table>
	        </td>
	    </tr>
	</table>
    </form>
</body>
</html>
