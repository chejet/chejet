using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class Article : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("", true, ref lblChannel);
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

        protected override void editBox()
        {
            plEdit.Visible = true;
            plList.Visible = false;
            GetEditDropDownList(ref ddlTcolor, ref ddlContentColumn, ref ddlSource, ref ddlDiss, ref FCKeditor1);
            doh.Reset();
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_Article", btnSaveContent);
            wh.AddBind(txtContentTitle, "title", true);
            wh.AddBind(txtSubTitle, "subtitle", true);
            wh.AddBind(ddlContentColumn, "columnId", false);
            wh.AddBind(ddlDiss, "disid", false);
            wh.AddBind(ddlSource, "sourceid", false);
            wh.AddBind(txtContentAuthor, "author", true);
            wh.AddBind(txtImg, "img", true);
            wh.AddBind(rbtnContentTop, "SelectedValue", "istop", false);
            wh.AddBind(txtContentUrl, "outurl", true);
            wh.AddBind(chkIsOut, "1", "isout", false);
            wh.AddBind(chkIsEdit, "1", "ispass", false);
            wh.AddBind(txtViewNum, "viewnum", false);
            wh.AddBind(FCKeditor1, "Value", "content", true);
            wh.AddBind(txtSummary, "summary", true);
            wh.AddBind(Channel.Id.ToString(), "ChannelId", false);
            wh.AddBind(ddlTcolor, "tcolor", true);
            wh.AddBind(HiddenFieldCorrelationIDs, "Value", "Correlation", true);
            wh.AddBind(txtContentKeyWord, "KeyWord", true);
            wh.AddBind(txtAddDate, "AddDate", true);
            if (id == "0")
            {
                wh.Mode = XkCms.DataOper.OperationType.Add;
                chkIsEdit.Checked = true;
                txtAddDate.Text = DateTime.Now.ToString();
            }
            else
            {
                wh.ConditionExpress = "id=" + id;
                wh.Mode = XkCms.DataOper.OperationType.Modify;
            }
            wh.AddOk += new EventHandler(save_ok);
            wh.ModifyOk += new EventHandler(save_ok);
        }
        protected void save_ok(object sender, EventArgs e)
        {
            DataAdded(ddlContentColumn.SelectedValue, txtImg, e);
            GetList();
        }
        protected void gvArticleList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 6, 1);
        }
        protected void gvArticleList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataDeled(gvArticleList.DataKeys[e.RowIndex].Value.ToString());
            getListBox();
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
    }
}
