using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.Collections;

namespace XkCms.WebForm.Controls
{
    public partial class Review : BasicPage
    {
        protected string _id;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (lblId.Text == "{$ChannelId$}") Response.End();
            _id = q("id");
            if (XkCms.Common.Utils.Validator.StrToInt(_id, 0) == 0)
                Response.Write("栏目Id不能为 0 ，是否从外部提交数据?");
            if (!IsPostBack)
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + lblId.Text;
                string ChannelType = doh.GetValue("Xk_Channel", "Type").ToString();
                doh.Reset();
                doh.ConditionExpress = "id=" + _id;
                string tit = doh.GetValue("xk_" + ChannelType, "title").ToString();
                ArrayList ContentList = new CreateHtml(false, doh, string.Empty).LoadOther(tit + " 评论");
                if (ContentList.Count == 1)
                    Response.Write(ContentList[0].ToString());
                else
                {
                    ltTop.Text = ContentList[0].ToString();
                    ltBottom.Text = ContentList[1].ToString();
                }
                AspNetPager1.PageSize = 30;
                GetList();
            }
        }

        protected void GetList()
        {
            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + lblId.Text + " and cid=" + _id;
            int rowCount = 0;
            rpList.DataSource = doh.GetDataTable("xk_review", "UserName,AddDate,Content,Ip", "AddDate", true, "id", AspNetPager1.CurrentPageIndex, 30, ref rowCount);
            rpList.DataBind();
            AspNetPager1.RecordCount = rowCount;
        }

        protected void AspNetPager1_PageChanged(object src, XkCms.Common.Web.PageChangedEventArgs e)
        {
            AspNetPager1.CurrentPageIndex = e.NewPageIndex;
            GetList();
        }
    }
}
