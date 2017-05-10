using System;
using System.Data;
using System.Web.UI.WebControls;
using XkCms.WebForm.BaseFunction;

namespace XkCms.WebForm.Manager
{
    public partial class Master : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("011");
        }

        protected override void editBox()
        {
            plAdd.Visible = true;
            plList.Visible = false;
            plEdit.Visible = false;
            XkCms.DataOper.Data.WebFormHandler wh = new XkCms.DataOper.Data.WebFormHandler(doh, "xk_master", btnSaveMaster);
            wh.AddBind(txtMasterName, "master_name", true);
            wh.AddBind(rbtnMasterState, "SelectedValue", "state", false);
            if (id == "0")
            {
                wh.Mode = XkCms.DataOper.OperationType.Add;
                RequiredPass.Enabled = true;
            }
            else
            {
                wh.ConditionExpress = "master_id=" + id;
                wh.Mode = XkCms.DataOper.OperationType.Modify;
                RequiredPass.Enabled = false;
            }
            wh.validator = chkName;
            wh.AddOk += new EventHandler(save_ok);
            wh.ModifyOk += new EventHandler(save_ok);
        }

        void save_ok(object sender, EventArgs e)
        {
            if (txtMasterPass.Text != "")
            {
                if (id == "0")
                {
                    XkCms.DataOper.Data.DbOperEventArgs de = (XkCms.DataOper.Data.DbOperEventArgs)e;
                    id = de.id.ToString();
                }
                doh.Reset();
                doh.ConditionExpress = "master_id=" + id;
                doh.AddFieldItem("master_password", XkCms.Common.Utils.Tools.GetMD5(txtMasterPass.Text));
                doh.Update("xk_master");
            }
            GetList();
        }

        protected bool chkName()
        {
            doh.Reset();
            if (id == "0")
            {
                if (txtMasterPass.Text == "")
                {
                    Alert("请填写密码");
                    return false;
                }
                doh.SqlCmd = "select master_id from xk_master where master_name='" + txtMasterName.Text + "'";
            }
            else
                doh.SqlCmd = "select master_id from xk_master where master_name='" + txtMasterName.Text + "' and master_id<>" + id;
            if (doh.GetDataTable().Rows.Count > 0)
            {
                Alert("用户名重复");
                return false;
            }
            return true;
        }

        protected override void getListBox()
        {
            plAdd.Visible = false;
            plList.Visible = true;
            plEdit.Visible = false;
            doh.Reset();
            doh.SqlCmd = "select master_ID,master_name,lastime,lastip,state from [xk_master] order by master_ID desc";
            gvMasterList.DataSource = doh.GetDataTable();
            gvMasterList.DataKeyNames = new string[] { "master_ID" };
            gvMasterList.DataBind();
        }

        protected void gvMasterList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 5, 3);
        }
        protected void gvMasterList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Session["master_id"].ToString() == gvMasterList.DataKeys[e.RowIndex].Value.ToString())
                return;
            doh.Reset();
            doh.ConditionExpress = "Master_id=" + gvMasterList.DataKeys[e.RowIndex].Value.ToString();
            doh.Delete("xk_master");
            getListBox();
        }
        protected void gvMasterList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            plAdd.Visible = false;
            plList.Visible = false;
            plEdit.Visible = true;

            string[,] menu = leftMenu();
            id = gvMasterList.DataKeys[e.NewEditIndex].Value.ToString();
            hfMasterSettingId.Value = id;
            doh.Reset();
            doh.ConditionExpress = "Master_id=" + id;
            string admin_power = doh.GetValue("xk_Master", "setting").ToString();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<table width='98%' cellpadding=\"5\" cellspacing=\"0\" border=\"0\" class=tableBorder>");
            sb.Append("<tr><th colspan=\"2\" align=\"center\">用户权限设置</th></tr>");
            for (int i = 0; i < menu.GetLength(0); i++)
            {
                if (menu[i, 0] == null) break;
                string[] channelNum = menu[i, 0].Split('$');
                sb.Append("<tr><td height=25 class=forumRowHighlight colspan=2 align=left>&nbsp;<b>" + channelNum[0] + "</b></td></tr>");
                sb.Append("<tr><td height=20 class=forumrow colspan=2 align=left>");
                for (int j = 1; j < menu.GetLength(1); j++)
                {
                    if (menu[i, j] == null)
                        break;
                    sb.Append("<span style='height:25px;line-height:25px;margin-left:16px;padding-top:5px;'><input type=checkbox class='checkbox' name=\"admin_power\" value=\"");
                    string tPower = string.Empty;
                    if (i < 3)
                        tPower = channelNum[1] + i.ToString() + j.ToString();
                    else
                        tPower = channelNum[1] + "-" + j.ToString();
                    sb.Append(tPower + "\"");
                    if (admin_power.IndexOf("," + tPower + ",") > -1)
                        sb.Append(" checked");
                    sb.Append(">" + tPower + "." + menu[i, j] + "</span>");
                }
                sb.Append("</td></tr>");
            }
            sb.Append("</table>");
            ltMasterSetting.Text = sb.ToString();
        }

        protected void btnSaveSetting_Click(object sender, EventArgs e)
        {
            string admin_power = ",";
            if (Request.Form["admin_power"] != null)
                admin_power = "," + Request.Form["admin_power"].ToString() + ",";
            id = hfMasterSettingId.Value.ToString();
            if (id == "0" || !XkCms.Common.Utils.Validator.IsNumberId(id))
                Alert("参数错误,请重新操作!");
            else
            {
                doh.Reset();
                doh.ConditionExpress = "Master_id=" + id;
                doh.AddFieldItem("setting", admin_power);
                doh.Update("xk_master");
            }
            getListBox();
        }

        public string chkMasterState(string state)
        {
            if (state == "1")
                return "正常";
            else
                return "<font color='red'>锁定</font>";
        }
    }
}
