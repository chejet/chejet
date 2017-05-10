using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class Vote : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("", false, ref lblChannel);
            if (Channel.Id > 0)
                ChkPower(Channel.Id + "-10");
            else
                ChkPower("004");
        }

        protected override void getListBox()
        {
            plList.Visible = true;
            plEdit.Visible = false;
            doh.Reset();
            doh.SqlCmd = "select id,title,votetotal,type,IsPass from xk_vote where ChannelId=" + Channel.Id + " order by id desc";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.PageSize = pageSize;
            gvList.DataBind();
        }

        protected override void editBox()
        {
            plList.Visible = false;
            plEdit.Visible = true;
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_Vote", Button1);
            wh.AddBind(txtTitle, "title", true);
            wh.AddBind(txtContent, "votetext", true);
            wh.AddBind(rblType, "SelectedValue", "type", false);
            wh.AddBind(Channel.Id.ToString(), "ChannelId", false);
            if (id == "0")
            {
                wh.Mode = XkCms.DataOper.OperationType.Add;
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
            int voteTotal = txtContent.Text.Split('|').Length;
            string voteNum = "0";
            for (int i = 0; i < voteTotal - 1; i++)
            {
                voteNum += "|0";
            }
            if (id == "0")
            {
                XkCms.DataOper.Data.DbOperEventArgs de = (XkCms.DataOper.Data.DbOperEventArgs)e;
                id = de.id.ToString();
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + id;
            doh.AddFieldItem("votenum", voteNum);
            doh.AddFieldItem("VoteTotal", 0);
            doh.AddFieldItem("IsPass", 1);
            doh.Update("xk_vote");
            GetList();
        }
        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            getListBox();
        }
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 4, 1);
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value;
            doh.Delete("xk_vote");
            getListBox();
        }
        protected void LinkButton3_Command(object sender, CommandEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + e.CommandArgument.ToString();
            doh.Audit("xk_vote", "IsPass");
            getListBox();
        }
        public string chkEnabled(string en)
        {
            if (en == "1")
                return "禁用";
            else
                return "<font color='red'>启用</a>";
        }
        public string chkThisType(string cType)
        {
            if (cType == "1")
                return "多选";
            else
                return "单选";
        }
    }
}
