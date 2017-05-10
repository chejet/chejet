using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.UserBox
{
    public partial class Soft : UserCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User_Nav(true);
        }

        protected override void EditBox()
        {
            if (GroupSetting[11] == "0" && id == 0)
            {
                ErrMsg = "对不起,您所在的用户组不允许添加软件!";
                ShowErrMsg();
            }
            else if (GroupSetting[12] == "0" && id > 0)
            {
                ErrMsg = "对不起,您所在的用户组不允许修改软件!";
                ShowErrMsg();
            }
            if (id == 0)
            {
                if (!Channel.IsPost)
                {
                    ErrMsg = "对不起,来频道不允许发表内容";
                    ShowErrMsg();
                }
                if (Validator.StrToInt(GroupSetting[14], 0) > 0 && Validator.StrToInt(UserToday[2], 0) >= Validator.StrToInt(GroupSetting[14], 0))
                {
                    ErrMsg = "您每天最多只能发布" + GroupSetting[10] + "个下载内容！";
                    ShowErrMsg();
                }
            }

            plEdit.Visible = true;
            plList.Visible = false;
            GetColumns(ref ddlColumn);
            txtContentAuthor.Text = UserName;
            FCKeditor1.BasePath = Cms.EditorDir;
            FCKeditor1.ManagerPath = Cms.Dir + "UserBox/";
            if (GroupSetting[2] == "0")
            {
                txtCheckCode.Enabled = false;
                txtCheckCode.Text = "0000";
            }
            if (id > 0)
            {
                SoftInfo _Soft = new SoftInfo(id.ToString(), doh);
                txtContentAuthor.Text = _Soft.Author;
                txtContentKeyWord.Text = _Soft.KeyWord;
                txtImg.Text = _Soft.Img;
                txtTitle.Text = _Soft.Title;
                FCKeditor1.Value = _Soft.Summary;
                hfDownUrl.Value = _Soft.DownUrl;
                ddlCopyrightType.ClearSelection();
                ddlCopyrightType.Items.FindByValue(_Soft.CopyrightType).Selected = true;
                ddlLanguage.ClearSelection();
                ddlLanguage.Items.FindByValue(_Soft.SLanguage).Selected = true;
                ddlSoftType.ClearSelection();
                ddlSoftType.Items.FindByValue(_Soft.SType).Selected = true;
                txtDemoUrl.Text = _Soft.DemoUrl;
                txtOperatingSystem.Text = _Soft.OperatingSystem;
                txtRegUrl.Text = _Soft.RegUrl;
                txtVersion.Text = _Soft.Version;
                txtPassword.Text = _Soft.UnZipPass;
                ddlColumn.ClearSelection();
                ddlColumn.Items.FindByValue(_Soft.ColumnId.ToString()).Selected = true;
                JsExe("初始化", "UrlHiddenToList()");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidateCode(txtCheckCode.Text) && GroupSetting[2] == "1")
            {
                Alert("验证码错误!");
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + ddlColumn.SelectedValue;
            object[] _obj = doh.GetValues("xk_column", "code,title");
            if (_obj == null)
            {
                Alert("请选择栏目!");
                return;
            }
            SoftInfo _Soft;
            if (id > 0)
                _Soft = new SoftInfo(id.ToString(), doh);
            else
                _Soft = new SoftInfo();
            _Soft.Title = txtTitle.Text;
            _Soft.ChannelId = Channel.Id;
            _Soft.ColumnId = int.Parse(ddlColumn.SelectedValue);
            _Soft.Author = txtContentAuthor.Text;
            _Soft.KeyWord = txtContentKeyWord.Text;
            _Soft.Img = txtImg.Text;
            _Soft.ColumnCode = _obj[0].ToString();
            _Soft.ColumnName = _obj[1].ToString();
            _Soft.Summary = FCKeditor1.Value;
            _Soft.UserId = UserId;
            _Soft.DownUrl = hfDownUrl.Value;
            _Soft.CopyrightType = ddlCopyrightType.SelectedValue;
            _Soft.DemoUrl = txtDemoUrl.Text;
            _Soft.OperatingSystem = txtOperatingSystem.Text;
            _Soft.RegUrl = txtRegUrl.Text;
            _Soft.SLanguage = ddlLanguage.SelectedValue;
            _Soft.SType = ddlSoftType.SelectedValue;
            _Soft.UnZipPass = txtPassword.Text;
            _Soft.Version = txtVersion.Text;
            if (GroupSetting[15] == "0")
            {
                _Soft.IsPass = 1;
                UpdateUserToday(2, 13);
            }
            else
                _Soft.IsPass = 0;
            if (id > 0)
                _Soft.Update(doh);
            else
                _Soft.Add(doh);
            GetList();
        }

        protected override void GetList()
        {
            plEdit.Visible = false;
            plList.Visible = true;
            AspNetPager1.PageSize = 10;
            int rowCount = 0;
            doh.Reset();
            doh.ConditionExpress = "UserId=" + UserId;
            gvList.DataSource = doh.GetDataTable("xk_Soft", "id,title,ispass,channelid", "adddate", true, "id", AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, ref rowCount);
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
            AspNetPager1.RecordCount = rowCount;
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (GroupSetting[12] == "0")
            {
                ErrMsg = "对不起,您所在的用户组不允许删除软件!";
                ShowErrMsg();
            }
            new SoftInfo().Del(gvList.DataKeys[e.RowIndex].Value.ToString(), doh);
            GetList();
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 2, 1);
        }

        protected void AspNetPager1_PageChanged(object src, XkCms.Common.Web.PageChangedEventArgs e)
        {
            AspNetPager1.CurrentPageIndex = e.NewPageIndex;
            GetList();
        }
    }
}
