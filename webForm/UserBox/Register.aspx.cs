using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.Collections;

namespace XkCms.WebForm.UserBox
{
    public partial class Register : Front
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ChkLogin())
                Response.Redirect("Default.aspx");
            plReg.Visible = false;
            plRegSuccess.Visible = false;
            plErrorMsg.Visible = false;
            plAnnounce.Visible = false;
            ltTempErrMsg.Visible = false;
            if (!IsPostBack)
            {
                if (Cms.AllowReg)
                {
                    plAnnounce.Visible = true;
                    txtUserName.Attributes.Add("onblur", "CheckUserExist()");
                    txtUserMail.Attributes.Add("onblur", "CheckMailExist()");
                    txtCheckCode.Attributes.Add("onblur", "CheckValidateCode()");
                    ArrayList ContentList = new CreateHtml(false, doh, string.Empty).LoadRegLogin();
                    if (ContentList.Count == 1)
                        Response.Write(ContentList[0].ToString());
                    else
                    {
                        ltTop.Text = ContentList[0].ToString();
                        ltBottom.Text = ContentList[1].ToString();
                        ltAnnounce.Text = ContentList[2].ToString();
                        ltRegSuccess.Text = ContentList[4].ToString();
                        ltTempErrMsg.Text = ContentList[5].ToString();
                    }
                }
                else
                {
                    ltErrorMsg.Text = ltTempErrMsg.Text.Replace("{$Message$}", "对不起，本站暂停新用户注册！");
                    plErrorMsg.Visible = true;
                }
            }
        }

        protected void btnReg_Click(object sender, EventArgs e)
        {
            if (!CheckValidateCode(txtCheckCode.Text))
            {
                ltErrorMsg.Text = ltTempErrMsg.Text.Replace("{$Message$}", "验证码错误！");
                plErrorMsg.Visible = true;
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "usermail=@mail";
            doh.AddConditionParameter("@mail", txtUserMail.Text);
            if (doh.Exist("xk_user"))
            {
                ltErrorMsg.Text = ltTempErrMsg.Text.Replace("{$Message$}", "用户名已经存在，请重新填写！");
                plErrorMsg.Visible = true;
                return;
            }
            
            UserInfo AUser = new UserInfo();
            AUser.Username = txtUserName.Text;
            AUser.Nickname = txtNickName.Text;
            AUser.UserMail = txtUserMail.Text;
            AUser.Password = XkCms.Common.Utils.Tools.GetMD5(txtPass.Text);
            AUser.UserSex = rblSex.SelectedValue;
            AUser.Userpoint = Cms.UserPoint;
            AUser.UserGrade = 1;
            doh.Reset();
            doh.ConditionExpress = "Grades=1";
            AUser.UserGroup = doh.GetValue("xk_UserGroup", "GroupName").ToString();
            if (Cms.CheckUser)
                AUser.IsPass = 0;
            else
                AUser.IsPass = 1;
            if (AUser.Add(doh) == 0)
            {
                ltErrorMsg.Text = ltTempErrMsg.Text.Replace("{$Message$}", "用户名已经存在，请重新填写！");
                plErrorMsg.Visible = true;
                return;
            }
            plRegSuccess.Visible = true;
            ltRegSuccess.Text = ltRegSuccess.Text.Replace("{$UserName$}", AUser.Username);
        }

        protected void btnAgree_Click(object sender, EventArgs e)
        {
            plReg.Visible = true;
        }
    }
}
