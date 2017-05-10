using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class Template : Admin
    {
        public string pId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            pId = q("pid");
            if (!XkCms.Common.Utils.Validator.IsNumberId(pId))
            {
                Alert("参数错误,请不要从外部提交数据!");
                btnAdd.Enabled = false;
                return;
            }
            if (!IsPostBack)
            {
                doh.Reset();
                doh.SqlCmd = "select id,title from Xk_TempProject";
                DataTable dt = doh.GetDataTable();
                ddlProject.DataSource = dt;
                ddlProject.DataTextField = "title";
                ddlProject.DataValueField = "id";
                ddlProject.DataBind();
                ddlProject.Items.FindByValue(pId).Selected = true;
            }
            Xkzi_Load("007");
            hlAdd.NavigateUrl = "Template.aspx?action=add&pid=" + pId;
            if (q("action") == "set")
                setDefault();
        }

        protected override void editBox()
        {
            plList.Visible = false;
            plAdd.Visible = true;
            lblProject.Text = ddlProject.SelectedItem.Text;
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_Template", btnAdd);
            wh.AddBind(txtName, "title", true);
            wh.AddBind(ddlType, "type", true);
            wh.AddBind(ddlSType, "stype", true);
            wh.AddBind(ddlProject, "pId", false);
            wh.AddBind(txtSource, "source", true);
            if (id == "0")
            {
                wh.AddBind("0", "isDefault", false);
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
            wh.BindBeforeModifyOk += new EventHandler(bind_ok);
        }
        protected bool chkName()
        {
            doh.Reset();
            doh.ConditionExpress = "Title=@title and id<>@id";
            doh.AddConditionParameter("@title", txtName.Text);
            doh.AddConditionParameter("@id", id);
            if (doh.Exist("Xk_Template"))
            {
                Alert("模板名称重复！");
                return false;
            }
            return true;
        }
        protected void bind_ok(object sender, EventArgs e)
        {
            if (id != "0")
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + id;
                if (doh.GetValue("xk_template", "isdefault").ToString() == "1")
                {
                    ddlSType.Enabled = false;
                    ddlType.Enabled = false;
                    Alert("默认模板,不能修改模板类型!");
                }
            }
            ddlType_SelectedIndexChanged(null, null);
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
            doh.SqlCmd = "select id,title,stype,isDefault,[type],source from [Xk_Template] where pId = " + pId;
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
        }
        protected string chkTempType(string Type, string sType)
        {
            string TempStr = string.Empty;
            switch (Type)
            {
                case "System":
                    TempStr = "<font color='red'>系统</font> - ";
                    break;
                case "Article":
                    TempStr = "<font color='blue'>文章</font> - ";
                    break;
                case "Soft":
                    TempStr = "<font color='green'>下载</font> - ";
                    break;
                case "Photo":
                    TempStr = "<font color='#008080'>图片</font> - ";
                    break;
                case "Diss":
                    TempStr = "<font color='#cc0000'>专题</font> - ";
                    break;
                case "User":
                    TempStr = "<font color='#FF00F4'>用户</font> - ";
                    break;
                case "Other":
                    TempStr = "其它 - ";
                    break;
            }
            switch (sType)
            {
                case "Column":
                    TempStr += "列表";
                    break;
                case "Content":
                    TempStr += "内容";
                    break;
                case "MoreDiss":
                    TempStr += "过往专题";
                    break;
                case "DissList":
                    TempStr += "内容列表";
                    break;
                case "RegLogin":
                    TempStr += "注册登陆";
                    break;
                case "UserBox":
                    TempStr += "用户中心";
                    break;
                case "Friend":
                    TempStr += "友情链接";
                    break;
                case "Placard":
                    TempStr += "站内公告";
                    break;
                case "Other":
                    TempStr += "其它信息";
                    break;
                default:
                    TempStr += "首页";
                    break;
            }
            return TempStr;
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 4, 1);
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string tId = gvList.DataKeys[e.RowIndex].Value.ToString();
            string TempType = string.Empty;
            string TempSType = string.Empty;
            bool isUsing = false;
            doh.Reset();
            doh.ConditionExpress = "id=" + tId;
            TempSType = doh.GetValue("Xk_Template", "sType").ToString();
            if (doh.GetValue("Xk_Template", "isDefault").ToString() == "1")
                isUsing = true;
            else
            {
                if (TempSType == "DissList")
                {
                    doh.Reset();
                    doh.SqlCmd = "select id from xk_diss where TemplateId=" + tId;
                    if (doh.GetDataTable().Rows.Count > 0)
                        isUsing = true;
                }
                else if (TempSType == "Channel" || TempSType == "MoreDiss")
                {
                    doh.Reset();
                    doh.SqlCmd = "select id from xk_channel where TemplateId=" + tId + " or TemplateDiss=" + tId;
                    if (doh.GetDataTable().Rows.Count > 0)
                        isUsing = true;
                }
                else
                {
                    doh.Reset();
                    doh.SqlCmd = "select id from xk_Column where TemplateId=" + tId + " or ContentTemp=" + tId;
                    if (doh.GetDataTable().Rows.Count > 0)
                        isUsing = true;
                }
            }
            if (isUsing)
                Alert("正在使用或默认模板不允许删除!");
            else
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + tId;
                doh.Delete("Xk_Template");
            }
            getListBox();
        }
        public string isDefault(string type, string isType, string isd, string tid)
        {
            if (isd == "0")
                return "<a href='?action=set&pid=" + pId + "&stype=" + isType + "&type=" + type + "&id=" + tid + "'>设为默认</a>";
            else
                return "<font color='#cccccc'>设为默认</a>";
        }
        private void setDefault()
        {
            string cType = q("stype");
            string Type = q("type");
            if (cType == "")
                return;
            if (Type == "")
                return;
            doh.Reset();
            doh.ConditionExpress = "stype=@stype and type=@type and isDefault=1";
            doh.AddConditionParameter("@stype", cType);
            doh.AddConditionParameter("@type", Type);
            doh.AddFieldItem("isDefault", 0);
            doh.Update("xk_Template");
            doh.Reset();
            doh.ConditionExpress = "id=" + id;
            doh.AddFieldItem("isDefault", 1);
            doh.Update("xk_Template");
            doh.Reset();
            getListBox();
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("Template.aspx?pid=" + ddlProject.SelectedValue);
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem li in ddlSType.Items)
                li.Enabled = false;
            if (ddlType.SelectedValue == "System")
            {
                ddlSType.Items.FindByValue("Index").Enabled = true;
            }
            else if (ddlType.SelectedValue == "Diss")
            {
                ddlSType.Items.FindByValue("MoreDiss").Enabled = true;
                ddlSType.Items.FindByValue("DissList").Enabled = true;
            }
            else if (ddlType.SelectedValue == "User")
            {
                ddlSType.Items.FindByValue("RegLogin").Enabled = true;
                ddlSType.Items.FindByValue("UserBox").Enabled = true;
            }
            else if (ddlType.SelectedValue == "Message")
            {
                ddlSType.Items.FindByValue("MsgIndex").Enabled = true;
                ddlSType.Items.FindByValue("MsgReply").Enabled = true;
                ddlSType.Items.FindByValue("MsgPost").Enabled = true;
            }
            else if (ddlType.SelectedValue == "Other")
            {
                ddlSType.Items.FindByValue("Friend").Enabled = true;
                ddlSType.Items.FindByValue("Placard").Enabled = true;
                ddlSType.Items.FindByValue("Other").Enabled = true;
            }
            else
            {
                ddlSType.Items.FindByValue("Channel").Enabled = true;
                ddlSType.Items.FindByValue("Column").Enabled = true;
                ddlSType.Items.FindByValue("Content").Enabled = true;
            }
        }
    }
}
