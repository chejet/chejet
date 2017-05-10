using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using XkCms.WebForm.BaseFunction;

namespace XkCms.WebForm.Manager
{
    public partial class MenuList : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[,] menu = leftMenu();
            Title = "小孔子文章管理系统 - - 后台管理";
            Table tb = new Table();
            tb.Width = Unit.Parse("158");
            tb.CellPadding = 0;
            tb.CellSpacing = 0;
            string tcText = "";
            TableRow tr;
            TableCell tc;
            for (int i = 0; i < menu.GetLength(0); i++)
            {
                if (menu[i, 0] == null) break;
                tr = new TableRow();
                tc = new TableCell();
                tc.Height = 25;
                tc.CssClass = "menu_title";
                tc.Attributes.Add("background", "images/title_bg_show.gif");
                tc.Attributes.Add("onclick", "showsubmenu(" + i + "," + menu.GetLength(0) + ")");
                tc.Attributes.Add("id", "menu" + i);
                tc.Text = "<span>" + menu[i, 0].Split('$')[0] + "</span>";
                tr.Cells.Add(tc);
                tb.Rows.Add(tr);

                tr = new TableRow();
                tc = new TableCell();
                tc.Attributes.Add("style", "display:none");
                tc.Attributes.Add("id", "submenu" + i);
                tcText = "<div class=sec_menu>";
                for (int j = 1; j < menu.GetLength(1); j++)
                {
                    if (menu[i, j] == null)
                    {
                        break;
                    }
                    tcText += "<table cellpadding=0 cellspacing=0><tr><td width='20' height='20' align='right'><img src='images/ico.gif' align='absmiddle'></td><td align='left'>";
                    tcText += menu[i, j];
                    tcText += "</td></tr></table>";
                }
                tcText += "</div><div  style='width:158'><table cellpadding=0 cellspacing=0 align=center width=135><tr><td height=10></td></tr></table></div>";
                tc.Text = tcText;
                tr.Cells.Add(tc);
                tb.Rows.Add(tr);
            }
            this.plMenu.Controls.Add(tb);
        }
    }
}
