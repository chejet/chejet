using System;
using System.Web;
using System.Data;
using XkCms.Common.Utils;

namespace XkCms.WebForm.BaseFunction
{
    public class Front : BasicPage
    {
        protected HttpCookie UserCookies;
        protected string CookieName = "XkCmsCookies";
        protected int UserId = 0;
        protected string UserName = string.Empty;
        protected string UserGroup = string.Empty;
        protected int UserGrade = 0;
        protected string[] UserToday;
        protected int UserPoint = 0;
        protected string[] GroupSetting;
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Cms.IsOpen)
            {
                Response.Write(Cms.CloseMessage);
                Response.End();
            }
            if (Cms.IsTiming && !SysOpration(Cms.OpenTime))
            {
                Response.Write(Cms.CloseMessage);
                Response.End();
            }
            UserCookies = Request.Cookies[CookieName];
            ErrMsg = string.Empty;
        }

        protected bool SysOpration(string sysObj)
        {
            string[] t2 = sysObj.Split(',');
            int t1 = t2.Length;
            int time1 = DateTime.Now.Hour;
            if (t1 % 2 != 0)
            {
                return false;
            }
            for (int i = 0; i < t1; i += 2)
            {
                if (time1 < Convert.ToInt32(t2[i].ToString()) || time1 > Convert.ToInt32(t2[i + 1].ToString())) return false;
            }
            return true;
        }

        /// <summary>
        /// 验证登陆
        /// </summary>
        protected bool ChkLogin()
        {
            if (UserCookies == null || UserCookies.Value == string.Empty)
                return false;
            else
            {
                UserName = Server.UrlDecode(UserCookies["UserName"]);
                UserId = Validator.StrToInt(UserCookies["UserId"], 0);
                UserGrade = Validator.StrToInt(UserCookies["UserGrade"], 0);
                UserPoint = Validator.StrToInt(UserCookies["UserPoint"], 0);
                UserGroup = Server.UrlDecode(UserCookies["UserGroup"]);
                GroupSetting = UserCookies["UserGradeSet"].Split('|');
                UserToday = UserCookies["UserToday"].Split('|');
                
                if (UserId > 0 && UserName != "")
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + UserId + " and UserName=@name and IsPass=1";
                    doh.AddConditionParameter("@name", UserName);
                    if (doh.Exist("xk_user"))
                    {
                        if (UserToday.Length < 6)
                            UserToday = new string[] { "0", "0", "0", "0", "0", "0" };
                        return true;
                    }
                }
            }
            UserLogout();
            return false;
        }

        protected bool UserLogin(string uName, string uPass, ref string msg)
        { 
            doh.Reset();
            doh.ConditionExpress = "username=@name";
            doh.AddConditionParameter("@name", uName);
            object[] _obj = doh.GetValues("xk_user", "IsPass,password,Id");
            if (_obj == null)
            {
                msg = "用户名不存在，是否拼写错误！";
                return false;
            }
            if (_obj[0].ToString() == "0")
            {
                msg = "此账户未审核或被锁定，暂时不能登陆！";
                return false;
            }
            if (_obj[1].ToString() != XkCms.Common.Utils.Tools.GetMD5(uPass))
            {
                msg = "密码错误，是否拼写错误！";
                return false;
            }
            else
            {
                UserInfo AUser = new UserInfo(_obj[2].ToString(), doh);
                doh.Reset();
                doh.ConditionExpress = "Grades=" + AUser.UserGrade;
                string GroupSet = doh.GetValue("Xk_UserGroup", "GroupSet").ToString();
                GroupSetting = GroupSet.Split('|');

                if (UserCookies == null)
                    UserCookies = new HttpCookie(CookieName);
                UserCookies["LastTimeDate"] = AUser.LastTime.ToString();
                UserCookies["LastTimeIP"] = AUser.Userlastip;
                UserCookies["UserId"] = AUser.Id.ToString();
                UserCookies["UserName"] = Server.UrlEncode(AUser.Username);
                UserCookies["NickName"] = Server.UrlEncode(AUser.Nickname);
                UserCookies["UserGrade"] = AUser.UserGrade.ToString();
                UserCookies["UserGroup"] = Server.UrlEncode(AUser.UserGroup);
                if (Tools.DateDiff(AUser.LastTime, DateTime.Now).Days > 0)
                    AUser.UserToday = "0|0|0|0|0|0";
                UserCookies["UserToday"] = AUser.UserToday;
                UserCookies["UserPoint"] = AUser.Userpoint.ToString();
                UserCookies["UserGradeSet"] = GroupSet;
                UserCookies.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(UserCookies);

                if (Tools.DateDiff(DateTime.Now, AUser.LastTime).Days != 0)
                {
                    if (AUser.Userpoint < 0)
                        AUser.Userpoint = int.Parse(GroupSetting[25]);
                    else
                        AUser.Userpoint += int.Parse(GroupSetting[25]);
                    if (AUser.Experience < 0)
                        AUser.Experience = int.Parse(GroupSetting[32]);
                    else
                        AUser.Experience += int.Parse(GroupSetting[32]);
                    if (AUser.Charm < 0)
                        AUser.Charm = int.Parse(GroupSetting[33]);
                    else
                        AUser.Charm += int.Parse(GroupSetting[25]);
                }
                AUser.LastTime = DateTime.Now;
                AUser.Userlastip = XkCms.Common.Utils.Tools.GetUserIp();
                AUser.Userlogin++;
                AUser.Update(doh);
                return true;
            }
        }

        protected void UserLogout()
        {
            UserCookies = new HttpCookie(CookieName);
            Response.Cookies.Add(UserCookies);
            UserCookies = null;
        }

        protected void ShowErrMsg()
        {
            if (ErrMsg == string.Empty) return;
            Response.Write("<table class=\"Usertableborder\" cellspacing=\"1\" cellpadding=\"3\" align=\"center\" border=\"0\"><tr><th>提示信息</th></tr><tr><td><li>" + ErrMsg + "</li></td></tr><tr><td align=center><a href=\"javascript:history.go(-1)\">返回上一页</a></td></tr></table>");
            Response.End();
        }
    }
}
