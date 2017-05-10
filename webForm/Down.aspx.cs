using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm
{
    public partial class Down : Front
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = q("id");
            if (!XkCms.Common.Utils.Validator.IsNumberId(id) || id == "0")
            {
                Response.Write("请不要修改地址栏参数!");
                Response.End();
            }
            int down = XkCms.Common.Utils.Validator.StrToInt(q("no"), 0);
            doh.Reset();
            doh.ConditionExpress = "ispass=1 and id=" + id;
            string tempurl = doh.GetValue("Xk_Soft", "downUrl").ToString();
            if (tempurl == string.Empty || tempurl.Trim() == "")
            {
                Response.Write("你所请求的地址不存在,请确认是否修改了地址栏参数!");
                Response.End();
            }
            string[] downUrl = tempurl.Split(new string[] { "|||" }, StringSplitOptions.None);
            if ((down + 1) > downUrl.Length)
            {
                Response.Write("你所请求的地址不存在,请确认是否修改了地址栏参数!");
                Response.End();
            }
            string[] downUrls = downUrl[down].Split('|');
            string downs = string.Empty;
            if (downUrls.Length == 2)
                downs = downUrls[1];
            else
                downs = downUrls[0];
            HttpCookie LookCookie = Request.Cookies["viewNumSoft" + id];
            if (LookCookie == null)
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + id;
                doh.Count("xk_Soft", "ViewNum");
                LookCookie = new HttpCookie("viewNumSoft" + id);
                LookCookie["viewNumSoft" + id] = "ok";
                Response.Cookies.Add(LookCookie);
            }
            Response.Redirect(downs);
        }
    }
}
