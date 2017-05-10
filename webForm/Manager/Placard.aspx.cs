using System;
using System.Data;
using System.Web.UI.WebControls;
using XkCms.WebForm.BaseFunction;

namespace XkCms.WebForm.Manager
{
    public partial class Placard : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("", false, ref lblChannel);
            if (Channel.Id > 0)
                ChkPower(Channel.Id + "-9");
            else
                ChkPower("003");
        }

        protected override void getListBox()
        {
            plList.Visible = true;
            plEdit.Visible = false;
            int rowCount = 1;
            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + Channel.Id;
            gvList.DataSource = doh.GetDataTable("XK_Placard", "id,title", "id", true, "id", AspNetPager1.CurrentPageIndex, pageSize, ref rowCount);
            gvList.DataKeyNames = new string[] { "id" };
            gvList.PageSize = pageSize;
            gvList.DataBind();

            //…Ë÷√∑÷“≥
            AspNetPager1.PageSize = pageSize;
            AspNetPager1.RecordCount = rowCount;
            AspNetPager1.AlwaysShow = true;
        }

        protected override void editBox()
        {
            plList.Visible = false;
            plEdit.Visible = true;
            FCKeditor1.BasePath = Cms.EditorDir;
            FCKeditor1.ManagerPath = Cms.ManagerDir;
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_Placard", Button1);
            wh.AddBind(txtTitle, "title", true);
            wh.AddBind(FCKeditor1, "Value", "Content", true);
            wh.AddBind(Channel.Id.ToString(), "ChannelId", false);
            if (id == "0")
            {
                string addDate = DateTime.Now.ToString();
                wh.AddBind(addDate, "addTime", true);
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
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 2, 1);
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value;
            doh.Delete("XK_Placard");
            getListBox();
        }

        protected void AspNetPager1_PageChanged(object src, XkCms.Common.Web.PageChangedEventArgs e)
        {
            AspNetPager1.CurrentPageIndex = e.NewPageIndex;
            getListBox();
        }
    }
}
