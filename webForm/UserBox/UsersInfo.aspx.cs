using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.UserBox
{
    public partial class UsersInfo : UserCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User_Nav(false);
            if (!IsPostBack)
            {
                UserInfo AUser = new UserInfo(UserId.ToString(), doh);
                lblUserName.Text = AUser.Username;
                txtEditAddress.Text = AUser.Address;
                txtEditAnswer.Text = "";
                txtEditMail.Text = AUser.UserMail;
                txtEditNickName.Text = AUser.Nickname;
                txtEditOicq.Text = AUser.Oicq;
                txtEditPhone.Text = AUser.Phone;
                txtEditPostCode.Text = AUser.Postcode;
                txtEditQuestion.Text = AUser.Question;
                txtEditTrueName.Text = AUser.TrueName;
                txtEditUserIDCard.Text = AUser.UserIDCard;
                rblEditSex.Items.FindByValue(AUser.UserSex).Selected = true;
                txtHomePage.Text = AUser.HomePage;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "username=@name and id=" + UserId;
            doh.AddConditionParameter("@name", UserName);
            string oldpass = doh.GetValue("xk_User", "password").ToString();
            string oldpass2 = Tools.GetMD5(txtPass.Text);
            if (oldpass != oldpass2)
            {
                ErrMsg = "您输入的原始密码错误！";
                ShowErrMsg();
            }

            UserInfo AUser = new UserInfo(UserId.ToString(), doh);
            AUser.Address = txtEditAddress.Text;
            if (txtEditAnswer.Text.Trim() != "")
                AUser.Answer = Tools.GetMD5(txtEditAnswer.Text);
            AUser.HomePage = txtHomePage.Text;
            AUser.Nickname = txtEditNickName.Text;
            AUser.Oicq = txtEditOicq.Text;
            AUser.Phone = txtEditPhone.Text;
            AUser.Postcode = txtEditPostCode.Text;
            AUser.TrueName = txtEditTrueName.Text;
            AUser.UserIDCard = txtEditUserIDCard.Text;
            AUser.UserMail = txtEditMail.Text;
            AUser.UserSex = rblEditSex.SelectedValue;
            AUser.Question = txtEditQuestion.Text;

            AUser.Update(doh);

            ErrMsg = "恭喜您，资料修改成功！";
            ShowErrMsg();
        }
    }
}
