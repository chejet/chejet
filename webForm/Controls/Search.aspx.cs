using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.Collections;

namespace XkCms.WebForm.Controls
{
    public partial class Search : BasicPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (lblId.Text == "{$ChannelId$}") Response.End();
            if (f("keywords") != "")
                txtkeyWords.Text = f("keywords").Trim().Replace("'", "").Replace("\"", "");
            SearchList.Visible = false;
            if (!IsPostBack)
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + lblId.Text;
                object[] _obj = doh.GetValues("Xk_Channel", "type,title");
                hfChannelType.Value = _obj[0].ToString();
                ArrayList ContentList = new CreateHtml(false, doh, string.Empty).LoadOther(_obj[1].ToString() + " ËÑË÷");
                if (ContentList.Count == 1)
                    Response.Write(ContentList[0].ToString());
                else
                {
                    ltTop.Text = ContentList[0].ToString();
                    ltBottom.Text = ContentList[1].ToString();
                }
                if (txtkeyWords.Text != "")
                    GetList();
            }
        }

        protected void GetList()
        {
            SearchList.Visible = true;
            if (txtkeyWords.Text.Trim() == "") return;
            string keys = txtkeyWords.Text.Trim().Replace("'", "").Replace("\"", "");
            doh.Reset();
            doh.ConditionExpress = "title like '%" + keys + "%' or [keyword] like '%" + keys + "%'";
            int rowCount = 0;
            rpList.DataSource = doh.GetDataTable("Xk_" + hfChannelType.Value, "id,columnid,columnname,title,AddDate,keyword", "id", true, "id", AspNetPager1.CurrentPageIndex, 20, ref rowCount);
            rpList.DataBind();
            AspNetPager1.PageSize = 20;
            AspNetPager1.RecordCount = rowCount;
        }

        protected void AspNetPager1_PageChanged(object src, XkCms.Common.Web.PageChangedEventArgs e)
        {
            AspNetPager1.CurrentPageIndex = e.NewPageIndex;
            GetList();
        }

        protected void btnSearch_Click(object senger, EventArgs e)
        {
            GetList();
        }
    }
}
