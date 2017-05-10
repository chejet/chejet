using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.IO;
using XkCms.Common.Utils;

namespace XkCms.WebForm.Manager
{
    public partial class Channel : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("", false, ref lblChannel);
            if (Channel.Id == 0)
                ChkPower("006");
            else
            {
                if (!IsPower("006"))
                    ChkPower(Channel.Id + "-4");
            }
        }

        protected override void editBox()
        {
            plEdit.Visible = true;
            plList.Visible = false;
            DataTable dt;
            bool isTemplate = true;
            if (!IsPostBack)
            {
                doh.Reset();
                if (Channel.Type != "System")
                    doh.SqlCmd = "select id,title from [xk_template] where type='" + Channel.Type + "' and sType='Channel' order by isDefault desc";
                else
                    doh.SqlCmd = "select id,title from [xk_template] where type='Article' and sType='Channel' order by isDefault desc";

                dt = doh.GetDataTable();
                if (dt.Rows.Count < 1)
                    isTemplate = false;
                ddlTemplate.Items.Add(new ListItem("默认模板","0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlTemplate.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                }
                doh.SqlCmd = "select id,title from [xk_template] where sType='MoreDiss' order by isDefault desc";
                dt = doh.GetDataTable();
                if (dt.Rows.Count < 1)
                    isTemplate = false;
                ddlTemplate2.Items.Add(new ListItem("默认模板","0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlTemplate2.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                }
            }
            if (!isTemplate)
            {
                Alert("未找到模板，请先添加模板!");
                btnSave.Enabled = false;
            }
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_Channel", btnSave);
            wh.AddBind(txtName, "Title", true);
            wh.AddBind(txtInfo, "Info", true);
            wh.AddBind(rblType, "SelectedValue", "isOut", false);
            wh.AddBind(ddlType, "Type", false);
            wh.AddBind(txtDir, "dir", true);
            wh.AddBind(txtItemName, "ItemName", true);
            wh.AddBind(txtItemUnit, "ItemUnit", true);
            wh.AddBind(txtOuturl, "outUrl", true);
            wh.AddBind(rblTarget, "SelectedValue", "target", false);
            wh.AddBind(rblEnabled, "SelectedValue", "Enabled", false);
            wh.AddBind(ddlTemplate, "templateId", false);
            wh.AddBind(ddlTemplate2, "templateDiss", false);
            wh.AddBind(txtDissPageSize, "dissPageSize", false);
            wh.AddBind(rblIsPost, "SelectedValue", "ispost", false);
            wh.AddBind(rblIsReview, "SelectedValue", "isreview", false);
            rfvOutUrl.Enabled = false;
            txtOuturl.Enabled = false;
            if (Channel.Id > 0)
            {
                wh.ConditionExpress = "id=" + Channel.Id;
                wh.Mode = XkCms.DataOper.OperationType.Modify;
                rblType.Enabled = false;
                txtDir.Enabled = false;
                ddlType.Enabled = false;
            }
            else
            {
                wh.Mode = XkCms.DataOper.OperationType.Add;
            }
            wh.validator = ChkName;
            wh.BindBeforeModifyOk += new EventHandler(bind_ok);
            wh.AddOk += new EventHandler(add_ok);
            wh.ModifyOk += new EventHandler(save_ok);
        }
        protected void bind_ok(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "1")
            {
                rfvOutUrl.Enabled = true;
                rfvDir.Enabled = false;
                rfvItemName.Enabled = false;
                txtItemName.Enabled = false;
                rfvItemUnit.Enabled = false;
                txtItemUnit.Enabled = false;
                rfvOutUrl.Enabled = true;
                txtOuturl.Enabled = true;
            }
        }
        protected bool ChkName()
        {
            doh.Reset();
            doh.ConditionExpress = "Title=@title and id<>@id";
            doh.AddConditionParameter("@title", txtName.Text);
            doh.AddConditionParameter("@id", Channel.Id);
            if (doh.Exist("Xk_Channel"))
            {
                Alert("频道名重复!");
                return false;
            }
            if (rblType.SelectedValue == "0")
            {
                doh.Reset();
                if (Channel.Id == 0)
                {
                    doh.ConditionExpress = "dir=@dir";
                    if (System.IO.Directory.Exists(Request.PhysicalApplicationPath + "\\" + txtDir.Text))
                    {
                        Alert("名为" + txtDir.Text + "的文件夹已存在!");
                        return false;
                    }
                }
                else
                    doh.ConditionExpress = "dir=@dir and id<>" + Channel.Id;
                doh.AddConditionParameter("@dir", txtDir.Text);
                if (doh.Exist("Xk_Channel"))
                {
                    Alert("目录名重复!");
                    return false;
                }
            }
            return true;
        }
        protected void add_ok(object sender, EventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id>0 order by pId desc";
            string pId = doh.GetValue("Xk_Channel", "pId").ToString();
            doh.Reset();
            doh.ConditionExpress = "Title=@title";
            doh.AddConditionParameter("@title", txtName.Text);
            doh.AddFieldItem("TopicNum", 0);
            doh.AddFieldItem("ReviewNum", 0);
            doh.AddFieldItem("pId", Validator.StrToInt(pId, 0) + 1);
            doh.Update("Xk_Channel");
            if (txtDir.Text.Length > 0)
            {
                DirFile.CreateDir(txtDir.Text);
                DirFile.CreateDir(txtDir.Text + "/List");
                DirFile.CreateDir(txtDir.Text + "/View");
                DirFile.CreateDir(txtDir.Text + "/UserFiles");
                DirFile.CreateDir(txtDir.Text + "/Js");

                XkCms.DataOper.Data.DbOperEventArgs de = (XkCms.DataOper.Data.DbOperEventArgs)e;
                CopyFiles(txtDir.Text, de.id.ToString());
            }
            if (rblType.SelectedValue == "0")
                JsExe("Reload", "top.frames['fleft'].location=top.frames['fleft'].location");
            GetList();
        }
        protected void save_ok(object sender, EventArgs e)
        {
            if (txtDir.Text.Length > 0)
            {
                DirFile.CreateDir(txtDir.Text);
                DirFile.CreateDir(txtDir.Text + "/List");
                DirFile.CreateDir(txtDir.Text + "/View");
                DirFile.CreateDir(txtDir.Text + "/UserFiles");
                DirFile.CreateDir(txtDir.Text + "/Js");
                CopyFiles(txtDir.Text, Channel.Id.ToString());
            }
            Alert("修改成功");
        }
        protected override void getListBox()
        {
            plList.Visible = true;
            plEdit.Visible = false;
            doh.Reset();
            doh.SqlCmd = "Select id,title,type,pId,ItemName,dir,isout,outurl,target,Enabled from [Xk_Channel] order by pid";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
        }
        public string ChkEnabled(string en)
        {
            if (en == "1")
                return "禁用";
            else
                return "<font color='red'>启用</a>";
        }
        public string ChkTarget(string ta)
        {
            if (ta == "1")
                return "新窗口";
            else
                return "原窗口";
        }
        public string ChkOut(string isOut, string a, string dir, string url)
        {
            if (a == "0")
            {
                if (isOut == "-1")
                    return "<font color='blue'>系统频道</a>";
                else if (isOut == "0")
                    return "<font color='green'>内部频道</a>";
                else
                    return "<font color='red'>外部频道</a>";
            }
            else
            {
                if (isOut == "1")
                    return "<font color='red'>" + url + "</a>";
                else
                    return "目录：" + dir;
            }
        }

        protected void upChannel(object sender, CommandEventArgs e)
        {
            moveChannel(e.CommandArgument.ToString(), 1);
        }

        protected void downChannel(object sender, CommandEventArgs e)
        {
            moveChannel(e.CommandArgument.ToString(), -1);
        }

        private void moveChannel(string id, int isUp)
        {
            if (id == "0") return;
            doh.Reset();
            doh.ConditionExpress = "id=@id";
            doh.AddConditionParameter("@id", id);
            string pId = doh.GetValue("Xk_Channel", "pId").ToString();

            string temp;
            doh.Reset();
            if (isUp == 1)
            {
                doh.ConditionExpress = "pId<@pId order by pId desc";
                doh.AddConditionParameter("@pId", pId);
            }
            else
            {
                doh.ConditionExpress = "pId>@pId order by pId";
                doh.AddConditionParameter("@pId", pId);
            }
            temp = doh.GetValue("Xk_Channel", "pId").ToString();

            doh.Reset();
            doh.ConditionExpress = "pId=@pId";
            doh.AddConditionParameter("@pId", temp);
            doh.AddFieldItem("pId", "-8888888");
            doh.Update("Xk_Channel");
            doh.Reset();
            doh.ConditionExpress = "id=@id";
            doh.AddConditionParameter("@id", id);
            doh.AddFieldItem("pId", temp);
            doh.Update("Xk_Channel");
            doh.Reset();
            doh.ConditionExpress = "pId=@pId";
            doh.AddConditionParameter("@pId", "-8888888");
            doh.AddFieldItem("pId", pId);
            doh.Update("Xk_Channel");
            JsExe("Reload", "top.frames['fleft'].location=top.frames['fleft'].location");
            getListBox();
        }
        protected void doEnabled(object sender, CommandEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=@id";
            doh.AddConditionParameter("@id", e.CommandArgument.ToString());
            doh.Audit("Xk_Channel", "Enabled");
            JsExe("Reload", "top.frames['fleft'].location=top.frames['fleft'].location");
            getListBox();
        }
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 8, 3);
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string cId = gvList.DataKeys[e.RowIndex].Value.ToString();
            doh.Reset();
            doh.ConditionExpress = "id=" + cId;
            string cType = doh.GetValue("Xk_Channel", "Type").ToString();
            if (doh.GetValue("Xk_Channel", "isout").ToString() == "-1")
            {
                Alert("系统频道，不允许删除！");
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + cId;
            doh.Delete("Xk_" + cType);
            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + cId;
            doh.Delete("Xk_Column");
            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + cId;
            doh.Delete("Xk_Diss");
            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + cId;
            doh.Delete("Xk_Source");
            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + cId;
            doh.Delete("Xk_FriendLink");
            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + cId;
            doh.Delete("Xk_Placard");
            doh.Reset();
            doh.ConditionExpress = "ChannelId=" + cId;
            doh.Delete("Xk_Vote");
            doh.Reset();
            doh.ConditionExpress = "id=" + cId;
            doh.Delete("Xk_Channel");
            JsExe("Reload", "alert('删除成功,请手工删除相应目录!');top.frames['fleft'].location=top.frames['fleft'].location");
            getListBox();
        }
        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "1")
            {
                rfvDir.Enabled = false;
                txtDir.Enabled = false;
                txtDir.Text = "";
                rfvItemName.Enabled = false;
                txtItemName.Enabled = false;
                txtItemName.Text = "";
                rfvItemUnit.Enabled = false;
                txtItemUnit.Enabled = false;
                txtItemUnit.Text = "";
                ddlType.Enabled = false;
                rfvOutUrl.Enabled = true;
                txtOuturl.Enabled = true;
            }
            else
            {
                rfvOutUrl.Enabled = false;
                txtOuturl.Text = "";
                txtOuturl.Enabled = false;
                rfvDir.Enabled = true;
                txtDir.Enabled = true;
                rfvItemName.Enabled = true;
                txtItemName.Enabled = true;
                rfvItemUnit.Enabled = true;
                txtItemUnit.Enabled = true;
                ddlType.Enabled = true;
            }
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            doh.Reset();
            doh.SqlCmd = "select id,title from [xk_template] where type='" + ddlType.SelectedValue + "' and sType='Channel'";
            ddlTemplate.DataSource = doh.GetDataTable();
            ddlTemplate.DataTextField = "title";
            ddlTemplate.DataValueField = "id";
            ddlTemplate.DataBind();
        }

        protected void CopyFiles(string cDir, string TempId)
        {
            if (Directory.Exists(Request.PhysicalApplicationPath + "\\Controls"))
            {
                string TempStr = string.Empty;
                StreamWriter sw;
                if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx"))
                {
                    TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx");
                    TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                    sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Default.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                    sw.Write(TempStr);
                    sw.Close();
                }
                if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx"))
                {
                    TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx");
                    TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                    sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\List.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                    sw.Write(TempStr);
                    sw.Close();
                }
                if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx"))
                {
                    TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx");
                    TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                    sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\View.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                    sw.Write(TempStr);
                    sw.Close();
                }
                if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx"))
                {
                    TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx");
                    TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                    sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\DissList.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                    sw.Write(TempStr);
                    sw.Close();
                }
                if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx"))
                {
                    TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx");
                    TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                    sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Diss.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                    sw.Write(TempStr);
                    sw.Close();
                }
                if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx"))
                {
                    TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx");
                    TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                    sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Review.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                    sw.Write(TempStr);
                    sw.Close();
                }
                if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx"))
                {
                    TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx");
                    TempStr = TempStr.Replace("{$ChannelId$}", TempId);

                    sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Search.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
                    sw.Write(TempStr);
                    sw.Close();
                }
            }
        }
    }
}
