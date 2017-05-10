using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class Source : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("", false, ref lblChannel);
            if (Channel.Id > 0)
                ChkPower(Channel.Id + "-7");
            else
                ChkPower("005");
        }

        protected override void getListBox()
        {
            plEdit.Visible = false;
            plList.Visible = true;
            doh.Reset();
            doh.SqlCmd = "select id,title,url from Xk_Source where ChannelId=" + Channel.Id;
            doh.SqlCmd += " order by id desc";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
        }

        protected override void editBox()
        {
            plList.Visible = false;
            plEdit.Visible = true;
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_Source", Button1);
            wh.AddBind(txtName, "title", true);
            wh.AddBind(txtUrl, "url", true);
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
            GetList();
        }
        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            getListBox();
        }
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 3, 1);
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value.ToString();
            doh.Delete("Xk_Source");
            getListBox();
        }
    }
}
