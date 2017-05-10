using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.UserBox
{
    public partial class Main : UserCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User_Nav(false);
            UserInfo AUser = new UserInfo(UserId.ToString(), doh);
            lblCharm.Text = AUser.Charm.ToString();
            lblExperince.Text = AUser.Experience.ToString();
            lblLastIp.Text = AUser.Userlastip;
            lblLastTime.Text = AUser.LastTime.ToString();
            lblLogin.Text = AUser.Userlogin.ToString();
            lblMsg.Text = AUser.Usermsg.ToString();
            lblNickName.Text = AUser.Nickname;
            lblRegTime.Text = AUser.JoinTime.ToString();
            lblTrueName.Text = AUser.TrueName;
            lblUserGroup.Text = AUser.UserGroup;
            lblUserName.Text = AUser.Username;
            lblUserPoint.Text = AUser.Userpoint.ToString();
        }
    }
}
