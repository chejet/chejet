using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.UserBox
{
    public partial class Favorite : UserCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User_Nav(false);
            GetList();
        }

        protected override void GetList()
        {
            int rowCount = 0;
            AspNetPager1.PageSize = 10;
            doh.Reset();
            doh.ConditionExpress = "userid=" + UserId;
            gvList.DataSource = doh.GetDataTable("xk_Favorite", "id,title,url", "id", true, "id", AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, ref rowCount);
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
            AspNetPager1.RecordCount = rowCount;
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 1, 1);
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value.ToString();
            doh.Delete("xk_Favorite");
            GetList();
        }
    }
}
