using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.Collections;

namespace XkCms.WebForm.UserBox
{
    public partial class Login : Front
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ChkLogin())
                Response.Redirect("Default.aspx");
            ltTempErrMsg.Visible = false;
            plLoginSuccess.Visible = false;
            plLoginBody.Visible = false;
            if (!IsPostBack)
            {
                plLoginBody.Visible = true;
                ArrayList ContentList = new CreateHtml(false, doh, string.Empty).LoadRegLogin();
                if (ContentList.Count == 1)
                    Response.Write(ContentList[0].ToString());
                else
                {
                    ltTop.Text = ContentList[0].ToString();
                    ltBottom.Text = ContentList[1].ToString();
                    ltLoginSuccess.Text = ContentList[3].ToString();
                    ltTempErrMsg.Text = ContentList[5].ToString();
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!CheckValidateCode(txtCheckCode.Text))
            {
                ltErrorMsg.Text = ltTempErrMsg.Text.Replace("{$Message$}", "ÑéÖ¤Âë´íÎó£¡");
                plErrorMsg.Visible = true;
                return;
            }
            string msg = string.Empty;
            if (UserLogin(txtUserName.Text, txtPass.Text, ref msg))
            {
                ltLoginSuccess.Text = ltLoginSuccess.Text.Replace("{$UserName$}", txtUserName.Text);
                plLoginSuccess.Visible = true;
            }
            else
            {
                ltErrorMsg.Text = ltTempErrMsg.Text.Replace("{$Message$}", msg);
                plErrorMsg.Visible = true;
            }
        }
    }
}
