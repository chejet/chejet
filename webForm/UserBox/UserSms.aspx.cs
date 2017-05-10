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
                ErrMsg = "�Բ��������ڵ��û��鲻����ʹ�ö��ŷ���";
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
                ltSmsBar.Text = "<img src='Images/hr.gif' width='" + imgWidth + "' height='10' alt='���������" + maxSms + "�����Ѵ洢" + smsCount + "��'>";
                lblSmsUse.Text = (smsCount * 100) / maxSms + "%";
            }
            User_Nav(false);
        }

        protected override void GetList()
        {
            string box = q("box");
            if (box != "in" && box != "to")
                box = "in";

            #region ����
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
                        ErrMsg = "��������";
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
                        ErrMsg = "��������";
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

            #region �б�
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
                ErrMsg = "�Բ�������������������ɾ�������ż������ԣ�";
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
                    ErrMsg = "��������";
                    ShowErrMsg();
                }
                doh.Reset();
                doh.ConditionExpress = "id=" + id;
                object[] _obj = doh.GetValues("xk_message", "id,title,fromUser,toUser,sendTime,content");
                if (_obj == null)
                {
                    ErrMsg = "��������";
                    ShowErrMsg();
                }
                if (oper == "rw")
                {
                    txtTitle.Text = "Re��" + _obj[1].ToString();
                    txtToUser.Text = _obj[2].ToString();
                    txtContent.Text = "\r\r\r\r\r\r��" + _obj[4].ToString() + "��" + _obj[2].ToString() + "д����\r" + _obj[5].ToString();
                }
                else if (oper == "fw")
                {
                    txtTitle.Text = "Fw��" + _obj[1].ToString();
                    txtToUser.Text = "";
                    txtContent.Text = "\r\r===========ת������Ϣ=========\r�����ˣ�" + _obj[2].ToString() + "\r�������ڣ�" + _obj[4].ToString() + "\r�ռ��ˣ�" + _obj[3].ToString() + "\r���⣺" + _obj[1].ToString() + "\r\r" + _obj[5].ToString();
                }
            }
        }

        protected string CheckState(string isRead, string flag)
        {
            if (flag == "1")
                return "<img src=\"images/m_system.gif\" border=0 alt=\"ϵͳ��Ϣ\">";
            else
            {
                if (isRead == "0")
                    return "<img src=\"images/m_news.gif\" border=0 alt=\"δ��\">";
                else
                    return "<img src=\"images/m_olds.gif\" border=0 alt=\"�Ѷ�\">";
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (txtToUser.Text.Trim() == UserName)
            {
                ErrMsg = "�벻Ҫ���Լ������ţ�";
                ShowErrMsg();
            }
            int maxContent = Validator.StrToInt(GroupSetting[23], 200);
            if (txtContent.Text.Length > maxContent)
            {
                ErrMsg = "�������ݲ�������� " + maxContent + " ���ַ���";
                ShowErrMsg();
            }
            doh.Reset();
            doh.ConditionExpress = "username=@name";
            doh.AddConditionParameter("@name", txtToUser.Text.Trim());
            if (!doh.Exist("xk_User"))
            {
                ErrMsg = "û���ҵ�" + txtToUser.Text + "����ȷ���û����Ƿ���ȷ��";
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
