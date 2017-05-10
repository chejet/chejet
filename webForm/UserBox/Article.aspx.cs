using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using XkCms.WebForm.BaseFunction;
using XkCms.Common.Utils;

namespace XkCms.WebForm.UserBox
{
    public partial class Article : UserCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User_Nav(true);
        }

        protected override void EditBox()
        {
            if (GroupSetting[7] == "0" && id == 0)
            {
                ErrMsg = "对不起,您所在的用户组不允许添加文章!";
                ShowErrMsg();
            }
            else if (GroupSetting[8] == "0" && id > 0)
            {
                ErrMsg = "对不起,您所在的用户组不允许修改文章!";
                ShowErrMsg();
            }
            if (id == 0)
            {
                if (!Channel.IsPost)
                {
                    ErrMsg = "对不起,来频道不允许发表内容";
                    ShowErrMsg();
                }
                if (Validator.StrToInt(GroupSetting[10], 0) > 0 && Validator.StrToInt(UserToday[3], 0) >= Validator.StrToInt(GroupSetting[10], 0))
                {
                    ErrMsg = "您每天最多只能发布" + GroupSetting[10] + "篇文章！";
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
                ArticleInfo _Article = new ArticleInfo(id.ToString(), doh);
                txtContentAuthor.Text = _Article.Author;
                txtContentKeyWord.Text = _Article.KeyWord;
                txtImg.Text = _Article.Img;
                txtSubTitle.Text = _Article.SubTitle;
                txtTitle.Text = _Article.Title;
                FCKeditor1.Value = _Article.Content;
                if (ddlColumn.Items.FindByValue(_Article.ColumnId.ToString()) != null)
                {
                    ddlColumn.ClearSelection();
                    ddlColumn.Items.FindByValue(_Article.ColumnId.ToString()).Selected = true;
                }
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
            ArticleInfo _Article;
            if (id > 0)
                _Article = new ArticleInfo(id.ToString(), doh);
            else
                _Article = new ArticleInfo();
            _Article.Title = txtTitle.Text;
            _Article.SubTitle = txtSubTitle.Text;
            _Article.ChannelId = Channel.Id;
            _Article.ColumnId = int.Parse(ddlColumn.SelectedValue);
            _Article.Author = txtContentAuthor.Text;
            _Article.KeyWord = txtContentKeyWord.Text;
            _Article.Img = txtImg.Text;
            _Article.ColumnCode = _obj[0].ToString();
            _Article.ColumnName = _obj[1].ToString();
            _Article.Content = FCKeditor1.Value;
            _Article.UserId = UserId;
            if (GroupSetting[15] == "0")
            {
                _Article.IsPass = 1;
                UpdateUserToday(1, 9);
            }
            else
                _Article.IsPass = 0;
            if (id > 0)
                _Article.Update(doh);
            else
            {
                _Article.Add(doh);
            }
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
            gvList.DataSource = doh.GetDataTable("xk_article", "id,title,ispass,channelid", "adddate", true, "id", AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, ref rowCount);
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
            AspNetPager1.RecordCount = rowCount;
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (GroupSetting[8] == "0")
            {
                ErrMsg = "对不起,您所在的用户组不允许删除文章!";
                ShowErrMsg();
            }
            new ArticleInfo().Del(gvList.DataKeys[e.RowIndex].Value.ToString(), doh);
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
