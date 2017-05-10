using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using XkCms.Common.Utils;

namespace XkCms.WebForm.Manager
{
    public partial class Review : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("", true, ref lblChannel);
            ChkPower(Channel.Id + "-5");
        }

        protected override void getListBox()
        {
            plEdit.Visible = false;
            plList.Visible = true;

            int countNum = 0;
            AspNetPager1.PageSize = pageSize;

            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + Channel.Id;
            gvReviewList.DataSource = doh.GetDataTable("Xk_Review", "id,content,UserName,addDate,cId", "id", true, "id", 1, pageSize, ref countNum);
            gvReviewList.DataKeyNames = new string[] { "id" };
            gvReviewList.DataBind();
            AspNetPager1.RecordCount = countNum;
        }

        protected void gvReviewList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 4, 3);
        }
        protected void gvReviewList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.SqlCmd = "select cid from xk_review where id=" + gvReviewList.DataKeys[e.RowIndex].Value.ToString();
            DataTable dt = doh.GetDataTable();
            string contentId = string.Empty;
            if (dt.Rows.Count > 0)
            {
                contentId = dt.Rows[0][1].ToString();
            }
            else
                return;
            doh.Reset();
            doh.ConditionExpress = "id=" + contentId;
            doh.Substract("xk_" + Channel.Type, "reviewnum");
            doh.Reset();
            doh.ConditionExpress = "id=" + Channel.Id;
            doh.Substract("xk_channel", "reviewnum");
            doh.Reset();
            doh.Substract("xk_system", "reviewnum");
            doh.Reset();
            doh.ConditionExpress = "id=" + gvReviewList.DataKeys[e.RowIndex].Value.ToString();
            doh.Delete("xk_review");
            getListBox();
        }
        protected void gvReviewList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            plEdit.Visible = true;
            plList.Visible = false;
            doh.Reset();
            doh.ConditionExpress = "id=" + gvReviewList.DataKeys[e.NewEditIndex].Value;
            txtReviewContent.Text = doh.GetValue("xk_review", "content").ToString();
            hfReviewId.Value = gvReviewList.DataKeys[e.NewEditIndex].Value.ToString();
        }
        protected void btnSaveReview_Click(object sender, EventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + hfReviewId.Value;
            doh.AddFieldItem("content", txtReviewContent.Text);
            doh.Update("xk_review");
            getListBox();
        }

        protected void AspNetPager1_PageChanged(object src, XkCms.Common.Web.PageChangedEventArgs e)
        {
            AspNetPager1.CurrentPageIndex = e.NewPageIndex;
            getListBox();
        }

        protected string ClipContent(string cont)
        {
            return Tools.ClipString(cont, 60);
        }
    }
}
