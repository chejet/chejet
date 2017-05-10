using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.IO;

namespace XkCms.WebForm.Manager
{
    public partial class Diss : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("", true, ref lblChannel);
            ChkPower(Channel.Id + "-6");
        }

        protected override void editBox()
        {
            plEdit.Visible = true;
            plList.Visible = false;
            FCKeditor1.BasePath = Cms.EditorDir;
            FCKeditor1.ManagerPath = Cms.ManagerDir;
            if (!IsPostBack)
            {
                doh.Reset();
                doh.SqlCmd = "select id,title from [xk_template] where sType='DissList'";
                DataTable dt = doh.GetDataTable();
                if (dt.Rows.Count < 1)
                {
                    Alert("未找到模板，请先添加模板!");
                    btnSaveColumn.Enabled = false;
                }
                ddlTemplate.Items.Add(new ListItem("默认模板", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlTemplate.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                }
            }
            doh.Reset();
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_Diss", btnSaveColumn);
            wh.AddBind(txtName, "title", true);
            wh.AddBind(FCKeditor1, "Value", "Info", true);
            wh.AddBind(chkIsTop, "1", "isTop", false);
            wh.AddBind(Channel.Id.ToString(), "ChannelId", false);
            wh.AddBind(txtImg, "img", true);
            wh.AddBind(ddlTemplate, "TemplateId", false);
            wh.AddBind(txtPageSize, "pagesize", false);
            if (id == "0")
                wh.Mode = XkCms.DataOper.OperationType.Add;
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

        protected override void getListBox()
        {
            plEdit.Visible = false;
            plList.Visible = true;
            doh.Reset();
            doh.SqlCmd = "select id,title,ChannelId,isTop from [Xk_Diss] where ChannelId=" + Channel.Id + " order by id";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
        }
        public string chkTop(string isTop)
        {
            if (isTop == "1")
                return "<font color='green'>是</fong>";
            else
                return "否";
        }
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 3, 1);
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value.ToString();
            doh.Delete("Xk_Diss");
            if (Directory.Exists(Request.PhysicalApplicationPath + Channel.Dir + "\\List"))
            {
                string[] htmFiles = Directory.GetFiles(Request.PhysicalApplicationPath + Channel.Dir + "\\List", "Diss_" + gvList.DataKeys[e.RowIndex].Value.ToString() + "_*.htm");
                foreach (string fileName in htmFiles)
                {
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }
            }
            getListBox();
        }
    }
}
