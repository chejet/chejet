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
            this.Title = "С�������ݹ���ϵͳ";
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
                Alert("��֤�����!");
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
                    //����session
                    string userCookies = new Random().Next(10000000, 99999999).ToString();
                    Session.Add("master_id", userCookies);
                    Session.Add("master_name", txtName.Text);

                    //���¹���Ա��½��Ϣ
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
                    Alert("������ʾ", "�������!");
                    txtCode.Text = "";
                }
            }
            else
            {
                Alert("������ʾ", "�û��������ڻ��߱���ֹ��½!");
                txtCode.Text = "";
            }
        }
    }
}
