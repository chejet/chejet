using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm
{
    public partial class Redirect : Front
    {
        public string ReUrl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = Cms.Name;
            ReUrl = q("url");
        }
    }
}
