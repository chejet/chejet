using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm
{
    public partial class ViewPlacard : Front
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int _id = XkCms.Common.Utils.Validator.StrToInt(q("id"), 0);
            if (_id == 0)
            {
                ErrMsg = "²ÎÊý´íÎó";
                ShowErrMsg();
            }
            else
                Response.Write(new CreateHtml(false, doh, string.Empty).LoadViewPlacard(_id));
        }
    }
}
