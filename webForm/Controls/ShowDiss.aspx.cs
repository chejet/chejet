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
                Response.Write("��������,�벻Ҫ�޸Ĳ����ύ!");
                Response.End();
            }
            Response.Write(new CreateHtml(false, doh, lblId.Text).LoadDiss(DissId, XkCms.Common.Utils.Validator.StrToInt(q("page"), 1)));
        }
    }
}
