<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Redirect.aspx.cs" Inherits="XkCms.WebForm.Redirect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body topmargin="0" leftmargin="0" rightmargin="0" bottommargin="0">
    <form id="form1" runat="server">
        <iframe id="main" style="width: 100%;height:100%" name="main" src="<%= ReUrl %>" scrolling="auto" frameborder="0" marginheight="0" marginwidth="0" onload="this.height=document.frames['main'].document.body.scrollHeight"></iframe>
    </form>
</body>
</html>
