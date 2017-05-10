using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using XkCms.Common.Utils;

namespace XkCms.WebForm
{
    public partial class Ajax : Front
    {
        protected string operType = string.Empty;
        protected string response = string.Empty;

        protected override void OnError(EventArgs e)
        {
            response = "����δ֪�������Ժ����ԣ�";
            Response.Write(response);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            operType = q("oper");
            switch (operType)
            {
                case "addVote":
                    AddVote();
                    break;
                case "getVote":
                    GetVote();
                    break;
                case "getViewNum":
                    GetViewCount();
                    break;
                case "getReviewNum":
                    GetCommentCount();
                    break;
                case "addReview":
                    AddReview();
                    break;
                case "login":
                    GetLoginBar();
                    break;
                case "checkUserExist":
                    CheckUserExist();
                    break;
                case "checkMailExist":
                    CheckMailExist();
                    break;
                case "checkValidateCode":
                    CheckValidateCode();
                    break;
                case "addFav":
                    AddFavorite();
                    break;
                default:
                    DefaultResponse();
                    break;
            }

            Response.Write(response);
        }

        private void DefaultResponse()
        {
            response = "δ֪����";
        }

        private void AddVote()
        {
            int voteId = Validator.StrToInt(q("id"), 0);
            if (voteId == 0)
            {
                response = "���������벻Ҫ���ⲿ�ύ���ݣ�";
                return;
            }
            HttpCookie voteCookie = Request.Cookies["userVote" + voteId];
            if (voteCookie != null)
                response = "�벻Ҫ�ظ�ͶƱ!";
            else
            {
                string[] voteResult = q("vote").Split(',');
                doh.Reset();
                doh.ConditionExpress = "id=" + voteId + " and ispass=1";
                object[] _obj = doh.GetValues("xk_vote", "VOTETEXT,VOTENUM");
                if (_obj == null)
                    response = "���ݴ���,���Ժ�����!";
                else
                {
                    string[] voteText = _obj[0].ToString().Split('|');
                    string[] voteNum = _obj[1].ToString().Split('|');
                    int voteTotal = 0;
                    if (voteText.Length != voteNum.Length)
                    {
                        voteNum = new string[voteText.Length];
                        for (int i = 0; i < voteText.Length; i++)
                            voteNum[i] = "0";
                    }
                    for (int i = 0; i < voteResult.Length; i++)
                    {
                        int x = Validator.StrToInt(voteResult[i], 0);
                        voteNum[x] = Convert.ToString(Validator.StrToInt(voteNum[x], 0) + 1);
                    }
                    string res = "";
                    for (int i = 0; i < voteNum.Length; i++)
                    {
                        int x = Validator.StrToInt(voteNum[i], 0);
                        voteTotal += x;
                        res += "|" + x.ToString();
                    }
                    res = res.Substring(1, res.Length - 1);

                    doh.Reset();
                    doh.ConditionExpress = "id=" + q("id");
                    doh.AddFieldItem("votenum", res);
                    doh.AddFieldItem("votetotal", voteTotal);
                    doh.Update("xk_vote");

                    voteCookie = new HttpCookie("userVote" + voteId);
                    voteCookie["userVote" + voteId] = "ok";
                    voteCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(voteCookie);
                    response = "ok";
                }
            }
        }

        private void GetVote()
        {
            doh.Reset();
            doh.SqlCmd = "SELECT TITLE,VOTETEXT,VOTENUM,VOTETOTAL FROM [xk_vote] WHERE ID=" + q("id") + " And IsPass=1";
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count > 0)
            {
                response = "<table width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"5\" cellspacing=\"0\">";
                response += "<tr><td>ͶƱ��Ŀ��" + dt.Rows[0][0].ToString() + "<br>";
                response += "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">";
                string[] voteText = dt.Rows[0][1].ToString().Split('|');
                string[] voteNum = dt.Rows[0][2].ToString().Split('|');
                for (int i = 0; i < voteText.Length; i++)
                {
                    response += "<tr><td>" + (i + 1) + ". " + voteText[i] + "</td><td>" + voteNum[i] + "</td></tr>";
                }
                response += "</table></td></tr></table>";
            }
            else
                response = "���ݴ���,���Ժ�����!";
        }

        private void GetViewCount()
        {
            string cid = q("id");
            string ctype = q("cType");
            HttpCookie LookCookie = Request.Cookies["viewNum" + ctype + cid];
            if (LookCookie == null && ctype != "Soft")
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + q("id");
                doh.Count("xk_" + ctype, "viewNum");
                LookCookie = new HttpCookie("viewNum" + ctype + cid);
                LookCookie["viewNum" + ctype + cid] = "ok";
                Response.Cookies.Add(LookCookie);
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + cid;
            response = doh.GetValue("xk_" + ctype, "viewNum").ToString();
        }

        private void GetCommentCount()
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + q("id");
            response = doh.GetValue("xk_" + q("cType"), "ReviewNum").ToString();
        }

        private void GetLoginBar()
        {
            string tempBody = "";
            if (f("state") == "1")
            {
                if (!CheckValidateCode(f("code")))
                {
                    response = "error=��֤������������д��";
                    return;
                }
                string uName = f("name");
                string uPass = f("pass");
                string msg = string.Empty;
                if (UserLogin(uName, uPass, ref msg))
                    response = "ok";
                else
                {
                    response = "error=" + msg;
                    return;
                }
            }
            else if (f("state") == "-1")
            {
                UserLogout();
                response = "ok";
                return;
            }
            if (ChkLogin())
            {
                tempBody = "<ul id=\"loginBarContent\"><li><span class=\"left\">�û����ƣ�</span>" + UserName + "</li>\r"
                + "<li><span class=\"left\">�û���ݣ�</span>" + UserGroup + "</li>"
                + "<li><span class=\"registerButton\">"
                + "<a href=\"" + Cms.Dir + "UserBox/Default.aspx\">�û�����</a></span>"
                + "<span class=\"loginButton\"><input type=\"button\" onclick=\"getLoginBar(-1)\" id=\"btnLoginBarBtn\" value=\"�� ��\">"
                + "</span></li></ul>";
                response = tempBody;
                return;
            }
        }

        private void AddReview()
        {
            string uName, uId, cid, vcontent;
            cid = f("id");
            string ChannelType = f("stype");
            doh.Reset();
            doh.ConditionExpress = "id=" + cid;
            object[] _obj = doh.GetValues("xk_" + ChannelType, "channelid,columnid");
            if (_obj == null)
            {
                response = "���ݲ�����,���ܱ�����Աɾ���ˣ�";
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + _obj[0].ToString();
            if (doh.GetValue("xk_channel", "isreview").ToString() != "1")
            {
                response = "��Ƶ�������������ۣ�";
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + _obj[1].ToString();
            if (doh.GetValue("xk_column", "isreview").ToString() != "1")
            {
                response = "����Ŀ�����������ۣ�";
                return;
            }
            vcontent = f("content");
            if (vcontent == "")
            {
                response = "�������ݲ���Ϊ�գ�";
                return;
            }
            if (ChkLogin())
            {
                uName = UserName;
                uId = UserId.ToString();
            }
            else
            {
                uName = f("name");
                uId = "0";
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + cid;
            doh.Count("xk_" + ChannelType, "ReviewNum");
            doh.Reset();
            doh.AddFieldItem("ChannelId", _obj[0].ToString());
            doh.AddFieldItem("ColumnId", _obj[1].ToString());
            doh.AddFieldItem("CId", cid);
            doh.AddFieldItem("userId", uId);
            doh.AddFieldItem("AddDate", DateTime.Now.ToString());
            doh.AddFieldItem("Content", vcontent);
            doh.AddFieldItem("ip", Tools.GetUserIp());
            doh.AddFieldItem("UserName", uName);
            doh.Insert("xk_Review");
            doh.Reset();
            doh.ConditionExpress = "id=" + _obj[0].ToString();
            doh.Count("xk_Channel", "ReviewNum");
            doh.Reset();
            doh.Count("xk_system", "ReviewNum");
            response = "ok";
        }

        private void CheckUserExist()
        {
            string uName = f("name");
            if (uName == "")
                response = "no empty";
            else
            {
                doh.Reset();
                doh.ConditionExpress = "userName=@name";
                doh.AddConditionParameter("@name", uName);
                if (doh.Exist("xk_user"))
                    response = "no";
                else
                    response = "ok";
            }
        }

        private void CheckMailExist()
        {
            string mail = f("mail");
            if (!Cms.OnlyEmail)
                response = "ok";
            else
            {
                if (mail == "")
                    response = "no empty";
                else
                {
                    doh.Reset();
                    doh.ConditionExpress = "usermail=@mail";
                    doh.AddConditionParameter("@mail", mail);
                    if (doh.Exist("xk_user"))
                        response = "no";
                    else
                        response = "ok";
                }
            }
        }

        private void CheckValidateCode()
        {
            string code = f("code");
            if (CheckValidateCode(code))
                response = "ok";
            else
                response = "no";
        }

        private void AddFavorite()
        {
            if (!ChkLogin())
            {
                response = "���ȵ�½��";
                return;
            }
            if (GroupSetting[3] == "0")
            {
                response = "�Բ��������ڵ��û��鲻����ʹ���ղؼУ�";
                return;
            }
            if (GroupSetting[5] != "0")
            {
                doh.Reset();
                doh.ConditionExpress = "userid=" + UserId;
                int favCount = doh.GetCount("xk_Favorite", "id");
                if (favCount >= int.Parse(GroupSetting[5]))
                {
                    response = "�Բ��������ֻ���ղ�" + GroupSetting[5] + "����Ϣ��";
                    return;
                }
            }
            int cId = Validator.StrToInt(q("id"), 0);
            if (cId == 0)
            {
                response = "�����������Ժ����ԣ�";
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + cId;
            string ctitle = doh.GetValue("xk_" + q("cType"), "title").ToString();
            if (ctitle == string.Empty)
            {
                response = "�����������Ժ����ԣ�";
                return; 
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + q("channel");
            string cDir = doh.GetValue("xk_Channel", "dir").ToString();
            string cUrl = Cms.Dir + cDir + "/View.aspx?id=" + cId;
            doh.Reset();
            doh.ConditionExpress = "title=@title and url=@url";
            doh.AddConditionParameter("@title", ctitle);
            doh.AddConditionParameter("@url", cUrl);
            if (doh.Exist("xk_Favorite"))
            {
                response = "���Ѿ�����˴���Ϣ���벻Ҫ�ظ���ӣ�";
                return;
            }
            doh.Reset();
            doh.AddFieldItem("userId", UserId);
            doh.AddFieldItem("title", ctitle);
            doh.AddFieldItem("url", cUrl);
            doh.Insert("xk_Favorite");
            response = "ok";
        }
    }
}
