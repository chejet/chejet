using System;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using XkCms.Common.Utils;

namespace XkCms.WebForm.BaseFunction
{
    public class UserCenter : Front
    {
        protected string Action = string.Empty;
        protected int id = 0;
        public string ChannelId = "0";
        protected ChannelInfo Channel = new ChannelInfo();
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ChkLogin())
                Response.Redirect("Login.aspx");

            Action = q("action");
            ChannelId = q("ChannelId");
            if (Session["FCKeditor:UserFilesPath"] == null)
                Session.Add("FCKeditor:UserFilesPath", Cms.Dir + "UserFiles");
            if (Session["Xkzi_Cms:AllowUserUpload"] == null)
                Session.Add("Xkzi_Cms:AllowUserUpload", Cms.UserUpload);
            else
                Session["Xkzi_Cms:AllowUserUpload"] = Cms.UserUpload;
            if (ChannelId == "") ChannelId = "0";
            if (ChannelId != "0" && ChannelId != "-1")
            {
                Channel = new ChannelInfo(ChannelId, doh);
                Session["FCKeditor:UserFilesPath"] = Cms.Dir + Channel.Dir + "/UserFiles";
            }
            else if (ChannelId == "0")
                Session["FCKeditor:UserFilesPath"] = Cms.Dir + "UserFiles";
            id = XkCms.Common.Utils.Validator.StrToInt(q("id"), 0);
        }

        protected void GetColumns(ref DropDownList ddl)
        {
            doh.Reset();
            doh.SqlCmd = "select id,title,code from [xk_Column] where ChannelId=" + ChannelId + " and isout=0 and isPost=1 order by code";
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count == 0)
                JsExe("alert", "alert('此频道不允许发布信息');history.go(-1)");
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem(GetColumnListName(dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString()), dt.Rows[i][0].ToString()));
                }
            }
        }

        protected void User_Nav(bool isChannel)
        {
            Response.Write("<link href='images/style.css' type=text/css rel=stylesheet>");
            Response.Write("<script type='text/javascript' src='../js/prototype.js'></script>");
            Response.Write("<script language='JavaScript' src='../Js/admin.js'></SCRIPT>");
            if (isChannel && Channel.Id == 0)
            {
                Response.Write("参数错误,请不要从外部提交数据");
                Response.End();
            }
            if (!IsPostBack)
            {
                if (Action == "add")
                    EditBox();
                else
                    GetList();
            }
        }

        /// <summary>
        /// 判断数据状态
        /// </summary>
        /// <param name="isEdit">0,1或-1</param>
        /// <returns>未审核,已审核或已删除</returns>
        protected string ChkState(string isEdit)
        {
            if (isEdit == "0")
                return "<font color='blue'>未审</font>";
            else if (isEdit == "1")
                return "已审";
            else
                return "<font color='red'>已删</font>";
        }

        protected void UpdateUserToday(int num, int pot)
        {
            int t = Validator.StrToInt(UserToday[num], 0) + 1;
            string userToday = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                if (i == num)
                    userToday += "|" + t;
                else
                    userToday += "|" + UserToday[i];
            }
            userToday = userToday.Substring(1);
            UserToday = userToday.Split('|');
            if (pot > 0)
                UserPoint += Validator.StrToInt(GroupSetting[pot], 0);
            doh.Reset();
            doh.ConditionExpress = "id=" + UserId;
            doh.AddFieldItem("UserToday", userToday);
            doh.AddFieldItem("UserPoint", UserPoint);
            doh.Update("xk_User");
            UserCookies["UserPoint"] = UserPoint.ToString();
            UserCookies["UserToday"] = userToday;
            Response.Cookies.Add(UserCookies);
        }

        protected virtual void EditBox()
        { }
        protected virtual void GetList()
        { }
    }
}
