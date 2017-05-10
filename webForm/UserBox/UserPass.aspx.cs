using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.UserBox
{
    public partial class UserPass : UserCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User_Nav(false);
            lblUserName.Text = UserName;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "username=@name and id=" + UserId;
            doh.AddConditionParameter("@name", UserName);
            string oldpass = doh.GetValue("xk_User", "password").ToString();
            string oldpass2 = Tools.GetMD5(txtOldPass.Text);
            if (oldpass != oldpass2)
            {
                ErrMsg = "您输入的原始密码错误！";
                ShowErrMsg();
            }
            string newpass = Tools.GetMD5(txtNewPass.Text);
            doh.Reset();
            doh.ConditionExpress = "id=" + UserId;
            doh.AddFieldItem("password", newpass);
            doh.Update("xk_user");

            ErrMsg = "恭喜您！密码修改成功。";
            ShowErrMsg();
        }
    }
}
