using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class TempProject : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("007");
            if (q("action") == "set")
                setDefault();
        }
        protected override void editBox()
        {
            plList.Visible = false;
            plAdd.Visible = true;

            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_TempProject", btnAdd);
            wh.AddBind(txtName, "title", true);
            wh.AddBind(txtDir, "dir", true);
            wh.AddBind(txtInfo, "info", true);
            if (id == "0")
            {
                wh.AddBind("0", "isDefault", true);
                wh.Mode = XkCms.DataOper.OperationType.Add;
            }
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
            if (doh.Exist("Xk_TempProject"))
            {
                Alert("方案名重复!");
                return false;
            }
            return true;
        }
        protected void save_ok(object sender, EventArgs e)
        {
            if (id == "0")
            {
                XkCms.DataOper.Data.DbOperEventArgs de = (XkCms.DataOper.Data.DbOperEventArgs)e;
                string[] _tempTitle = new string[] { "首页", "文章首页", "文章列表", "文章内容", "下载首页", "下载列表", "下载内容", "图片首页", "图片列表", "图片内容", "过往专题", "专题显示", "注册登陆", "用户中心", "友情链接", "站内公告", "其它信息" };
                string[] _tempType = new string[] { "System", "Article", "Article", "Article", "Soft", "Soft", "Soft", "Photo", "Photo", "Photo", "Diss", "Diss", "User", "User", "Other", "Other", "Other" };
                string[] _tempStype = new string[] { "Index", "Channel", "Column", "Content", "Channel", "Column", "Content", "Channel", "Column", "Content", "MoreDiss", "DissList", "RegLogin", "UserBox", "Friend", "Placard", "Other" };
                for (int i = 0; i < _tempTitle.Length; i++)
                {
                    doh.Reset();
                    doh.AddFieldItem("title", txtName.Text + _tempTitle[i]);
                    doh.AddFieldItem("type", _tempType[i]);
                    doh.AddFieldItem("stype", _tempStype[i]);
                    doh.AddFieldItem("source", _tempType[i] + "_" + _tempStype[i] + ".htm");
                    doh.AddFieldItem("pid", de.id);
                    doh.AddFieldItem("isdefault", 0);
                    doh.Insert("xk_template");
                }

                Alert("请把模板文件复制到/Template/" + txtDir.Text + "/下");
            }
            GetList();
        }
        protected override void getListBox()
        {
            plList.Visible = true;
            plAdd.Visible = false;
            doh.Reset();
            doh.SqlCmd = "select id,title,info,isDefault from xk_tempProject";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 3, 1);
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string tId = gvList.DataKeys[e.RowIndex].Value.ToString();
            doh.Reset();
            doh.ConditionExpress = "pid=@id";
            doh.AddConditionParameter("@id", tId);
            if (doh.Exist("xk_template"))
            {
                Alert("请先删除方案下的所有模板！");
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "id=@id and isdefault=1";
            doh.AddConditionParameter("@id", tId);
            if (doh.Exist("xk_tempProject"))
            {
                Alert("默认模板不允许删除！");
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "id=@id";
            doh.AddConditionParameter("@id", tId);
            doh.Delete("Xk_TempProject");
            getListBox();
        }
        public string isDefault(string isd, string sid)
        {
            if (isd == "0")
                return "<a href='?action=set&id=" + sid + "' onclick=\"return confirm('把此方案中的所有模板设为默认,确定吗?')\">设为默认</a>";
            else
                return "<font color='#cccccc'>设为默认</a>";
        }
        private void setDefault()
        {
            doh.Reset();
            doh.AddFieldItem("isDefault", 0);
            doh.Update("Xk_TempProject");
            doh.Reset();
            doh.ConditionExpress = "id=" + id;
            doh.AddFieldItem("isDefault", 1);
            doh.Update("Xk_TempProject");
            doh.Reset();
            doh.AddFieldItem("isDefault", 0);
            doh.Update("xk_Template");
            doh.Reset();
            doh.ConditionExpress = "pid=" + id;
            doh.AddFieldItem("isDefault", 1);
            doh.Update("xk_Template");
            getListBox();
        }
    }
}
