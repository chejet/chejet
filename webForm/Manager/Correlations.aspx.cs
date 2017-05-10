using System;
using System.Data;
using System.Web;
using XkCms.WebForm.BaseFunction;
using System.Web.UI.WebControls;

namespace XkCms.WebForm.Manager
{
    public partial class Correlations : Admin
    {
        private string ContentIDs = string.Empty;
        private string Keys = string.Empty;
        private string ChannelType;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<link href='images/style.css' type=text/css rel=stylesheet>");
            Response.Write("<script type='text/javascript' src='../js/prototype.js'></script>");
            Response.Write("<script language='JavaScript' src='../Js/admin.js'></SCRIPT>");
            ChkLogin();
            ChannelId = q("ChannelId");
            if (!XkCms.Common.Utils.Validator.IsNumberId(ChannelId))
            {
                ErrMsg = "请不要从外部提交数据!";
                ShowErrMsg();
            }
            id = q("id");
            if (!XkCms.Common.Utils.Validator.IsNumberId(id))
                id = "0";

            doh.Reset();
            doh.ConditionExpress = "id=" + ChannelId;
            ChannelType = doh.GetValue("Xk_Channel", "type").ToString();

            if (!IsPostBack)
            {
                ContentIDs = q("ContentIDs");
                HiddenFieldContentIDs.Value = ContentIDs;
                Keys = q("keys");
                HiddenFieldKeys.Value = Keys;
            }
            else
            {
                ContentIDs = HiddenFieldContentIDs.Value.Trim();
                Keys = HiddenFieldKeys.Value.Trim();
            }
            if (Keys.Trim() == "") btnAutoSelect.Enabled = false;
            if (ContentIDs.Trim() == "") ContentIDs = "0";
            getListBox();
        }

        protected override void getListBox()
        {
            doh.Reset();
            doh.SqlCmd = "select id,title from [xk_" + ChannelType + "] where id in (" + ContentIDs + ") and ChannelId=" + ChannelId + " order by id desc";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataBind();
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 0, 0);
        }

        protected void ButtonReFresh_Click(object sender, EventArgs e)
        {
            string[] hfcids = this.HiddenFieldContentIDs.Value.Split(',');
            string cids = string.Empty;
            for (int i = 0; i < hfcids.Length; i++)
            {
                if (hfcids[i] != id)
                {
                    if (cids.IndexOf(hfcids[i]) == -1)
                        cids += hfcids[i] + ",";
                }
            }
            if (cids.Length > 0)
                cids = cids.Substring(0, cids.Length - 1);
            Response.Redirect("Correlations.aspx?ChannelId=" + ChannelId + "&id=" + id + "&ContentIDs=" + cids + "&keys=" + Keys, true);
        }

        protected void btnAutoSelect_Click(object sender, EventArgs e)
        {
            string[] key = Keys.Split(',');
            doh.Reset();
            doh.SqlCmd = "select id from [xk_" + ChannelType + "] where ChannelId=" + ChannelId + " and (KeyWord like '%" + key[0] + "%'";
            if (key.Length > 1)
            {
                for (int i = 1; i < key.Length; i++)
                    doh.SqlCmd += " or KeyWord like '%" + key[i] + "%'";
            }
            doh.SqlCmd += ") order by id desc";
            DataTable dt = doh.GetDataTable();
            ContentIDs = string.Empty;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                    ContentIDs += "," + dt.Rows[i][0].ToString();
                ContentIDs = ContentIDs.Substring(1);
                HiddenFieldContentIDs.Value = ContentIDs;
                Response.Redirect("Correlations.aspx?ChannelId=" + ChannelId + "&id=" + id + "&ContentIDs=" + ContentIDs + "&keys=" + Keys, true);
            }
        }
    }
}
