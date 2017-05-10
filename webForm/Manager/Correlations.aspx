<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Correlations.aspx.cs" Inherits="XkCms.WebForm.Manager.Correlations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>相关内容</title>
    <base target="_self">
    </base>
    <meta http-equiv="pragma" content="no-cache">
    <script type="text/javascript">
        function ReFresh(AddContentIDs){
            var strContentIds=$("HiddenFieldContentIDs").value;
            if(strContentIds==""){
                $("HiddenFieldContentIDs").value=AddContentIDs;
            }else{
                $("HiddenFieldContentIDs").value+=","+AddContentIDs;
            }
            $("ButtonReFresh").click();
        }
        
        function Delete(ContentID){
            var strContentIds=$("HiddenFieldContentIDs").value;
            strContentIds=strContentIds.replace(ContentID,"");
            strContentIds=strContentIds.replace(",,",",");
            if(strContentIds.indexOf(",")==0){
                strContentIds=strContentIds.substring(1)
            }
            if(strContentIds.lastIndexOf(",")==strContentIds.length-1){
                strContentIds=strContentIds.substring(0,strContentIds.length-1)
            }
            $("HiddenFieldContentIDs").value=strContentIds;
            $("ButtonReFresh").click();
        }
        
        function ReturnOk(){
            window.dialogArguments.document.getElementById("HiddenFieldCorrelationIDs").value=$("HiddenFieldContentIDs").value;
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table align="center" cellpadding="1" cellspacing="1" class="TableBorder" width="100%">
            <tr>
                <td align="left" class="forumRow" height="22">
                    选择相关内容
                </td>
                <td align="right" class="forumRow">
                    <asp:Button ID="btnAutoSelect" runat="server" Text="根据关键字匹配" Height="20px" OnClick="btnAutoSelect_Click" />
                    <input id="ButtonInsertArticle" type="button" value="新增相关内容" onclick="ShowDialog('SelectCorrelations.aspx?ChannelId=<%=ChannelId %>',600,600,0)" style="height: 20px" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="plList" runat="server" Width="100%" HorizontalAlign="Center">
        <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" CellPadding="5"
            CssClass="tableBorder" GridLines="None" Width="100%" OnRowDataBound="gvList_RowDataBound" ShowHeader="False">
            <Columns>
                <asp:TemplateField>
                    <HeaderStyle Width="10%" />
                    <ItemTemplate>
                        <a href="javascript:Delete('<%# Eval("id") %>')">[删除]</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="标题" DataField="title" />
            </Columns>
        </asp:GridView>
        <table align="center" cellpadding="1" cellspacing="1" class="TableFooter" width="100%">
            <tr>
                <td>
                <input id="ButtonOk" type="button" value="确定" onclick="ReturnOk();" />
            </td>
        </tr>
        </table>
        </asp:Panel>
        <div style="display:none">
            <asp:HiddenField ID="HiddenFieldContentIDs" runat="server" />
            <asp:HiddenField ID="HiddenFieldKeys" runat="server" />
            <asp:Button ID="ButtonReFresh" runat="server" Text="ReFresh" OnClick="ButtonReFresh_Click" />
        </div>
    </form>
</body>
</html>
