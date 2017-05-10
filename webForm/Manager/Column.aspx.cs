using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;
using System.IO;

namespace XkCms.WebForm.Manager
{
    public partial class Column : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("", true, ref lblChannel);
            ChkPower(Channel.Id + "-3");
        }

        protected override void editBox()
        {
            plList.Visible = false;
            plEdit.Visible = true;
            bool isTemplate = true;
            if (!IsPostBack)
            {
                doh.Reset();
                doh.SqlCmd = "select id,title,code from [xk_Column] where ChannelId=" + Channel.Id + " and isout=0 order by code";
                DataTable dt = doh.GetDataTable();
                ddlColumn.Items.Add(new ListItem("根目录", "0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Value = dt.Rows[i][0].ToString();
                    li.Text = GetColumnListName(dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString());
                    ddlColumn.Items.Add(li);
                }
                doh.Reset();
                doh.SqlCmd = "select id,title from [xk_template] where type='" + Channel.Type + "' and sType='Column' order by isDefault desc";
                dt = doh.GetDataTable();
                if (dt.Rows.Count < 1)
                    isTemplate = false;
                ddlTemplate.Items.Add(new ListItem("默认模板","0"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlTemplate.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                }
                doh.Reset();
                doh.SqlCmd = "select id,title from [xk_template] where type='" + Channel.Type + "' and sType='Content' order by isDefault desc";
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
                btnSaveColumn.Enabled = false;
            }
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "Xk_Column", btnSaveColumn);
            wh.AddBind(txtName, "Title", true);
            wh.AddBind(ddlColumn, "ParentId", false);
            wh.AddBind(txtInfo, "Info", true);
            wh.AddBind(rblIsOut, "SelectedValue", "isOut", false);
            wh.AddBind(txtOuturl, "outUrl", true);
            wh.AddBind(rblIsPost, "SelectedValue", "isPost", false);
            wh.AddBind(rblIsReview, "SelectedValue", "isReview", false);
            wh.AddBind(rblTop, "SelectedValue", "isTop", false);
            wh.AddBind(Channel.Id.ToString(), "ChannelId", false);
            wh.AddBind(ddlTemplate, "TemplateId", false);
            wh.AddBind(ddlTemplate2, "ContentTemp", false);
            wh.AddBind(txtPageSize, "pagesize", false);
            txtOuturl.Enabled = false;
            rfvOutUrl.Enabled = false;
            if (id == "0")
            {
                wh.Mode = XkCms.DataOper.OperationType.Add;
                if (Channel.IsPost)
                    rblIsPost.Items.FindByValue("1").Selected = true;
                else
                    rblIsPost.Items.FindByValue("0").Selected = true;
            }
            else
            {
                wh.ConditionExpress = "id=" + id;
                wh.Mode = XkCms.DataOper.OperationType.Modify;
            }
            wh.BindBeforeModifyOk += new EventHandler(bind_ok);
            wh.AddOk += new EventHandler(save_ok);
            wh.ModifyOk += new EventHandler(save_ok);
        }
        protected void bind_ok(object sender, EventArgs e)
        {
            if (rblIsOut.SelectedValue == "1")
            {
                txtOuturl.Enabled = true;
                rfvOutUrl.Enabled = true;
                rblIsPost.Enabled = false;
                rblIsReview.Enabled = false;
            }
        }
        protected void save_ok(object sender, EventArgs e)
        {
            string parentCode = string.Empty;
            if (ddlColumn.SelectedValue != "0")
            {
                doh.Reset();
                doh.ConditionExpress = "id=@id";
                doh.AddConditionParameter("@id", ddlColumn.SelectedValue);
                parentCode = doh.GetValue("Xk_Column", "code").ToString();
            }
            string leftCode = string.Empty;
            string selfCode = string.Empty;
            string oldCode = string.Empty;
            string sourceCode = string.Empty;

            bool isEditCode = true;

            if (id != "0")
            {
                doh.Reset();
                doh.ConditionExpress = "id=@id";
                doh.AddConditionParameter("@id", id);
                oldCode = doh.GetValue("Xk_Column", "code").ToString();
                sourceCode = oldCode;
                oldCode = oldCode.Substring(0, oldCode.Length - 4);
                if (oldCode == parentCode)
                    isEditCode = false;
            }

            if (isEditCode)
            {
                doh.Reset();
                doh.SqlCmd = "select code from [Xk_Column] where left(code," + parentCode.Length.ToString() + ")='" + parentCode + "' and len(code)=" + Convert.ToString(parentCode.Length + 4) + " and ChannelId=" + Channel.Id + " order by code desc";
                DataTable dt = doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    leftCode = dt.Rows[0][0].ToString();
                }
                if (leftCode.Length > 0)
                    selfCode = Convert.ToString(Convert.ToInt32(leftCode.Substring(leftCode.Length - 4, 4)) + 1).PadLeft(4, '0');
                else
                    selfCode = "0001";

                selfCode = parentCode + selfCode;

                if (id == "0")
                {
                    XkCms.DataOper.Data.DbOperEventArgs de = (XkCms.DataOper.Data.DbOperEventArgs)e;
                    doh.Reset();
                    doh.ConditionExpress = "id=@id";
                    doh.AddConditionParameter("@id", de.id);
                    doh.AddFieldItem("Code", selfCode);
                    doh.Update("Xk_Column");
                }
                else
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + id;
                    doh.AddFieldItem("Code", selfCode);
                    doh.Update("Xk_Column");
                    doh.Reset();
                    doh.SqlCmd = "update [xk_Column] Set code='" + selfCode + "'+Right(code,len(code)-" + sourceCode.Length.ToString() + ") where code like '" + sourceCode + "%'";
                    doh.ExecuteSqlNonQuery();
                }
            }
            GetList();
        }

        protected override void getListBox()
        {
            plEdit.Visible = false;
            plList.Visible = true;
            doh.Reset();
            doh.SqlCmd = "select id,title,Code,isOut,ChannelId,isTop from [Xk_Column] where ChannelId=" + Channel.Id + " order by code";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
        }
        protected void upColumn(object sender, CommandEventArgs e)
        {
            moveColumn(Convert.ToInt32(e.CommandArgument), 1);
        }
        protected void downColumn(object sender, CommandEventArgs e)
        {
            moveColumn(Convert.ToInt32(e.CommandArgument), -1);
        }

        private void moveColumn(int id, int isUp)
        {
            if (id == 0) return;
            doh.Reset();
            doh.ConditionExpress = "id=" + id.ToString();
            string oldCode = doh.GetValue("xk_Column", "code").ToString();
            int codeLen = oldCode.Length;
            string subStr = dbType == "0" ? "mid" : "substring";
            if (codeLen > 1)
            {
                string temp = string.Empty;
                string wStr = "";
                string newStr = "";
                for (int i = 0; i < codeLen; i++)
                    newStr += "-";
                if (codeLen > 4)
                    wStr = " and left(code," + Convert.ToString(codeLen - 4) + ")='" + oldCode.Substring(0, codeLen - 4) + "'";

                if (isUp == 1)
                    wStr = "select top 1 code from xk_Column where len(code)=" + codeLen.ToString() + " and code<'" + oldCode + "'" + wStr + " order by code desc";
                else
                    wStr = "select top 1 code from xk_Column where len(code)=" + codeLen.ToString() + " and code>'" + oldCode + "'" + wStr + " order by code";
                doh.Reset();
                doh.SqlCmd = wStr;
                DataTable dt = doh.GetDataTable();
                if (dt.Rows.Count > 0)
                    temp = dt.Rows[0][0].ToString();

                if (temp.Length > 1)
                {
                    //Move Under Column
                    wStr = "update [xk_Column] set code='" + newStr + "'+" + subStr + "(code," + Convert.ToString(codeLen + 1) + ",len(code)) where left(code," + codeLen.ToString() + ")='" + temp + "'";
                    doh.Reset();
                    doh.SqlCmd = wStr;
                    doh.ExecuteSqlNonQuery();

                    //Update Target Column
                    wStr = "update [xk_Column] set code='" + temp + "'+" + subStr + "(code," + Convert.ToString(codeLen + 1) + ",len(code)) where left(code," + codeLen.ToString() + ")='" + oldCode + "'";
                    doh.Reset();
                    doh.SqlCmd = wStr;
                    doh.ExecuteSqlNonQuery();

                    //Update Under Column
                    wStr = "update [xk_Column] set code='" + oldCode + "'+" + subStr + "(code," + Convert.ToString(newStr.Length + 1) + ",len(code)) where left(code," + newStr.Length.ToString() + ")='" + newStr + "'";
                    doh.Reset();
                    doh.SqlCmd = wStr;
                    doh.ExecuteSqlNonQuery();

                    if (dbType == "0")
                    {
                        wStr = "update [xk_" + Channel.Type + "] a left join [xk_Column] b on a.columnId=b.id set a.columnCode=b.code,a.columnName=b.title where a.columncode like ";
                        doh.Reset();
                        doh.SqlCmd = wStr + "'" + oldCode + "%'";
                        doh.ExecuteSqlNonQuery();

                        doh.Reset();
                        doh.SqlCmd = wStr + "'" + temp + "%'";
                        doh.ExecuteSqlNonQuery();
                    }
                    else
                    {
                        doh.Reset();
                        doh.SqlCmd = "select id,code,title from [xk_Column] where code like '" + oldCode + "%'";
                        dt = doh.GetDataTable();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            doh.Reset();
                            doh.SqlCmd = "update [xk_" + Channel.Type + "] set columnCode ='" + dt.Rows[i][1].ToString() + "',columnName='" + dt.Rows[i][2].ToString() + "' where columnId=" + dt.Rows[i][0].ToString();
                            doh.ExecuteSqlNonQuery();
                        }
                        doh.Reset();
                        doh.SqlCmd = "select id,code,title from [xk_Column] where code like '" + temp + "%'";
                        dt = doh.GetDataTable();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            doh.Reset();
                            doh.SqlCmd = "update [xk_" + Channel.Type + "] set columnCode ='" + dt.Rows[i][1].ToString() + "',columnName='" + dt.Rows[i][2].ToString() + "' where columnId=" + dt.Rows[i][0].ToString();
                            doh.ExecuteSqlNonQuery();
                        }
                    }
                }
            }
            getListBox();
        }
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 3, 1);
        }
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string cColumnId = gvList.DataKeys[e.RowIndex].Value.ToString();
            doh.Reset();
            doh.ConditionExpress = "id=" + cColumnId;
            string cCode = doh.GetValue("xk_Column", "code").ToString();
            doh.Reset();
            doh.ConditionExpress = " code like '" + cCode + "%' and len(code)>" + cCode.Length;
            bool isDel = doh.Exist("xk_Column");
            if (isDel)
            {
                Alert("请先将本栏目的下级栏目移动别处或删除之后再进行此操作!");
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "columnId=" + cColumnId;
            isDel = doh.Exist("xk_" + Channel.Type);
            if (isDel)
            {
                Alert("请先将本栏目下的所有内容移动别处或删除之后再进行此操作!");
                return;
            }
            if (Directory.Exists(Request.PhysicalApplicationPath + Channel.Dir + "\\List"))
            {
                string[] htmFiles = Directory.GetFiles(Request.PhysicalApplicationPath + Channel.Dir + "\\List", cColumnId + "_*.htm");
                foreach (string fileName in htmFiles)
                {
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + cColumnId;
            doh.Delete("Xk_Column");
            getListBox();
        }
        protected string chkOut(string isOut)
        {
            if (isOut == "1")
                return "<font color='red'>[外部]</font>";
            else
                return "<font color='green'>[内部]</font>";
        }
        protected void rblIsOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblIsOut.SelectedValue == "1")
            {
                txtOuturl.Enabled = true;
                rfvOutUrl.Enabled = true;
                rblIsPost.Enabled = false;
                rblIsReview.Enabled = false;
            }
            else
            {
                txtOuturl.Enabled = false;
                rfvOutUrl.Enabled = false;
                rblIsPost.Enabled = true;
                rblIsReview.Enabled = true;
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            CreateMenu();
            Alert("生成完成");
        }
    }
}
