using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.UserBox
{
    public partial class Photo : UserCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User_Nav(true);
        }

        protected override void EditBox()
        {
            if (GroupSetting[35] == "0" && id == 0)
            {
                ErrMsg = "对不起,您所在的用户组不允许添加图片!";
                ShowErrMsg();
            }
            else if (GroupSetting[36] == "0" && id > 0)
            {
                ErrMsg = "对不起,您所在的用户组不允许修改图片!";
                ShowErrMsg();
            }
            if (id == 0)
            {
                if (!Channel.IsPost)
                {
                    ErrMsg = "对不起,来频道不允许发表内容";
                    ShowErrMsg();
                }
                if (Validator.StrToInt(GroupSetting[38], 0) > 0 && Validator.StrToInt(UserToday[2], 0) >= Validator.StrToInt(GroupSetting[38], 0))
                {
                    ErrMsg = "您每天最多只能发布" + GroupSetting[10] + "个图片内容！";
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
                PhotoInfo _Photo = new PhotoInfo(id.ToString(), doh);
                txtContentAuthor.Text = _Photo.Author;
                txtContentKeyWord.Text = _Photo.KeyWord;
                txtImg.Text = _Photo.Img;
                txtTitle.Text = _Photo.Title;
                FCKeditor1.Value = _Photo.Summary;
                hfDownUrl.Value = _Photo.PhotoUrl;
                if (ddlColumn.Items.FindByValue(_Photo.ColumnId.ToString()) != null)
                {
                    ddlColumn.ClearSelection();
                    ddlColumn.Items.FindByValue(_Photo.ColumnId.ToString()).Selected = true;
                }
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
            PhotoInfo _Photo;
            if (id > 0)
                _Photo = new PhotoInfo(id.ToString(), doh);
            else
                _Photo = new PhotoInfo();
            _Photo.Title = txtTitle.Text;
            _Photo.ChannelId = Channel.Id;
            _Photo.ColumnId = int.Parse(ddlColumn.SelectedValue);
            _Photo.Author = txtContentAuthor.Text;
            _Photo.KeyWord = txtContentKeyWord.Text;
            _Photo.Img = txtImg.Text;
            _Photo.ColumnCode = _obj[0].ToString();
            _Photo.ColumnName = _obj[1].ToString();
            _Photo.Summary = FCKeditor1.Value;
            _Photo.UserId = UserId;
            _Photo.PhotoUrl = hfDownUrl.Value;
            if (GroupSetting[15] == "0")
            {
                _Photo.IsPass = 1;
                UpdateUserToday(3, 37);
            }
            else
                _Photo.IsPass = 0;
            if (id > 0)
                _Photo.Update(doh);
            else
                _Photo.Add(doh);
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
            gvList.DataSource = doh.GetDataTable("xk_Photo", "id,title,ispass,channelid", "adddate", true, "id", AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, ref rowCount);
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
            AspNetPager1.RecordCount = rowCount;
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (GroupSetting[36] == "0")
            {
                ErrMsg = "对不起,您所在的用户组不允许删除图片!";
                ShowErrMsg();
            }
            new PhotoInfo().Del(gvList.DataKeys[e.RowIndex].Value.ToString(), doh);
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
