using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Controls
{
    public partial class MoreDiss : BasicPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (lblId.Text == "{$ChannelId$}") Response.End();
            int page = XkCms.Common.Utils.Validator.StrToInt(q("page"), 1);
            Response.Write(new CreateHtml(false, doh, lblId.Text).LoadMoreDiss(page));
        }
    }
}
