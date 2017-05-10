using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class Friend : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("", false, ref lblChannel);
            if (Channel.Id > 0)
                ChkPower(Channel.Id + "-8");
            else
                ChkPower("002");
        }
        protected override void getListBox()
        {
            plList.Visible = true;
            plEdit.Visible = false;
            int rowCount = 1;
            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + Channel.Id;
            gvList.DataSource = doh.GetDataTable("Xk_FriendLink", "id,linkname,linkurl,ordernum,ispass", "id", true, "id", AspNetPager1.CurrentPageIndex, pageSize, ref rowCount);
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
            doh.Reset();
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_FriendLink", Button1);       
            wh.AddBind(txtName, "LinkName", true);
            wh.AddBind(txtUrl, "LinkURL", true);
            wh.AddBind(txtImg, "LinkImgPath", true);
            wh.AddBind(txtInfo, "LinkInfo", true);
            wh.AddBind(Channel.Id.ToString(), "ChannelId", false);
            wh.AddBind(txtSort, "OrderNum", false);
            wh.AddBind(rblStata, "SelectedValue", "IsPass", false);
            wh.AddBind(rblStyle, "SelectedValue", "Style", false);
            wh.AddBind(txtUserName, "UserName", true);
            string temp = "0";
            wh.AddBind(temp, "UserId", false);
            if (id != "0")
            {
                wh.ConditionExpress = "id=" + id.ToString();
                wh.Mode = XkCms.DataOper.OperationType.Modify;
            }
            else
            {
                txtUserName.Text = MasterName;
                wh.Mode = XkCms.DataOper.OperationType.Add;
            }
            wh.AddOk += new EventHandler(save_ok);
            wh.ModifyOk += new EventHandler(save_ok);
        }
        protected void save_ok(object sender, EventArgs e)
        {
            GetList();
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value.ToString();
            doh.Delete("Xk_FriendLink");
            getListBox();
        }
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 4, 1);
        }

        protected void AspNetPager1_PageChanged(object src, XkCms.Common.Web.PageChangedEventArgs e)
        {
            AspNetPager1.CurrentPageIndex = e.NewPageIndex;
            getListBox();
        }
    }
}
