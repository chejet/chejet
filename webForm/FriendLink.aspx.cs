using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm
{
    public partial class FriendLink : Front
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int pageIndex = XkCms.Common.Utils.Validator.StrToInt(q("page"), 1);
            Response.Write(new CreateHtml(false, doh, string.Empty).LoadFriendLink(pageIndex));
        }
    }
}
