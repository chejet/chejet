using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Controls
{
    public partial class ShowDiss : BasicPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string DissId = q("id");
            if (!XkCms.Common.Utils.Validator.IsNumberId(DissId))
            {
                Response.Write("参数错误,请不要修改参数提交!");
                Response.End();
            }
            Response.Write(new CreateHtml(false, doh, lblId.Text).LoadDiss(DissId, XkCms.Common.Utils.Validator.StrToInt(q("page"), 1)));
        }
    }
}
