using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Controls
{
    public partial class Content : BasicPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (lblId.Text == "{$ChannelId$}") Response.End();
            string ContentId = q("id");
            if (!XkCms.Common.Utils.Validator.IsNumberId(ContentId) || ContentId == "0")
            {
                Response.Write("参数错误,请不要修改参数提交!");
                Response.End();
            }
            Response.Write(new CreateHtml(false, doh, lblId.Text).LoadView(ContentId));
        }
    }
}
