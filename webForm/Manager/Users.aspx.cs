using System;
using System.Data;
using System.Web.UI.WebControls;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.Manager
{
    public partial class Users : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("012");
        }

        protected override void getListBox()
        {
            plEditBox.Visible = false;
            plList.Visible = true;
            DataTable dt;
            string wStr = "";
            if (ddlKeyGroup.Items.Count == 0)
            {
                doh.Reset();
                doh.SqlCmd = "select grades,groupname from xk_usergroup where grades>0 order by grades";
                dt = doh.GetDataTable();
                ListItem li = new ListItem("全部", "-1");
                ddlKeyGroup.Items.Add(li);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    li = new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString());
                    ddlKeyGroup.Items.Add(li);
                    ddlGroup.Items.Add(li);
                }
                ddlKeyGroup.Items.Add(new ListItem("未审核用户", "-2"));
                ddlKeyGroup.Items.Add(new ListItem("已审核用户", "-3"));
            }

            if (ddlKeyGroup.SelectedValue != null && ddlKeyGroup.SelectedValue != "")
            {
                if (ddlKeyGroup.SelectedValue == "-2")
                    wStr = "IsPass=0 and ";
                else if (ddlKeyGroup.SelectedValue == "-3")
                    wStr = "IsPass=1 and ";
                else if (ddlKeyGroup.SelectedValue != "-1")
                    wStr = "UserGrade=" + ddlKeyGroup.SelectedValue + " and ";

                if (txtKeyWord.Text.Trim() != "")
                    wStr += ddlKeyType.SelectedValue + " like '%" + txtKeyWord.Text.Trim() + "%' and ";
            }
            int rowCount = 0;
            doh.Reset();
            doh.ConditionExpress = wStr + "1=1";
            gvList.DataSource = doh.GetDataTable("Xk_User", "id,username,usergroup,usersex,JoinTime,LastTime,userlogin,ispass", "id", true, "id", AspNetPager1.CurrentPageIndex, pageSize, ref rowCount);
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();

            //设置分页
            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = rowCount;
            AspNetPager1.AlwaysShow = true;
        }

        protected override void editBox()
        {
            plEditBox.Visible = true;
            plList.Visible = false;

            if (!IsPostBack)
            {
                doh.Reset();
                doh.SqlCmd = "select grades,groupname from xk_usergroup where grades>0 order by grades";
                DataTable dt = doh.GetDataTable();
                ddlAddGroup.DataSource = dt;
                ddlAddGroup.DataTextField = "groupname";
                ddlAddGroup.DataValueField = "grades";
                ddlAddGroup.DataBind();
                ddlEditGroup.DataSource = dt;
                ddlEditGroup.DataTextField = "groupname";
                ddlEditGroup.DataValueField = "grades";
                ddlEditGroup.DataBind();
            }

            if (id == "0")
            {
                plAdd.Visible = true;
                plEdit.Visible = false;
                txtAddPoint.Text = Cms.UserPoint.ToString();
            }
            else
            {
                if (!IsPostBack)
                {
                    plAdd.Visible = false;
                    plEdit.Visible = true;
                    UserInfo AUser = new UserInfo(id, doh);
                    lblEditName.Text = AUser.Username;
                    txtEditAddress.Text = AUser.Address;
                    txtEditAnswer.Text = "";
                    txtEditCharm.Text = AUser.Charm.ToString();
                    txtEditExperience.Text = AUser.Experience.ToString();
                    txtEditFace.Text = AUser.UserFace;
                    txtEditMail.Text = AUser.UserMail;
                    txtEditNickName.Text = AUser.Nickname;
                    txtEditOicq.Text = AUser.Oicq;
                    txtEditPass.Text = "";
                    txtEditPass2.Text = "";
                    txtEditPhone.Text = AUser.Phone;
                    txtEditPoint.Text = AUser.Userpoint.ToString();
                    txtEditPostCode.Text = AUser.Postcode;
                    txtEditQuestion.Text = AUser.Question;
                    txtEditTrueName.Text = AUser.TrueName;
                    txtEditUserIDCard.Text = AUser.UserIDCard;
                    lblEditUserlogin.Text = AUser.Userlogin.ToString();
                    lblEditJoinTime.Text = AUser.JoinTime.ToString();
                    lblEditLastTime.Text = AUser.LastTime.ToString();
                    lblEditUserLastIp.Text = AUser.Userlastip;
                    rblEditIsPass.Items.FindByValue(AUser.IsPass.ToString()).Selected = true;
                    rblEditSex.Items.FindByValue(AUser.UserSex).Selected = true;
                }

            }
        }

        protected void btnExecute_Click(object sender, EventArgs e)
        {
            if (f("chkUserId") == "") return;
            string[] idValue = f("chkUserId").Split(',');
            string oper = ddlOper.SelectedValue;

            if (oper == "pass")
            {
                for (int i = 0; i < idValue.Length; i++)
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    doh.AddFieldItem("ispass", 1);
                    doh.Update("Xk_User");
                }
            }
            else if (oper == "nopass")
            {
                for (int i = 0; i < idValue.Length; i++)
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    doh.AddFieldItem("ispass", 0);
                    doh.Update("Xk_User");
                }
            }
            else if (oper == "move")
            {
                string uGrade = ddlGroup.SelectedValue;
                string uGroup = ddlGroup.SelectedItem.Text;
                for (int i = 0; i < idValue.Length; i++)
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    doh.AddFieldItem("userGrade", uGrade);
                    doh.AddFieldItem("userGroup", uGroup);
                    doh.Update("Xk_User");
                }
            }
            else if (oper == "del")
            {
                for (int i = 0; i < idValue.Length; i++)
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + idValue[i];
                    doh.Delete("Xk_User");
                }
                Cms.RegUser -= idValue.Length;
                doh.Reset();
                doh.AddFieldItem("RegUser", Cms.RegUser);
                doh.Update("Xk_User");
            }
            getListBox();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            getListBox();
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            UserInfo AUser = new UserInfo();
            AUser.Username = txtAddName.Text;
            AUser.UserMail = txtAddEmail.Text;
            AUser.Password = Tools.GetMD5(txtAddPass.Text);
            AUser.Nickname = txtAddNickName.Text;
            AUser.Userpoint = Validator.StrToInt(txtAddPoint.Text, 50);
            AUser.UserSex = rblAddSex.SelectedValue;
            AUser.IsPass = 1;
            AUser.UserGrade = Validator.StrToInt(ddlAddGroup.SelectedValue, 1);
            AUser.UserGroup = ddlAddGroup.SelectedItem.Text;
            if (AUser.Add(doh) == 0)
                Alert("此用户名已存在，请重新输入！");
            else
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + AUser.UserGrade.ToString();
                doh.Count("Xk_UserGroup", "UserTotal");
                SetCmsNum(1, true, 1);
                GetList();
            }
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 8, 1);
        }

        protected void btnSaveUser_Click(object sender, EventArgs e)
        {
            UserInfo AUser = new UserInfo(id, doh);
            AUser.Address = txtEditAddress.Text;
            if (txtEditPass.Text != "")
                AUser.Password = Tools.GetMD5(txtEditPass.Text);
            AUser.Charm = Validator.StrToInt(txtEditCharm.Text, 0);
            AUser.Experience = Validator.StrToInt(txtEditExperience.Text, 0);
            AUser.HomePage = "";
            AUser.IsPass = Validator.StrToInt(rblEditIsPass.SelectedValue, 0);
            AUser.Nickname = txtEditNickName.Text;
            AUser.Oicq = txtEditOicq.Text;
            if (txtEditQuestion.Text.Trim() != "" && txtEditAnswer.Text != "")
            {
                AUser.Question = txtEditQuestion.Text;
                AUser.Answer = Tools.GetMD5(txtEditAnswer.Text);
            }
            AUser.Phone = txtEditPhone.Text;
            AUser.Postcode = txtEditPostCode.Text;
            AUser.TrueName = txtEditTrueName.Text;
            AUser.UserFace = txtEditFace.Text;
            AUser.UserGrade = Validator.StrToInt(ddlEditGroup.SelectedValue, 1);
            AUser.UserGroup = ddlEditGroup.SelectedItem.Text;
            AUser.UserIDCard = txtEditUserIDCard.Text;
            AUser.UserMail = txtEditMail.Text;
            AUser.Userpoint = Validator.StrToInt(txtEditPoint.Text, 0);
            AUser.UserSex = rblEditSex.SelectedValue;

            AUser.Update(doh);

            GetList();
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value.ToString();
            string uGrade = doh.GetValue("Xk_User", "UserGrade").ToString();
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value.ToString();
            doh.Delete("Xk_User");
            doh.Reset();
            doh.ConditionExpress = "Grades=" + uGrade;
            doh.Substract("Xk_UserGroup", "UserTotal");
            SetCmsNum(1, false, 1);
            getListBox();
        }

        protected void AspNetPager1_PageChanged(object src, XkCms.Common.Web.PageChangedEventArgs e)
        {
            AspNetPager1.CurrentPageIndex = e.NewPageIndex;
            getListBox();
        }
    }
}
