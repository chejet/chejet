using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm
{
    public partial class Default : Front
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Cms.IsHtml)
                Response.Redirect(Cms.IndexName);
            CreateHtml mh = new CreateHtml(false, doh, string.Empty);
            Response.Write(mh.LoadIndex());
        }
    }
}
