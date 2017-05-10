using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class TempLabel : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("007");
        }

        protected override void editBox()
        {
            plList.Visible = false;
            plAdd.Visible = true;
            if (!IsPostBack)
            {
                doh.Reset();
                doh.SqlCmd = "select id,title from Xk_TempProject";
                ddlTempProject.DataSource = doh.GetDataTable();
                ddlTempProject.DataTextField = "title";
                ddlTempProject.DataValueField = "id";
                ddlTempProject.DataBind();
            }
            
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_TempLabel", btnAdd);
            wh.AddBind(txtName, "title", true);
            wh.AddBind(ddlTempProject, "pId", false);
            wh.AddBind(txtSource, "source", true);
            wh.AddBind(txtInfo, "info", true);
            wh.AddBind(txtSort, "sort", false);
            if (id == "0")
                wh.Mode = XkCms.DataOper.OperationType.Add;
            else
            {
                wh.ConditionExpress = "id=" + id;
                wh.Mode = XkCms.DataOper.OperationType.Modify;
            }
            wh.validator = chkName;
            wh.AddOk += new EventHandler(save_ok);
            wh.ModifyOk += new EventHandler(save_ok);
        }
        protected bool chkName()
        {
            doh.Reset();
            doh.ConditionExpress = "Title=@title and id<>@id";
            doh.AddConditionParameter("@title", txtName.Text);
            doh.AddConditionParameter("@id", id);
            if (doh.Exist("Xk_TempLabel"))
            {
                Alert("±Í«©√˚÷ÿ∏¥!");
                return false;
            }
            return true;
        }
        protected void save_ok(object sender, EventArgs e)
        {
            GetList();
        }
        protected override void getListBox()
        {
            plList.Visible = true;
            plAdd.Visible = false;
            doh.Reset();
            doh.SqlCmd = "select a.id as id,a.title as title,b.title as pTitle,a.info as info,a.sort as sort from [Xk_TempLabel] as a,[Xk_TempProject] as b where a.pId = b.id";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 5, 1);
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string tId = gvList.DataKeys[e.RowIndex].Value.ToString();
            doh.Reset();
            doh.ConditionExpress = "id=@id";
            doh.AddConditionParameter("@id", tId);
            doh.Delete("Xk_TempLabel");
            getListBox();
        }
    }
}
