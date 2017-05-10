using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class Soft : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("", true, ref lblChannel);
        }

        protected override void editBox()
        {
            plEdit.Visible = true;
            plList.Visible = false;
            string id = q("id") == "" ? "0" : q("id");

            GetEditDropDownList(ref ddlTcolor, ref ddlContentColumn, ref ddlSource, ref ddlDiss, ref FCKeditor1);
            doh.Reset();
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_Soft", btnSaveContent);
            wh.AddBind(txtContentTitle, "title", true);
            wh.AddBind(ddlContentColumn, "columnId", false);
            wh.AddBind(ddlDiss, "disid", false);
            wh.AddBind(ddlSource, "sourceid", false);
            wh.AddBind(txtContentAuthor, "author", true);
            wh.AddBind(txtImg, "img", true);
            wh.AddBind(rbtnContentTop, "SelectedValue", "istop", false);
            wh.AddBind(chkIsEdit, "1", "ispass", false);
            wh.AddBind(txtViewNum, "viewnum", false);
            wh.AddBind(FCKeditor1, "Value", "summary", true);
            wh.AddBind(Channel.Id.ToString(), "ChannelId", false);
            wh.AddBind(ddlTcolor, "tcolor", true);
            wh.AddBind(ddlSoftType, "stype", true);
            wh.AddBind(ddlLanguage, "slanguage", true);
            wh.AddBind(txtVersion, "version", true);
            wh.AddBind(ddlCopyrightType, "CopyrightType", true);
            wh.AddBind(txtPassword, "UnZipPass", true);
            wh.AddBind(txtDemoUrl, "demourl", true);
            wh.AddBind(txtRegUrl, "regurl", true);
            wh.AddBind(hfDownUrl, "Value", "downurl", true);
            wh.AddBind(txtOperatingSystem, "OperatingSystem", true);
            wh.AddBind(txtContentKeyWord, "KeyWord", true);
            wh.AddBind(txtAddDate, "AddDate", true);
            txtAddDate.Text = DateTime.Now.ToString();
            if (id == "0")
            {
                wh.Mode = XkCms.DataOper.OperationType.Add;
                chkIsEdit.Checked = true;
            }
            else
            {
                wh.ConditionExpress = "id=" + id;
                wh.Mode = XkCms.DataOper.OperationType.Modify;
            }
            wh.BindBeforeModifyOk += new EventHandler(bind_ok);
            wh.AddOk += new EventHandler(save_ok);
            wh.ModifyOk += new EventHandler(save_ok);
            wh.validator = chkName;
        }
        private bool chkName()
        {
            if (FCKeditor1.Value == "")
            {
                Alert("«ÎÃÓ–¥ºÚΩÈ!");
                return false;
            }
            if (hfDownUrl.Value == "")
            {
                Alert("«ÎÃÌº”œ¬‘ÿµÿ÷∑!");
                return false;
            }
            return true;
        }
        protected void save_ok(object sender, EventArgs e)
        {
            DataAdded(ddlContentColumn.SelectedValue, txtImg, e);
            GetList();
        }
        protected void bind_ok(object sender, EventArgs e)
        {
            JsExe("≥ı ºªØ", "UrlHiddenToList()");
        }
        protected override void getListBox()
        {
            plEdit.Visible = false;
            plList.Visible = true;
            int cid = Convert.ToInt32("0" + q("cid"));
            string keyType = q("k");
            string keyWord = q("w");
            int isEdit = q("e") == "" ? 2 : Convert.ToInt32(q("e"));
            int page = Convert.ToInt32("0" + q("page"));
            string sDate = q("d");
            GetContentList(cid, keyType, keyWord, sDate, isEdit, page, ref gvArticleList, ref ddlColumn, ref ddlKeyColumn, ref ltContentPager);
        }
        protected void btnExecute_Click(object sender, EventArgs e)
        {
            if (f("chkContentId") == "") return;
            BatchContent(ddlOper.SelectedValue, f("chkContentId"), Convert.ToInt32(ddlColumn.SelectedValue));
            getListBox();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetContentList(Convert.ToInt32(ddlKeyColumn.SelectedValue), ddlKeyType.SelectedValue, txtKeyWord.Text, "", 2, 1, ref gvArticleList, ref ddlColumn, ref ddlKeyColumn, ref ltContentPager);
        }
        protected void gvArticleList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataDeled(gvArticleList.DataKeys[e.RowIndex].Value.ToString());
            getListBox();
        }
        protected void gvArticleList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 6, 1);
        }
    }
}
