using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.UserBox
{
    public partial class UserSms : UserCenter
    {
        private int maxSms, smsCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GroupSetting[22] == "0")
            {
                ErrMsg = "对不起，您所在的用户组不允许使用短信服务！";
                ShowErrMsg();
            }
            if (!IsPostBack)
            {
                maxSms = Validator.StrToInt(GroupSetting[24], 0);
                smsCount = 0;
                doh.Reset();
                doh.ConditionExpress = "(toUser=@name and flag=0) or (fromUser=@name and isDel=0)";
                doh.AddSqlCmdParameters("@name", UserName);
                smsCount = doh.GetCount("xk_message", "id");
                int imgWidth = (smsCount * 250) / maxSms;
                ltSmsBar.Text = "<img src='Images/hr.gif' width='" + imgWidth + "' height='10' alt='最大容量：" + maxSms + "条，已存储" + smsCount + "条'>";
                lblSmsUse.Text = (smsCount * 100) / maxSms + "%";
            }
            User_Nav(false);
        }

        protected override void GetList()
        {
            string box = q("box");
            if (box != "in" && box != "to")
                box = "in";

            #region 操作
            if (!IsPostBack)
            {
                if (Action == "delall")
                {
                    doh.Reset();
                    doh.ConditionExpress = "toUser=@name";
                    doh.AddSqlCmdParameters("@name", UserName);
                    doh.Delete("xk_message");

                    doh.Reset();
                    doh.ConditionExpress = "fromUser=@name";
                    doh.AddSqlCmdParameters("@name", UserName);
                    doh.AddFieldItem("isDel", "1");
                    doh.Update("xk_message");
                }
                else if (Action == "delin")
                {
                    doh.Reset();
                    doh.ConditionExpress = "toUser=@name";
                    doh.AddSqlCmdParameters("@name", UserName);
                    doh.Delete("xk_message");
                }
                else if (Action == "delto")
                {
                    doh.Reset();
                    doh.ConditionExpress = "fromUser=@name";
                    doh.AddSqlCmdParameters("@name", UserName);
                    doh.AddFieldItem("isDel", "1");
                    doh.Update("xk_message");
                }
                else if (Action == "del")
                {
                    if (box == "in")
                    {
                        doh.Reset();
                        doh.ConditionExpress = "flag=0 and id=" + id;
                        doh.Delete("xk_message");
                    }
                    else
                    {
                        doh.Reset();
                        doh.ConditionExpress = "flag=0 and id=" + id;
                        doh.AddFieldItem("isDel", "1");
                        doh.Update("xk_message");
                    }
                }
                else if (Action == "view")
                {
                    if (id == 0)
                    {
                        ErrMsg = "参数错误！";
                        ShowErrMsg();
                    }
                    plView.Visible = true;
                    plEdit.Visible = false;
                    plList.Visible = false;
                    doh.Reset();
                    doh.ConditionExpress = "id=" + id;
                    object[] _obj = doh.GetValues("xk_message", "id,title,fromUser,toUser,sendTime,content,isRead,flag");
                    if (_obj == null)
                    {
                        ErrMsg = "参数错误！";
                        ShowErrMsg();
                    }
                    lblFromUser.Text = _obj[2].ToString();
                    lblToUser.Text = _obj[3].ToString();
                    lblSendTime.Text = _obj[4].ToString();
                    lblSmsTitle.Text = _obj[1].ToString();
                    lblSmsContent.Text = _obj[5].ToString();

                    if (_obj[7].ToString() == "0" && _obj[6].ToString() == "0" && _obj[2].ToString() != UserName)
                    {
                        doh.Reset();
                        doh.ConditionExpress = "id=" + id;
                        doh.AddFieldItem("isRead", "1");
                        doh.Update("xk_message");

                        doh.Reset();
                        doh.ConditionExpress = "flag=0 and toUser=@name and isRead=0";
                        doh.AddConditionParameter("@name", UserName);
                        int usermsg = doh.GetCount("xk_message", "id");
                        doh.Reset();
                        doh.ConditionExpress = "id=" + UserId;
                        doh.AddFieldItem("usermsg", usermsg);
                        doh.Update("xk_User");
                    }
                    return;
                }
            }
            #endregion

            #region 列表
            plList.Visible = true;
            plEdit.Visible = false;
            plView.Visible = false;
            AspNetPager1.PageSize = 10;
            int rowCount = 0;
            if (box == "in")
            {
                doh.Reset();
                doh.ConditionExpress = "toUser=@sname or flag=1";
                doh.AddSqlCmdParameters("sname", UserName);
                rpInList.DataSource = doh.GetDataTable("xk_message", "id,fromUser,title,SendTime,isRead,flag", "id", true, "id", AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, ref rowCount);
                rpInList.DataBind();
                rpInList.Visible = true;
                rpOutList.Visible = false;
            }
            else
            {
                doh.Reset();
                doh.ConditionExpress = "fromUser=@name and isDel=0";
                doh.AddSqlCmdParameters("@name", UserName);
                rpOutList.DataSource = doh.GetDataTable("xk_message", "id,toUser,title,SendTime,isRead,flag", "id", true, "id", AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, ref rowCount);
                rpOutList.DataBind();
                rpInList.Visible = false;
                rpOutList.Visible = true;
            }
            AspNetPager1.RecordCount = rowCount;
            #endregion
        }

        protected override void EditBox()
        {
            if (smsCount >= maxSms)
            {
                ErrMsg = "对不起，您的信箱已满，请删除部分信件后再试！";
                ShowErrMsg();
            }
            plEdit.Visible = true;
            plView.Visible = false;
            plList.Visible = false;

            string oper = q("oper");
            if (oper != "")
            {
                if (id == 0)
                {
                    ErrMsg = "参数错误！";
                    ShowErrMsg();
                }
                doh.Reset();
                doh.ConditionExpress = "id=" + id;
                object[] _obj = doh.GetValues("xk_message", "id,title,fromUser,toUser,sendTime,content");
                if (_obj == null)
                {
                    ErrMsg = "参数错误！";
                    ShowErrMsg();
                }
                if (oper == "rw")
                {
                    txtTitle.Text = "Re：" + _obj[1].ToString();
                    txtToUser.Text = _obj[2].ToString();
                    txtContent.Text = "\r\r\r\r\r\r在" + _obj[4].ToString() + "，" + _obj[2].ToString() + "写道：\r" + _obj[5].ToString();
                }
                else if (oper == "fw")
                {
                    txtTitle.Text = "Fw：" + _obj[1].ToString();
                    txtToUser.Text = "";
                    txtContent.Text = "\r\r===========转发短信息=========\r发件人：" + _obj[2].ToString() + "\r发送日期：" + _obj[4].ToString() + "\r收件人：" + _obj[3].ToString() + "\r主题：" + _obj[1].ToString() + "\r\r" + _obj[5].ToString();
                }
            }
        }

        protected string CheckState(string isRead, string flag)
        {
            if (flag == "1")
                return "<img src=\"images/m_system.gif\" border=0 alt=\"系统信息\">";
            else
            {
                if (isRead == "0")
                    return "<img src=\"images/m_news.gif\" border=0 alt=\"未读\">";
                else
                    return "<img src=\"images/m_olds.gif\" border=0 alt=\"已读\">";
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (txtToUser.Text.Trim() == UserName)
            {
                ErrMsg = "请不要给自己发短信！";
                ShowErrMsg();
            }
            int maxContent = Validator.StrToInt(GroupSetting[23], 200);
            if (txtContent.Text.Length > maxContent)
            {
                ErrMsg = "短信内容不允许大于 " + maxContent + " 个字符！";
                ShowErrMsg();
            }
            doh.Reset();
            doh.ConditionExpress = "username=@name";
            doh.AddConditionParameter("@name", txtToUser.Text.Trim());
            if (!doh.Exist("xk_User"))
            {
                ErrMsg = "没有找到" + txtToUser.Text + "，请确认用户名是否正确！";
                ShowErrMsg();
            }
            doh.Reset();
            doh.AddFieldItem("title", txtTitle.Text);
            doh.AddFieldItem("fromUser", UserName);
            doh.AddFieldItem("toUser", txtToUser.Text);
            doh.AddFieldItem("content", txtContent.Text);
            doh.AddFieldItem("sendTime", DateTime.Now);
            doh.AddFieldItem("isdel", 0);
            doh.AddFieldItem("isRead", 0);
            doh.AddFieldItem("flag", 0);
            doh.Insert("xk_message");

            doh.Reset();
            doh.ConditionExpress = "username=@name";
            doh.AddConditionParameter("@name", txtToUser.Text);
            doh.Count("xk_User", "usermsg");

            UpdateUserToday(4, 0);
            Response.Redirect("UserSms.aspx?box=to");
        }
    }
}
