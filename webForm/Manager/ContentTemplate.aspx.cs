using System;
using System.Data;
using System.Web;
using System.Xml;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using XkCms.Common.Utils;
using System.IO;

namespace XkCms.WebForm.Manager
{
    public partial class ContentTemplate : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("007");
        }

        protected override void getListBox()
        {
            plEdit.Visible = false;
            plList.Visible = true;
            doh.Reset();
            doh.SqlCmd = "select id,title from Xk_ContentTemplate order by id desc";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
        }

        protected override void editBox()
        {
            plList.Visible = false;
            plEdit.Visible = true;
            FCKeditor1.BasePath = Cms.EditorDir;
            FCKeditor1.ManagerPath = Cms.ManagerDir;
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_ContentTemplate", Button1);
            wh.AddBind(txtName, "title", true);
            wh.AddBind(txtDescription, "Description", true);
            wh.AddBind(FCKeditor1, "Value", "content", true);
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
            GvRowAddFun(ref e, 2, 1);
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + gvList.DataKeys[e.RowIndex].Value.ToString();
            doh.Delete("Xk_ContentTemplate");
            getListBox();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            doh.Reset();
            doh.SqlCmd = "select title,Description,content from Xk_ContentTemplate order by id desc";
            DataTable dt = doh.GetDataTable();
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlNode node = XmlUtil.AppendElement(xml, "Templates");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlNode temp = XmlUtil.AppendElement(node, "Template");
                XmlUtil.SetAttribute(temp, "title", dt.Rows[i][0].ToString());
                XmlUtil.AppendElement(temp, "Description", dt.Rows[i][1].ToString());
                XmlNode html = XmlUtil.AppendElement(temp, "Html");
                html.AppendChild(html.OwnerDocument.CreateCDataSection(dt.Rows[i][2].ToString()));
            }
            StreamWriter sw = File.CreateText(Path.Combine(Server.MapPath(Cms.ManagerDir), "ContentTemplate.xml"));
            sw.Write(XmlUtil.FormatXml(xml));
            sw.Close();
            Alert("更新完成");
        }
    }
}
