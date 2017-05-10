using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Controls
{
    public partial class Default : BasicPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (lblId.Text == "{$ChannelId$}") Response.End();
            if (Cms.IsHtml)
                Response.Redirect(Cms.IndexName);
            else
                Response.Write(new CreateHtml(false, doh, lblId.Text).LoadChannelIndex());
        }
    }
}
