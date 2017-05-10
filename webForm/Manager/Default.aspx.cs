using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using XkCms.DataOper.Data;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.Manager
{
    public partial class Default : BasicPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "小孔子内容管理系统";
            txtName.Focus();
            if (Session["master_id"] != null)
                Session["master_id"] = null;
            if (Session["master_name"] != null)
                Session["master_name"] = null;
        }

        protected void btnLogin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (!CheckValidateCode(txtCode.Text))
            {
                Alert("验证码错误!");
                txtCode.Text = "";
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "master_name=@name and state=1";
            doh.AddConditionParameter("@name", txtName.Text);
            object pass = doh.GetValue("xk_master", "master_password");
            if (pass != null)
            {
                if (pass.ToString() == Tools.GetMD5(txtPass.Text))
                {
                    //设置session
                    string userCookies = new Random().Next(10000000, 99999999).ToString();
                    Session.Add("master_id", userCookies);
                    Session.Add("master_name", txtName.Text);

                    //更新管理员登陆信息
                    doh.Reset();
                    doh.ConditionExpress = "master_name=@name and state=1";
                    doh.AddConditionParameter("@name", txtName.Text);
                    doh.AddFieldItem("cookiess", userCookies);
                    doh.AddFieldItem("lastime", DateTime.Now.ToString());
                    doh.AddFieldItem("lastip", Tools.GetUserIp());
                    doh.Update("xk_master");

                    Response.Redirect("Main.html");
                }
                else
                {
                    Alert("错误提示", "密码错误!");
                    txtCode.Text = "";
                }
            }
            else
            {
                Alert("错误提示", "用户名不存在或者被禁止登陆!");
                txtCode.Text = "";
            }
        }
    }
}
