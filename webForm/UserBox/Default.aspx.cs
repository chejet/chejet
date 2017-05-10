using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.WebForm.BaseFunction;

namespace XkCms.WebForm.UserBox
{
    public partial class Default : UserCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(new CreateHtml(false, doh, string.Empty).LoadUserBox());
        }
    }
}
