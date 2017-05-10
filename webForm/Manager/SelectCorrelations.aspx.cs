using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class SelectCorrelations : Admin
    {
        private string ChannelType;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<link href='images/style.css' type=text/css rel=stylesheet>");
            Response.Write("<script type='text/javascript' src='../js/prototype.js'></script>");
            Response.Write("<script language='JavaScript' src='../Js/admin.js'></SCRIPT>");
            ChannelId = q("ChannelId");
            if (!XkCms.Common.Utils.Validator.IsNumberId(ChannelId))
            {
                ErrMsg = "请不要从外部提交数据!";
                ShowErrMsg();
            }

            doh.Reset();
            doh.ConditionExpress = "id=" + ChannelId;
            ChannelType = doh.GetValue("Xk_Channel", "type").ToString();

            AspNetPager1.PageSize = pageSize;
            if (!IsPostBack)
                getListBox();
        }

        protected override void getListBox()
        {
            doh.Reset();
            doh.ConditionExpress = "ispass=1 and ChannelId=" + ChannelId;
            int rowCount = 0;
            gvList.DataSource = doh.GetDataTable("xk_" + ChannelType, "id,title", "id", true, "id", AspNetPager1.CurrentPageIndex, pageSize, ref rowCount);
            gvList.DataBind();
            AspNetPager1.RecordCount = rowCount;
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 0, 0);
        }

        protected void AspNetPager1_PageChanged(object src, XkCms.Common.Web.PageChangedEventArgs e)
        {
            AspNetPager1.CurrentPageIndex = e.NewPageIndex;
            getListBox();
        }

        protected void btnReturnOk_Click(object sender, EventArgs e)
        {
            JsExe("返回", "window.dialogArguments.ReFresh('" + f("chkContentId") + "');window.close();");
        }
    }
}
