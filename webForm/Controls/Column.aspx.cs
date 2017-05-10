using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.Collections;

namespace XkCms.WebForm.Controls
{
    public partial class Column : BasicPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (lblId.Text == "{$ChannelId$}") Response.End();
            string page = q("page");
            if (XkCms.Common.Utils.Validator.StrToInt(page, 0) == 0)
                page = "1";
            string _id = q("id");
            if (XkCms.Common.Utils.Validator.StrToInt(_id, 0) == 0)
                Response.Write("��ĿId����Ϊ 0 ���Ƿ���ⲿ�ύ����?");

            Response.Write(new CreateHtml(false, doh, lblId.Text).LoadList(_id, int.Parse(page)));
        }
    }
}
